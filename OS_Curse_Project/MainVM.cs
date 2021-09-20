using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

using Gu.Wpf.DataGrid2D;

using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace OS_Curse_Project
{
    internal class MainVM : INotifyPropertyChanged
    {
        private SeriesCollection _seriesCollection;

        private ObservableCollection<TabItem> _tabs;
        private ObservableCollection<List<char>> answerList;

        private CacheReplacementPolicy<char> choosenPolicy;

        private string inputedPages;

        private List<char> pages;

        private RelayCommand startCommand;


        public MainVM()
        {
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


        public ObservableCollection<List<char>> AnswerList
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

        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ??
                       (startCommand = new RelayCommand(o =>
                       {
                           Tabs = new ObservableCollection<TabItem>(); // окно типа мру фифо и тд
                           SeriesCollection = new SeriesCollection();
                           var basetype = typeof(CacheReplacementPolicy);
                           var childs = Assembly.GetAssembly(basetype).GetTypes().Where(type => type.IsSubclassOf(basetype) && !type.IsAbstract);
                           pages = InputedPages.ToCharArray().ToList();
                           foreach (var alg in childs)
                           {
                               var tabsItems = new ObservableCollection<TabItem>(); // окна типа 1,2,3,4,5 (количество страниц в кэше)
                               var serie = new LineSeries();
                               serie.Title = alg.Name;
                               serie.Values = new ChartValues<ObservablePoint>();
                               Type[] tArgs = {typeof(char)};
                               var target = alg.MakeGenericType(tArgs);
                               for (var i = 1; i < 10; i++)
                               {
                                   var cachePages = new List<List<char>>();

                                   var policy = (CacheReplacementPolicy<char>) Activator.CreateInstance(target, i);
                                   foreach (var page in pages)
                                   {
                                       policy.AddPage(page);
                                       cachePages.Add(new List<char>());
                                       cachePages[cachePages.Count - 1].Add(page);
                                       foreach (var p in policy.Pages)
                                       {
                                           cachePages[cachePages.Count - 1].Add(p);
                                       }
                                   }

                                   serie.Values.Add(new ObservablePoint(i, policy.Interuptions));
                                   var grid = new DataGrid(); // НАРУШАЕМ ПРИНЦИПЫ MVVM?????????????? ДА!
                                   grid.AutoGenerateColumns = true;
                                   grid.SetRowsSource(cachePages);
                                   tabsItems.Add(new TabItem {Header = i, Content = grid});
                               }

                               SeriesCollection.Add(serie);
                               Tabs.Add(new TabItem
                               {
                                   Header = alg.Name,
                                   Content = new TabControl {ItemsSource = tabsItems}
                               });
                           }

                           OnPropertyChanged();
                       }));
            }
        }


        private void writeInAnswerPage(char page)
        {
            var l = new List<char>();
            l.Add(page);
            foreach (var p in ChoosenPolicy.Pages)
            {
                l.Add(p);
            }

            answerList.Add(l);
        }


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