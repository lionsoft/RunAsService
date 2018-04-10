using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml;
using RunAsService.Utils;

namespace RunAsService
{
    public class ChildProcess
    {
        public enum StartModes
        {
            Auto,
            Manual
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public StartModes StartMode { get; set; }
        public string CmdLine { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }

        internal ProcessStartInfo Psi = new ProcessStartInfo();
        internal Process Prc;

        public ChildProcess(XmlNode xml)
        {
            Name = xml.Name;
            xml.ChildNodes.ForEach<XmlNode>(node =>
                {
                    if (node.Name.SameText("Description")) Description = node.InnerText;
                    else if (node.Name.SameText("StartMode"))
                        StartMode = node.InnerText == "manual" ? StartModes.Manual : StartModes.Auto;
                    else if (node.Name.SameText("CmdLine")) CmdLine = node.InnerText;
                    else if (node.Name.SameText("Account"))
                    {
                        node.ChildNodes.ForEach<XmlNode>(accNode =>
                            {
                                if (accNode.Name.SameText("User")) UserName = accNode.InnerText;
                                else if (node.Name.SameText("Password")) UserPassword = accNode.InnerText;
                            });
                    }
                });
        }

        public void Suspend()
        {
            Prc?.Suspend();
        }

        public void Resume()
        {
            Prc?.Resume();
        }

        public void Restart()
        {
            Stop();
            Start();
        }

        public void Stop()
        {
            if (Prc == null) return;
            Prc.EnableRaisingEvents = false;
            Prc.Kill();
            Prc = null;
        }

        public void Start()
        {
            var cmdLine = CmdLine;
            Psi.FileName = StringsUtils.SeparateText(ref cmdLine, ' ');
            Psi.Arguments = cmdLine;

            Prc = Process.Start(Psi);
            if (Prc != null)
            {
                Prc.EnableRaisingEvents = true;
                Prc.Exited += (sender, e) =>
                {
                    if (Prc != null)
                    {
                        Prc = null;
                        Start();
                    }
                };
                var j = new Job();
                var res = j.AddProcess(Prc.Handle);
                if (!res)
                {
                    var b = Marshal.GetLastWin32Error();
                    Console.WriteLine($"Error {b}");
                }
            }
        }
    }
}

