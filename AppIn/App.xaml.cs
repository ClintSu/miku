using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AppIn
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            KillProcess("MiKu");
         
            base.OnStartup(e);
        }
        private void KillProcess(string processName)
        { 
            try
            {
                //获得需要杀死的进程名  
                foreach (Process thisproc in Process.GetProcessesByName(processName))
                {
                    //立即杀死进程
                    thisproc.Kill();
                }
            }
            catch (Exception Exc)
            {
                throw new Exception("", Exc);
            }
        }
    }
}
