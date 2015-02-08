using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace RoboVance.WindowsAgent
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            var agent = new WindowsAgent();

            if (Environment.UserInteractive)
            {
                agent.StartConsole(args);
                Console.WriteLine("Press any key to stop...");
                Console.ReadKey();
                agent.StopConsole();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
            { 
                new WindowsAgent() 
            };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
