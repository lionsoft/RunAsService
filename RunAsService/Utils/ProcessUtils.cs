using System;
using System.Diagnostics;

namespace RunAsService.Utils
{
    public static class ProcessUtils
    {
        private static string FindIndexedProcessName(int pid)
        {
            var processName = Process.GetProcessById(pid).ProcessName;
            var processesByName = Process.GetProcessesByName(processName);
            string processIndexdName = null;

            for (var index = 0; index < processesByName.Length; index++)
            {
                processIndexdName = index == 0 ? processName : processName + "#" + index;
                var processId = new PerformanceCounter("Process", "ID Process", processIndexdName);
                if (processId.NextValue().As<int>().Equals(pid))
                {
                    return processIndexdName;
                }
            }

            return processIndexdName;
        }

        private static Process FindPidFromIndexedProcessName(string indexedProcessName)
        {
            var parentId = new PerformanceCounter("Process", "Creating Process ID", indexedProcessName);
            return Process.GetProcessById((int) parentId.NextValue());
        }

        public static Process Parent(this Process process)
        {
            return FindPidFromIndexedProcessName(FindIndexedProcessName(process.Id));
        }

        public static void Suspend(this Process process)
        {
            foreach (ProcessThread pT in process.Threads)
            {
                var pOpenThread = PInvoke.OpenThread(PInvoke.ThreadAccess.SUSPEND_RESUME, false, (uint) pT.Id);
                if (pOpenThread == IntPtr.Zero) break;
                PInvoke.SuspendThread(pOpenThread);
            }
        }

        public static void Resume(this Process process)
        {
            foreach (ProcessThread pT in process.Threads)
            {
                var pOpenThread = PInvoke.OpenThread(PInvoke.ThreadAccess.SUSPEND_RESUME, false, (uint) pT.Id);
                if (pOpenThread == IntPtr.Zero) break;
                PInvoke.ResumeThread(pOpenThread);
            }
        }
   }
}
