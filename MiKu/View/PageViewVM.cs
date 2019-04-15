using MiKu.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MiKu
{
    public class PageViewVM:ViewModelBase
    {
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged(() => Title);
            }
        }
 
        private List<AppFile> appFiles = new List<AppFile>();
        public List<AppFile> AppFiles
        {
            get { return appFiles; }
            set {
                appFiles = value;
                RaisePropertyChanged(() => AppFiles);
            }
        }

        public RelayCommand<object> Open { get; }

        public PageViewVM(string title,List<AppFile> appFiles)
        {
            this.Open = new RelayCommand<object>((param) => Start(param));
            Title = title;
            AppFiles = appFiles;
        }

        private void Start(object param)
        {
            var file = param as AppFile;
            var af = AppFiles.First(x => x.ExeName == file.ExeName);
            af.HitNumber++;

            try
            {
                string appFileName = "";
                if (file.ExeName == "Default") return; //预留占位

                if (string.IsNullOrEmpty(file.ExeFolderName))
                {
                    appFileName = Environment.CurrentDirectory + "\\tools\\"+ file.ExeName + ".exe";
                }
                else
                {
                    appFileName = Environment.CurrentDirectory + "\\tools\\" + file.ExeFolderName + "\\" +
                                  file.ExeName + ".exe";
                }

                if (!System.IO.File.Exists(appFileName))
                {
                    MessageBox.Show("应用不存在！");
                    return;
                }
                try
                {
                    Process proc = Process.Start(appFileName);
                    if (proc != null)
                    {
                        SetForegroundWindow(proc.MainWindowHandle);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    //throw;
                }

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(file.AppName + " 运行失败，" + ex.Message);
            }
        }
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
    }
}
