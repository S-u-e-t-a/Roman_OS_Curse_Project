﻿using System;
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

namespace OS_Curse_Project
{
    internal class MainVM : INotifyPropertyChanged
    {
        private SeriesCollection _seriesCollection;

        private ObservableCollection<TabItem> _tabs;
        private ObservableCollection<List<char>> answerList;

        private CacheReplacementPolicy<char> choosenPolicy;

        private string inputedPages;
        private int maxPage;

        private int minPage;

        private List<char> pages;

        private RelayCommand startCommand;


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
                           if (!isDataCorrect())
                           {
                               MessageBox.Show("Введены некорректные данные, попробуйте снова", "Ошибка", MessageBoxButton.OK);
                           }
                           else
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
                                   for (var i = MinPage; i < MaxPage + 1; i++)
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
                           }
                       }));
            }
        }


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