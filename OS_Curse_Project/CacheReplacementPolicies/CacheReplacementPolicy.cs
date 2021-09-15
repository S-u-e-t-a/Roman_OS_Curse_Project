using System.Collections.Generic;

namespace OS_Curse_Project
{
    public abstract class CacheReplacementPolicy<T>
    {
        protected int CountOfPages;


        public CacheReplacementPolicy(int countOfPages)
        {
            CountOfPages = countOfPages;
            Pages = new List<T>();
        }


        public List<T> Pages { get; }


        public abstract void AddPage(T page); //todo Переименовать более удачно
    }
}