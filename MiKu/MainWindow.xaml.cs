using MiKu.View;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiKu
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Messenger messenger = new Messenger();
        private MainWindowVM vm;

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;

            this.Closed += MainWindow_Closed;
            messenger.Register<object>((p) =>
            {
                PageView pv = (PageView) p;
                this.pTransitionControl.ShowPage(pv);
            });
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            models.JsonCenter.WriteAppJson(vm.AppFiles);
            models.JsonCenter.WriteSettingJson(vm.AppSetting);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            vm = new MainWindowVM(messenger);
            this.DataContext = vm;

            vm.AppFiles = models.JsonCenter.ReadAppJson();

            vm.TypeList.Add("最多点击");
            foreach (var val in vm.AppFiles.GroupBy(x => x.AppType).Select(x => x.Key))
            {
                vm.TypeList.Add(val);
            }

            vm.AppSetting = models.JsonCenter.ReadSettingJson();

            InitBorderBackground();

            Color color = (Color)ColorConverter.ConvertFromString(vm.AppSetting.GridColor);
            InitMyBrush(color);
            
            PageTransitions.PageTransitionType type = (PageTransitions.PageTransitionType)Enum.Parse(typeof(PageTransitions.PageTransitionType), vm.AppSetting.PageTransitionType);
            InitPageType(type);

            vm.Type = vm.TypeList.First();
        }

        private void InitBorderBackground()
        {
            try
            {
                BitmapImage bitmapImage;
                var framepng = Environment.CurrentDirectory + "\\images\\backframe.png";
                using (var stream = new System.IO.FileStream(framepng, System.IO.FileMode.Open))
                {
                    bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapImage.StreamSource = stream;
                    bitmapImage.EndInit();
                    bitmapImage.Freeze(); // just in case you want to load the image in another thread
                }
                border.Background = new ImageBrush() { ImageSource = bitmapImage };
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        private void InitMyBrush(Color color)
        {
            try
            {
                LinearGradientBrush brush = new LinearGradientBrush();
                brush.StartPoint = new Point(0.5, 0);
                brush.EndPoint = new Point(0.5, 1);
                brush.GradientStops.Add(new GradientStop() { Color = (Color)ColorConverter.ConvertFromString("#CCFFFFFF") });
                brush.GradientStops.Add(new GradientStop() { Color = color, Offset = 1 });
                this.grid.Background = brush;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InitPageType(PageTransitions.PageTransitionType type)
        {
            this.pTransitionControl.TransitionType = type;      
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //拖动
            this.DragMove();
        }

        private void mniButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void maxButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
              this.Close();
        }

        //private void btnDefault_Checked(object sender, RoutedEventArgs e)
        //{
        //    PageDefault page = new PageDefault();
        //    page.DataContext = new PageDefaultVM(vm.AppFiles);
        //    this.pTransitionControl.ShowPage(page);
        //    headerNavi.SelectedIndex = -1;
        //}

        private void linkButton_Click(object sender, RoutedEventArgs e)
        {
            SoftLink softLink = new SoftLink();
            softLink.ShowDialog();
        }

        /// <summary>
        /// 设置背景图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            gridShadow.Visibility = Visibility.Visible;

            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "图片|*.png;*.jpg"
            };
            if (ofd.ShowDialog() == true)
            {
                System.IO.File.Delete(Environment.CurrentDirectory + "\\images\\backframe.png");
                System.IO.File.Copy(ofd.FileName, Environment.CurrentDirectory + "\\images\\backframe.png");

                InitBorderBackground();
            }

            gridShadow.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
            gridShadow.Visibility = Visibility.Visible;

            ColorDialog colorDialog = new ColorDialog();
            colorDialog.SelectedColor = (Color)ColorConverter.ConvertFromString(vm.AppSetting.GridColor);
            if ((bool)colorDialog.ShowDialog())
            {
                InitMyBrush(colorDialog.SelectedColor);
                vm.AppSetting.GridColor =
                    $"#{colorDialog.SelectedColor.A:X2}{colorDialog.SelectedColor.R:X2}{colorDialog.SelectedColor.G:X2}{colorDialog.SelectedColor.B:X2}";
            }

            gridShadow.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 页面切换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnType_Click(object sender, RoutedEventArgs e)
        {
            PageTransitions.PageTransitionType type = (PageTransitions.PageTransitionType)Enum.Parse(typeof(PageTransitions.PageTransitionType), vm.AppSetting.PageTransitionType);
            PageTransitions.PageTransitionType next = EnumNext(type);
            InitPageType(next);
            vm.AppSetting.PageTransitionType = Enum.GetName(typeof(PageTransitions.PageTransitionType), next);
        }

        private PageTransitions.PageTransitionType EnumNext(PageTransitions.PageTransitionType type)
        {
            int index = (int)type;
            if (index != 8) { index++; }
            else index = 0;
            return (PageTransitions.PageTransitionType)Enum.ToObject(typeof(PageTransitions.PageTransitionType), index);
        }

        private void BtnFontColor_OnClick(object sender, RoutedEventArgs e)
        {
            vm.AppSetting.FontColor = vm.AppSetting.FontColor == "#FFFFFF" ? "#000000" : "#FFFFFF";
        }

        private void BtnAll_OnClick(object sender, RoutedEventArgs e)
        {
            SoftList list = new SoftList();
            list.DataContext = new SoftListVM(vm.AppFiles);
            list.ShowDialog();
        }
    }
}
