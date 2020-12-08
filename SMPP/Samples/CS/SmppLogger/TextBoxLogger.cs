using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Inetlab.SMPP.Logging;

namespace SmppLogger
{

    public class TextBoxLogFactory : ILogFactory
    {
        private readonly TextBox _textBox;
        private readonly LogLevel _minLevel;
        private readonly string fileName;
        private readonly DataStore<string> _logStore = new DataStore<string>();

        public TextBoxLogFactory(TextBox textBox, LogLevel minLevel, string fileName)
        {
            this._textBox = textBox;
            this._textBox.HandleCreated += (sender, args) => this._textBox.BeginInvoke(new Action(this.AddToTextBox));
            this._minLevel = minLevel;
            this.fileName = fileName;
        }

        public ILog GetLogger(string loggerName)
        {
            return new TextBoxLogger(loggerName, this._minLevel, this.AddToLog);
        }

        private int _isThrottling = 0;

        private void AddToLog(string text)
        {
            this._logStore.Append(text);

            if (this._textBox.IsHandleCreated && Interlocked.CompareExchange(ref this._isThrottling, 1, 0) == 0)
            {
                this._textBox.BeginInvoke(new Action(this.AddToTextBox));
            }
        }

        private void AddToTextBox()
        {
            while (this._logStore.HasData)
            {
                StringBuilder sb = new StringBuilder();
                foreach (string line in this._logStore.TakeWork())
                {
                    sb.AppendLine(line);
                }

                var logText = sb.ToString();
                this._textBox.AppendText(logText);
                using (var logWriter = new StreamWriter(new FileStream(this.fileName, FileMode.Append, FileAccess.Write)))
                {
                    logWriter.WriteLine(logText);
                }
            }

            Interlocked.Exchange(ref this._isThrottling, 0);
        }
        
        private class DataStore<T>
        {
            private readonly List<T> _data = new List<T>();

            public void Append(T data)
            {
                lock (this._data)
                {
                    this._data.Add(data);
                }
            }

            public bool HasData
            {
                get
                {
                    lock (this._data)
                    {
                        return this._data.Count > 0;
                    }
                }
            }

            public T[] TakeWork()
            {
                T[] result;
                lock (this._data)
                {
                    result = this._data.ToArray();
                    this._data.Clear();
                }

                return result;
            }
        }


        private class TextBoxLogger : ILog
        {
            private readonly string _loggerName;
            private readonly LogLevel _minLevel;
            private readonly Action<string> _append;


            public TextBoxLogger(string loggerName, LogLevel minLevel, Action<string> append)
            {
                this._loggerName = loggerName;
                this._minLevel = minLevel;
                this._append = append;
            }


            public bool IsEnabled(LogLevel level)
            {
                return level >= this._minLevel;
            }

            public void Write(LogLevel level, string message, Exception ex, params object[] args)
            {
                if (!this.IsEnabled(level)) return;

                this.AddToLog("{0} ({1}) {2}{3}", level, this._loggerName, string.Format(message, args),
                    ex != null ? ", Исключение: " + ex.ToString() : "");

            }

            private void AddToLog(string message, params object[] args)
            {
                this._append(string.Format("{0:HH:mm:ss.fff}: {1}", DateTime.Now, string.Format(message, args)));

            }

        }

    }
}
