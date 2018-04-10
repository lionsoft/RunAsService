using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Xml;
using RunAsService.Utils;

namespace RunAsService
{
    public sealed partial class ChildProcesses : List<ChildProcess>
    {
        static ChildProcesses()
        {
            
        }

        private ChildProcesses()
        {
        }

        private static readonly ChildProcesses _instance = new ChildProcesses();

        private bool DoLoad(string configFileName = "")
        {
            if (string.IsNullOrEmpty(configFileName))
                configFileName = Path.ChangeExtension(Assembly.GetCallingAssembly().Location, "xml");
            DoStop();
            Clear();
            try
            {
                var xml = new XmlDocument();
                xml.Load(configFileName);
                xml.ChildNodes[1].ChildNodes.ForEach<XmlNode>(app => Add(new ChildProcess(app)));
            }
            catch (Exception)
            {
                Clear();
            }
            return Count > 0;
        }

        private void DoStop()
        {
            this.ForEach<ChildProcess>(p => p.Stop());
        }

        private void Suspend()
        {
            this.ForEach<ChildProcess>(p => p.Suspend());
        }

        private void Resume()
        {
            this.ForEach<ChildProcess>(p => p.Resume());
        }

        private void DoRun()
        {
            this.ForEach<ChildProcess>(p => p.Start());
        }
    }
}
