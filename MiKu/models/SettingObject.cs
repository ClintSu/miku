using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiKu.models
{
    public class SettingObject: ViewModelBase
    {
        public string GridColor { get; set; }
        public string PageTransitionType { get; set; }

        private string fontColor;
        public string FontColor
        {
            get { return fontColor; }
            set
            {
                fontColor = value;
                RaisePropertyChanged(() => FontColor);
            }
        }
    }
}
