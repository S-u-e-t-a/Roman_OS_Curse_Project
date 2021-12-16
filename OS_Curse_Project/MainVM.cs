using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

using CacheReplacementPolicies;

using Gu.Wpf.DataGrid2D;

using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

using Microsoft.Win32;

namespace OS_Curse_Project
{
    internal class MainVM : INotifyPropertyChanged
    {
        #region Constructors

        /// <summary>
        ///     Стандартный конструктор ViewModel
        /// </summary>
        public MainVM()
        {
            MinPage = 2;
            MaxPage = 10;

            var ourtype = typeof(CacheReplacementPolicy);
            var list = Assembly.GetAssembly(ourtype).GetTypes()
                .Where(type => type.IsSubclassOf(ourtype) && !type.IsAbstract);

            foreach (var itm in list)
            {
                Console.WriteLine(itm);
            }

            foreach (var alg in list)
            {
                // generic List with no parameters

                // To create a List<string>
                Type[] tArgs = {typeof(char)};
                var target = alg.MakeGenericType(tArgs);

                // Create an instance - Activator.CreateInstance will call the default constructor.
                // This is equivalent to calling new List<string>().
                var result = (CacheReplacementPolicy<char>) Activator.CreateInstance(target, 5);
                Trace.WriteLine(result);
            }


            SeriesCollection = new SeriesCollection();
        }

        #endregion


        #region Commands

        private RelayCommand showHelp;

        /// <summary>
        ///     команда показывающая справку
        /// </summary>
        public RelayCommand ShowHelp
        {
            get
            {
                return showHelp ?? (showHelp = new RelayCommand(o =>
                {
                    var help = new HelpWIndow();
                    help.ShowDialog();
                }));
            }
        }

        private RelayCommand saveInitialData;

        /// <summary>
        ///     команда сохраняющая начальные данные
        /// </summary>
        public RelayCommand SaveInitialData
        {
            get
            {
                return saveInitialData ?? (saveInitialData = new RelayCommand(o =>
                {
                    var dlg = new SaveFileDialog();
                    dlg.DefaultExt = ".txt";
                    dlg.FileName = "Начальные данные_" + DateTime.Now.ToString().Replace(':', '_');
                    var res = dlg.ShowDialog();
                    if (res == true)
                    {
                        var data = InputedPages + "\n" + MinPage + "\n" + MaxPage;
                        FileSystem.SaveToFile(dlg.FileName, data);
                    }
                }));
            }
        }

        private RelayCommand readInitialData;

        /// <summary>
        ///     Команда читающая данные из файла
        /// </summary>
        public RelayCommand ReadInitialData
        {
            get
            {
                return readInitialData ?? (readInitialData = new RelayCommand(o =>
                {
                    var dlg = new OpenFileDialog();
                    dlg.DefaultExt = ".txt";
                    var res = dlg.ShowDialog();
                    if (res == true)
                    {
                        var data = FileSystem.ReadFromFile(dlg.FileName);
                        if (data.Count != 3)
                        {
                            MessageBox.Show(
                                "Ошибка, в файле неправильное количество данных," +
                                " убедитесь что внутри файла находится строка с симоволами," +
                                " строка с минимальным количеством страниц и" +
                                " строка с максимальным количеством страниц",
                                "Ошибка чтения", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            InputedPages = data[0];
                            MinPage = int.Parse(data[1]);
                            MaxPage = int.Parse(data[2]);
                        }
                    }
                }));
            }
        }

        /// <summary>
        ///     Команда выполняет анализ алгоритмов
        /// </summary>
        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ??
                       (startCommand = new RelayCommand(o =>
                       {
                           if (!isDataCorrect())
                           {
                               MessageBox.Show("Введены некорректные данные, попробуйте снова", "Ошибка", MessageBoxButton.OK);
                           }
                           else
                           {
                               results = new Dictionary<string, Dictionary<int, List<List<char>>>>();
                               Tabs = new ObservableCollection<TabItem>(); // окно типа мру фифо и тд
                               SeriesCollection = new SeriesCollection();
                               var basetype = typeof(CacheReplacementPolicy);
                               var childs = Assembly.GetAssembly(basetype).GetTypes().Where(type => type.IsSubclassOf(basetype) && !type.IsAbstract);
                               pages = InputedPages.ToCharArray().ToList();
                               foreach (var alg in childs)
                               {
                                   var name = alg.Name.Split('`')[0];
                                   results[name] = new Dictionary<int, List<List<char>>>();
                                   var tabsItems = new ObservableCollection<TabItem>(); // окна типа 1,2,3,4,5 (количество страниц в кэше)
                                   var serie = new LineSeries();
                                   serie.Title = name;
                                   serie.Values = new ChartValues<ObservablePoint>();
                                   Type[] tArgs = {typeof(char)};
                                   var target = alg.MakeGenericType(tArgs);
                                   for (var i = MinPage; i < MaxPage + 1; i++)
                                   {
                                       var cachePages = new List<List<char>>();
                                       var headers = new List<string> {"П", "Добавлена"};
                                       var policy = (CacheReplacementPolicy<char>) Activator.CreateInstance(target, i);
                                       foreach (var page in pages)
                                       {
                                           var interuptions = policy.Interuptions;
                                           policy.AddPage(page);
                                           cachePages.Add(new List<char>());
                                           char interupt;
                                           // тут стоило сделать событие при изменении количества прерываний, но это надо все перелопачивать, а мне лень
                                           if (interuptions != policy.Interuptions)
                                           {
                                               interupt = 'П';
                                           }
                                           else
                                           {
                                               interupt = ' ';
                                           }

                                           cachePages[cachePages.Count - 1].Add(interupt);
                                           cachePages[cachePages.Count - 1].Add(page);
                                           foreach (var p in policy.Pages)
                                           {
                                               cachePages[cachePages.Count - 1].Add(p);
                                           }
                                       }

                                       results[name][i] = cachePages.ToList();
                                       for (var j = 1; j < cachePages.Last().Count - 1; j++)
                                       {
                                           headers.Add($"C{j}");
                                       }

                                       serie.Values.Add(new ObservablePoint(i, policy.Interuptions));
                                       serie.LineSmoothness = 0;
                                       var grid = new DataGrid(); // НАРУШАЕМ ПРИНЦИПЫ MVVM?????????????? ДА!
                                       grid.SetRowsSource(cachePages);
                                       grid.SetColumnHeadersSource(headers);
                                       tabsItems.Add(new TabItem {Header = i, Content = grid});
                                   }

                                   SeriesCollection.Add(serie);
                                   Tabs.Add(new TabItem
                                   {
                                       Header = name,
                                       Content = new TabControl {ItemsSource = tabsItems}
                                   });
                               }

                               OnPropertyChanged();
                           }
                       }));
            }
        }

        #endregion


        #region Fields

        public Dictionary<string, Dictionary<int, List<List<char>>>> results;

        private SeriesCollection _seriesCollection;

        private ObservableCollection<TabItem> _tabs;
        private ObservableCollection<List<List<char>>> answerList;

        private CacheReplacementPolicy<char> choosenPolicy;

        private string inputedPages;
        private int maxPage;

        private int minPage;

        private List<char> pages;

        private RelayCommand startCommand;

        #endregion


        #region Properties

        public int MinPage
        {
            get { return minPage; }
            set
            {
                minPage = value;
                OnPropertyChanged();

                if (MinPage > MaxPage)
                {
                    MaxPage = MinPage;
                }
            }
        }

        public int MaxPage
        {
            get { return maxPage; }
            set
            {
                maxPage = value;
                OnPropertyChanged();
                if (MaxPage < MinPage)
                {
                    MinPage = MaxPage;
                }
            }
        }


        public ObservableCollection<List<List<char>>> AnswerList
        {
            get { return answerList; }
            set
            {
                answerList = value;
                OnPropertyChanged();
            }
        }

        public string InputedPages
        {
            get { return inputedPages; }
            set
            {
                inputedPages = value;
                OnPropertyChanged();
            }
        }

        public CacheReplacementPolicy<char> ChoosenPolicy
        {
            get { return choosenPolicy; }
            set
            {
                choosenPolicy = value;
                OnPropertyChanged();
            }
        }

        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TabItem> Tabs
        {
            get { return _tabs; }
            set
            {
                _tabs = value;
                OnPropertyChanged();
            }
        }

        #endregion


        #region Functions

        private bool isDataCorrect()
        {
            if (isBordersCorrect() && isPagesCorrect())
            {
                return true;
            }

            return false;
        }


        private bool isBordersCorrect()
        {
            if ((MinPage < MaxPage) && (MinPage > 0))
            {
                return true;
            }

            return false;
        }


        private bool isPagesCorrect()
        {
            if (InputedPages != null)
            {
                return true;
            }

            return false;
        }

        #endregion


        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }

        #endregion
    }
}