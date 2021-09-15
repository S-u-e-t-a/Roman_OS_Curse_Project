using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

using OxyPlot;
using OxyPlot.Series;

namespace OS_Curse_Project
{
    internal class MainVM : INotifyPropertyChanged

    {
        private ObservableCollection<List<char>> answerList;

        private CacheReplacementPolicy<char> choosenPolicy;

        private string inputedPages;

        private List<char> pages;

        private PlotModel plotModel;

        private RelayCommand startCommand;

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

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
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
                           AnswerList = new ObservableCollection<List<char>>();
                           ChoosenPolicy = new RR<char>(5);
                           pages = InputedPages.ToCharArray().ToList();
                           foreach (var page in pages)
                           {
                               ChoosenPolicy.AddPage(page);
                               writeInAnswerPage(page);
                           }

                           foreach (var VARIABLE in AnswerList)
                           {
                               foreach (var VAR in VARIABLE)
                               {
                                   Trace.Write($"{VAR} ");
                               }

                               Trace.WriteLine("");
                           }

                           var rnd = new Random();
                           PlotModel = new PlotModel();
                           /*PlotModel.Axes.Add(new LinearAxis
                           {
                               Position = AxisPosition.Left,
                               Minimum = -0.05,
                               Maximum = 1.05,
                               MajorStep = 0.2,
                               MinorStep = 0.05,
                               TickStyle = TickStyle.Inside
                           });
                           PlotModel.Axes.Add(new LinearAxis
                           {
                               Position = AxisPosition.Bottom,
                               Minimum = -5.25,
                               Maximum = 5.25,
                               MajorStep = 1,
                               MinorStep = 0.25,
                               TickStyle = TickStyle.Inside
                           });*/

                           for (var i = 0; i < 3; i++)
                           {
                               var serie = new LineSeries();
                               for (var j = 0; j < 100; j++)
                               {
                                   serie.Points.Add(new DataPoint(rnd.Next(0, 50), rnd.Next(0, 50)));
                               }

                               PlotModel.Series.Add(serie);
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