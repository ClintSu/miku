using MiKu.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiKu.View
{
    public class SoftListVM : ViewModelBase
    {
        private List<AppFile> appFiles = new List<AppFile>();
        public List<AppFile> AppFiles
        {
            get { return appFiles; }
            set
            {
                appFiles = value;
                RaisePropertyChanged(() => AppFiles);
            }
        }

        public SoftListVM(List<AppFile> appFiles)
        {
            this.AppFiles= new List<AppFile>();
            foreach (var app in appFiles.OrderBy(x=>x.AppName))
            {
                AppFiles.Add(app);
            }
        }
    }
}
