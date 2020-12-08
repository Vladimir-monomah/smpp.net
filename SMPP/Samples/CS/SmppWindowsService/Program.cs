using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using SmppWindowsService;

namespace SmppClientWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            //Fix Current dir for windows service
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);


            var servicesToRun = new ServiceBase[]
            {
                new SmppServerWindowsService(), 
                new SmppClientWindwsService(), 
            };

            ServiceBase.Run(servicesToRun);
        }
    }
}
