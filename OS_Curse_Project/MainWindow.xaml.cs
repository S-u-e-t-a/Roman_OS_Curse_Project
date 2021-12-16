using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

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


        /// <summary>
        ///     Функция для получения изображения графика
        /// </summary>
        /// <param name="dpi">Плонтность пикселей</param>
        /// <returns>Массив пикселей с изображением графика</returns>
        public byte[] EncodeVisual(int dpi)
        {
            var visual = Chart123;
            var bitmap = new RenderTargetBitmap(((int) visual.ActualWidth * dpi) / 96, (((int) visual.ActualHeight + 50) * dpi) / 96, dpi, dpi
                , PixelFormats.Pbgra32);
            bitmap.Render(visual);
            var frame = BitmapFrame.Create(bitmap);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(frame);
            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                var bit = stream.ToArray();
                stream.Close();

                return bit;
            }
        }


        /// <summary>
        ///     Экспорт данных об анализе в PDF файл
        /// </summary>
        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog();
            dlg.DefaultExt = ".pdf";
            dlg.FileName = "АНАЛИЗ_" + DateTime.Now.ToString().Replace(':', '_');
            var res = dlg.ShowDialog();
            if (res == true)
            {
                if ((DataContext as MainVM).results != null)
                {
                    FileSystem.exportPdf(dlg.FileName, EncodeVisual(300), (DataContext as MainVM).results);
                }
                else
                {
                    MessageBox.Show("Нет данных для сохранения", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}