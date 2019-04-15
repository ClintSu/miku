using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiKu.models
{
    ///// <summary>
    ///// 应用分类
    ///// </summary>
    //public enum AppType
    //{
    //    SystemTools= 1, //系统应用
    //    PracticalTools = 2, //实用软件
    //    ProfessionTools = 3, //专业软件
    //    OtherTools = 4, //其他软件
    //}

    public class AppFile:ViewModelBase
    {
        private string appName;
        private string exeName;
        private string exeFolderName = "";
        private int hitNumber;

        /// <summary>
        /// 应用显示名称
        /// </summary>
        public string AppName
        {
            get { return appName; }
            set
            {
                appName = value;
                RaisePropertyChanged(() => AppName);
            }
        }
        /// <summary>
        /// 应用程序exe名称
        /// </summary>
        public string ExeName
        {
            get { return exeName; }
            set
            {
                exeName = value;
                RaisePropertyChanged(() => ExeName);
            }
        }
        /// <summary>
        /// 非单文件所在目录
        /// </summary>
        public string ExeFolderName
        {
            get { return exeFolderName; }
            set
            {
                exeFolderName = value;
                RaisePropertyChanged(() => ExeFolderName);
            }
        }
        /// <summary>
        /// 被点击次数
        /// </summary>
        public int HitNumber
        {
            get { return hitNumber; }
            set
            {
                hitNumber = value;
                RaisePropertyChanged(() => HitNumber);
            }
        }
        public string AppType { get; set; }
    }

   
    public class AppClassify
    {
        public int Index { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<AppFile> AppFiles { get; set; }
        public AppClassify()
        {
            AppFiles = new List<AppFile>();
        }
    }
}
