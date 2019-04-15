using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MiKu.models;

namespace MiKu
{
    public class MainWindowVM : ViewModelBase
    {
        private Messenger messenger;

        private List<models.AppFile> files = new List<AppFile>();

        private List<models.AppFile> appFiles = new List<models.AppFile>();
        public List<models.AppFile> AppFiles
        {
            get { return appFiles; }
            set
            {
                appFiles = value;
                RaisePropertyChanged(() => AppFiles);
            }
        }

        private List<string> typeList = new List<string>();
        public List<string> TypeList
        {
            get { return typeList; }
            set
            {
                typeList = value;
                RaisePropertyChanged(() => TypeList);
            }
        }

        private string type;
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                RaisePropertyChanged(() => Type);

                if (type == null) return;
                PageView pv = new PageView();
                files.Clear();
                if (type == "最多点击")
                {
                    files = AppFiles.OrderByDescending(x => x.HitNumber).Take(24).ToList();
                }
                else
                {
                    files = AppFiles.Where(x => x.AppType == type).OrderByDescending(x => x.HitNumber).ToList();
                }
                pv.DataContext = new PageViewVM(type, files);
  
                messenger.Send<object>(pv);
            }
        }

        private models.SettingObject appSetting = new models.SettingObject();
        public models.SettingObject AppSetting
        {
            get { return appSetting; }
            set
            {
                appSetting = value;
                RaisePropertyChanged(() => AppSetting);
            }
        }

        public MainWindowVM(Messenger messenger)
        {
            this.messenger = messenger;
        }
    }
}
