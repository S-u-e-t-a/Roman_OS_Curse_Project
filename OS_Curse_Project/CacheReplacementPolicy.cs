using System.Collections.Generic;

namespace OS_Curse_Project
{
    internal abstract class CacheReplacementPolicy
    {
        protected int CountOfPages;


        public CacheReplacementPolicy(int countOfPages = 5)
        {
            CountOfPages = countOfPages;
            Pages = new List<int>();
        }


        public List<int> Pages { get; }


        public abstract void AddPage(int page); //todo Переименовать более удачно
    }
}