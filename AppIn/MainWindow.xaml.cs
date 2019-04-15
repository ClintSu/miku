using MiKu.models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AppIn
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<AppFile> _appFiles;
        public List<IconModel> IconModelList = new List<IconModel>();
        private string fileName;
        private string name;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _appFiles = MiKu.models.JsonCenter.ReadAppJson();
            //this.typeList.ItemsSource = _appFiles;
        }

        private void border_DragEnter(object sender, DragEventArgs e)
        {
            this.image.Opacity = 0.1;
            this.txt.Visibility = Visibility.Collapsed;
            Mouse.SetCursor(Cursors.Cross);
        }

        private void border_Drop(object sender, DragEventArgs e)
        {
            fileName = ((System.Array)e.Data.GetData(DataFormats.FileDrop))?.GetValue(0).ToString();
            if (File.Exists(fileName))
            {
                
                var extension = Path.GetExtension(fileName);
                if (".exe" != extension)
                {
                    MessageBox.Show("非应用程序文件，请拖入要添加的应用程序文件。");
                    return;
                }
                name = System.IO.Path.GetFileNameWithoutExtension(fileName);

                this.grid1.Visibility = Visibility.Collapsed;
                this.grid2.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("拖入非文件。");
            }


        }

        private void border_DragLeave(object sender, DragEventArgs e)
        {
            this.image.Opacity = 0.5;
            this.txt.Visibility = Visibility.Visible;
            Mouse.SetCursor(Cursors.None);
        }

        #region 生成图标

        //details: https://msdn.microsoft.com/en-us/library/windows/desktop/ms648075(v=vs.85).aspx
        //Creates an array of handles to icons that are extracted from a specified file.
        //This function extracts from executable (.exe), DLL (.dll), icon (.ico), cursor (.cur), animated cursor (.ani), and bitmap (.bmp) files. 
        //Extractions from Windows 3.x 16-bit executables (.exe or .dll) are also supported.
        [DllImport("User32.dll")]
        public static extern int PrivateExtractIcons(
            string lpszFile, //file name
            int nIconIndex,  //The zero-based index of the first icon to extract.
            int cxIcon,      //The horizontal icon size wanted.
            int cyIcon,      //The vertical icon size wanted.
            IntPtr[] phicon, //(out) A pointer to the returned array of icon handles.
            int[] piconid,   //(out) A pointer to a returned resource identifier.
            int nIcons,      //The number of icons to extract from the file. Only valid when *.exe and *.dll
            int flags        //Specifies flags that control this function.
        );

        //details:https://msdn.microsoft.com/en-us/library/windows/desktop/ms648063(v=vs.85).aspx
        //Destroys an icon and frees any memory the icon occupied.
        [DllImport("User32.dll")]
        public static extern bool DestroyIcon(
            IntPtr hIcon //A handle to the icon to be destroyed. The icon must not be in use.
        );

        public void Extract()
        {
            //指定存放图标的文件夹
            string folderToSave = System.IO.Path.GetTempPath() + "miku\\" + name + "\\";
            if (!Directory.Exists(folderToSave)) Directory.CreateDirectory(folderToSave);

            //选中文件中的图标总数
            var iconTotalCount = PrivateExtractIcons(fileName, 0, 0, 0, null, null, 0, 0);

            //用于接收获取到的图标指针
            IntPtr[] hIcons = new IntPtr[iconTotalCount];
            //对应的图标id
            int[] ids = new int[iconTotalCount];
            //成功获取到的图标个数
            var successCount = PrivateExtractIcons(fileName, 0, 128, 128, hIcons, ids, iconTotalCount, 0);

            //遍历并保存图标
            for (var i = 0; i < successCount; i++)
            {
                //指针为空，跳过
                if (hIcons[i] == IntPtr.Zero) continue;

                using (var ico = System.Drawing.Icon.FromHandle(hIcons[i]))
                {
                    using (var myIcon = ico.ToBitmap())
                    {
                        myIcon.Save(folderToSave + name + ids[i].ToString("000") + ".png", System.Drawing.Imaging.ImageFormat.Png);
                        IconModelList.Add(new IconModel() { BitmapImage = BitmapToBitmapImage(myIcon), ImageUri = folderToSave + name + ids[i].ToString("000") + ".png", IsSelected = false });
                    }
                }
                //内存回收
                DestroyIcon(hIcons[i]);

                if (IconModelList.Count == 1)
                {
                    IconModelList.ToList().ForEach(x => x.IsSelected = true);
                }
                this.listBox.ItemsSource = IconModelList;
            }

        }
        #endregion

        private void only_Checked(object sender, RoutedEventArgs e)
        {
            file.IsChecked = !only.IsChecked;
            this.panelType.Visibility = Visibility.Visible;
            this.listBox.Visibility = Visibility.Visible;
            Extract();
            btnChecked.IsEnabled = true;
        }

        private void file_Checked(object sender, RoutedEventArgs e)
        {
            only.IsChecked = !file.IsChecked;
            this.panelType.Visibility = Visibility.Visible;
            this.listBox.Visibility = Visibility.Visible;
            Extract();
            btnChecked.IsEnabled = true;
        }

        private void btnChecked_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtName.Text) || string.IsNullOrEmpty(this.txtType.Text))
                {
                    MessageBox.Show("程序名称和类型均不可为空！");
                    return;
                }

                var first = IconModelList.First(x => x.IsSelected == true);
                var thumName = Environment.CurrentDirectory + "\\images\\button\\button_" + name + ".png";
                if (!File.Exists(thumName))
                {
                    System.IO.File.Move(first.ImageUri, thumName);
                }
                if (this.only.IsChecked == true)
                { //单文件
                    var destPath = Environment.CurrentDirectory + "\\tools\\";
                    if (!Directory.Exists(destPath))
                    {
                        try
                        {
                            Directory.CreateDirectory(destPath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("创建目标目录失败：" + ex.Message);
                        }
                    }

                    string destFile = System.IO.Path.Combine(new string[] { destPath, System.IO.Path.GetFileName(fileName) });
                    File.Copy(fileName, destFile, true);//覆盖模式

                    if (_appFiles.All(x => x.ExeName != name))
                    {
                        _appFiles.Add(new AppFile()
                        { ExeName = name, ExeFolderName = "", AppName = this.txtName.Text,AppType = this.txtType.Text});
                    }
                }
                else
                {
                    //便携文件夹
                    var path = System.IO.Path.GetDirectoryName(fileName);
                    var destPath = Environment.CurrentDirectory + "\\tools\\" + name + "\\";
                    CopyFolder(path, destPath);

                   

                    if (_appFiles.All(x => x.ExeName != name))
                    {
                        _appFiles.Add(new AppFile()
                        { ExeName = name, ExeFolderName = name, AppName = this.txtName.Text, AppType = this.txtType.Text });
                    }
                }

                MiKu.models.JsonCenter.WriteAppJson(_appFiles);

                btnClosed_Click(null, null);
                MessageBox.Show("添加成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("添加失败："+ ex.Message);
            }
        }

        private void btnClosed_Click(object sender, RoutedEventArgs e)
        {
           
            this.grid1.Visibility = Visibility.Visible;
            this.grid2.Visibility = Visibility.Collapsed;

            IconModelList.Clear();
            panelType.Visibility = Visibility.Collapsed;
            listBox.Visibility = Visibility.Collapsed;
            btnChecked.IsEnabled = false;
            only.IsChecked = false;
            file.IsChecked = false;
        }

        /// <summary>
        /// BitMap2BitMapImage
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        /// <summary> 
        /// 复制文件夹中的所有文件夹与文件到另一个文件夹
        /// </summary>
        /// <param name="sourcePath">源文件夹</param>
        /// <param name="destPath">目标文件夹</param>
        private void CopyFolder(string sourcePath, string destPath)
        {
            if (Directory.Exists(sourcePath))
            {
                if (!Directory.Exists(destPath))
                {
                    //目标目录不存在则创建
                    try
                    {
                        Directory.CreateDirectory(destPath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("创建目标目录失败：" + ex.Message);
                    }
                }
                //获得源文件下所有文件
                List<string> files = new List<string>(Directory.GetFiles(sourcePath));
                files.ForEach(c =>
                {
                    string destFile = System.IO.Path.Combine(new string[] { destPath, System.IO.Path.GetFileName(c) });
                    File.Copy(c, destFile, true);//覆盖模式
                });
                //获得源文件下所有目录文件
                List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
                folders.ForEach(c =>
                {
                    string destDir = System.IO.Path.Combine(new string[] { destPath, System.IO.Path.GetFileName(c) });
                    //采用递归的方法实现
                    CopyFolder(c, destDir);
                });
            }
            else
            {
                throw new DirectoryNotFoundException("源目录不存在！");
            }
        }
    }

    public class IconModel
    {
        public BitmapImage BitmapImage { get; set; }

        public string ImageUri { get; set; }

        public bool IsSelected { get; set; }
    }
}
