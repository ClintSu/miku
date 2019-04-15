using MiKu.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MiKu
{
    public class SoftLinkVM:ViewModelBase
    {

        private List<LinkObject> linkObjects = new List<LinkObject>();
        public List<LinkObject> LinkObjects {
            get { return linkObjects; }
            set { linkObjects = value;
                RaisePropertyChanged(() => LinkObjects);
            }
        }


        public RelayCommand<object> Open { get; }

        public SoftLinkVM()
        {
            this.Open = new RelayCommand<object>((param) => HyperlinkClick(param));

            LinkObjects=models.JsonCenter.ReadLinkJson();
         
        }
        void HyperlinkClick(object obj)
        {
            LinkObject link = obj as LinkObject;
            // 激活的是当前默认的浏览器
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.LinkUrl));
        }
    }


    
}
