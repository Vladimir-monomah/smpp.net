using System;
using System.Windows.Forms;
using Inetlab.SMPP;
using Inetlab.SMPP.Common;

namespace SmppServerDemo
{
    public partial class SendMessage : Form
    {
        readonly SmppServerClient _client;

        public SendMessage(SmppServerClient client )
        {
            this.InitializeComponent();

            this._client = client;

            if (this._client != null)
            {
                this.lClient.Text = this._client.ToString();
            }

        }

        private async void bSubmit_Click(object sender, EventArgs e)
        {
            if (this._client != null)
            {
                SmeAddress source = new SmeAddress(this.tbSrcAdr.Text, (AddressTON)byte.Parse(this.tbSrcAdrTON.Text), (AddressNPI)byte.Parse(this.tbSrcAdrNPI.Text));
                SmeAddress destination = new SmeAddress(this.tbDestAdr.Text, (AddressTON)byte.Parse(this.tbDestAdrTON.Text), (AddressNPI)byte.Parse(this.tbDestAdrNPI.Text));

                await this._client.DeliverAsync(SMS.ForDeliver()
                    .From(source)
                    .To(destination)
                    .Coding(this.GetDataCoding())
                    .Text(this.tbSend.Text)
                    );

                this.DialogResult = DialogResult.OK;
            }

        }

        private DataCodings GetDataCoding()
        {
            return (DataCodings)Enum.Parse(typeof(DataCodings), this.cbDataCoding.Text);
        }

        private void SendMessage_Load(object sender, EventArgs e)
        {
            this.cbDataCoding.SelectedIndex = 0;
        }
    }
}