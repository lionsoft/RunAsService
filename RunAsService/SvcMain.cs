using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace RunAsService
{
  public partial class SvcMain : ServiceBase
  {
    public SvcMain()
    {
      InitializeComponent();
    }

    protected override void OnStart(string[] args)
    {
      ChildProcesses.Start(args);
    }

    protected override void OnStop()
    {
      ChildProcesses.Stop();
    }

    protected override void OnPause()
    {
      ChildProcesses.Pause();
    }

    protected override void OnContinue()
    {
      ChildProcesses.Continue();
    }
  }
}
