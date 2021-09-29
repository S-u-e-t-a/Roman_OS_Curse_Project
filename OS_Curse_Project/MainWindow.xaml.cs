using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace OS_Curse_Project
{
    /// <summary>
    ///     Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM(); // очень сильно нарушили mvvm
        }


        public BitmapSource GetPngGraphImage(FrameworkElement visual)
        {
            var encoder = new PngBitmapEncoder();

            return new BitmapImage();
        }


        public byte[] EncodeVisual(int dpi)
        {
            var visual = Chart123;
            var bitmap = new RenderTargetBitmap(((int) visual.ActualWidth * dpi) / 96, (((int) visual.ActualHeight + 50) * dpi) / 96, dpi, dpi
                , PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);
            //using (var stream = File.Create("fileName.png"))
            //{
            //    encoder.Save(stream);
            //}

            //return new byte[5];
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                var bit = stream.ToArray();
                stream.Close();

                return bit;
            }
        }


        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            FileSystem.test(EncodeVisual(300));
        }
    }
}