using System.Windows;

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
            DataContext = new MainVM();
            /*// тестирование всяких штук
            //todo Удалить
            var a = new MRU<int>(4);
            var b = new List<int>
                {3, 7, 4, 2, 4, 2, 6, 1, 0, 4, 2, 8, 9, 5, 7, 4, 2, 6, 9, 2}; // тестирование всяких штук
            foreach (var UPPER in b)
            {
                a.AddPage(UPPER);
                Trace.Write($"{UPPER} - ");
                printList(a.Pages);
            }*/
        }


        /*//todo Удалить
        private void printList(List<int> l)
        {
            foreach (var UPPER in l)
            {
                Trace.Write($"{UPPER} ");
            }

            Trace.WriteLine(null);
        }*/
    }
}