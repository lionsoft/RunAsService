using System;
using System.ServiceProcess;

namespace RunAsService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                ChildProcesses.Start(args);
                Console.ReadKey();
                ChildProcesses.Stop();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] { new SvcMain() });
            }
        }
    }
}
