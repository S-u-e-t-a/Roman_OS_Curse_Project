using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace OS_Curse_Project
{
    internal class MainVM : INotifyPropertyChanged
    {
        private SeriesCollection _seriesCollection;
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

        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ??
                       (startCommand = new RelayCommand(o =>
                       {
                           SeriesCollection = new SeriesCollection();
                           var basetype = typeof(CacheReplacementPolicy);
                           var childs = Assembly.GetAssembly(basetype).GetTypes()
                               .Where(type => type.IsSubclassOf(basetype) && !type.IsAbstract);
                           pages = InputedPages.ToCharArray().ToList();
                           foreach (var alg in childs)
                           {
                               var serie = new LineSeries();
                               serie.Title = alg.Name;
                               serie.Values = new ChartValues<ObservablePoint>();
                               // generic List with no parameters

                               // To create a List<string>
                               Type[] tArgs = {typeof(char)};
                               var target = alg.MakeGenericType(tArgs);

                               // Create an instance - Activator.CreateInstance will call the default constructor.
                               // This is equivalent to calling new List<string>().
                               for (var i = 1; i < 10; i++)
                               {
                                   var policy = (CacheReplacementPolicy<char>) Activator.CreateInstance(target, i);
                                   foreach (var page in pages)
                                   {
                                       policy.AddPage(page);
                                   }

                                   serie.Values.Add(new ObservablePoint(i, policy.Interuptions));
                               }

                               SeriesCollection.Add(serie);
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