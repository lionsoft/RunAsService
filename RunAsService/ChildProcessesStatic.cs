using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace RunAsService
{
    public sealed partial class ChildProcesses
    {
        public static bool Start(string[] args)
        {
            var res = args.Length > 0 ? Load(args[0]) : Load();
            if (res) _instance.DoRun();
            return res;
        }

        public static bool Load(string configFileName = "")
        {
            return _instance.DoLoad(configFileName);
        }

        public static void Run()
        {
            _instance.DoRun();
        }

        public static void Stop()
        {
            _instance.DoStop();
        }

        public static void Pause()
        {
            _instance.Suspend();
        }

        public static void Continue()
        {
            _instance.Resume();
        }
    }
}
