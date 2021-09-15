using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;

namespace OS_Curse_Project
{
    internal class MainVM : INotifyPropertyChanged

    {
        private ObservableCollection<List<char>> answerList;

        private CacheReplacementPolicy<char> choosenPolicy;

        private string inputedPages;

        private List<char> pages;

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


        public RelayCommand StartCommand
        {
            get
            {
                return startCommand ??
                       (startCommand = new RelayCommand(o =>
                       {
                           AnswerList = new ObservableCollection<List<char>>();
                           ChoosenPolicy = new FIFO<char>(5);
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