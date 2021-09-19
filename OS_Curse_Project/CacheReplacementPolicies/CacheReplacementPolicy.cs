using System.Collections.Generic;

namespace OS_Curse_Project
{
    public abstract class CacheReplacementPolicy // так надо
    {
        //надо чтобы получить от этого класса детей, потому что от класса с дженериком е получить нормально, ну или я не нашел как (я искал)
    }

    public abstract class CacheReplacementPolicy<T> : CacheReplacementPolicy
    {
        /// <summary>
        ///     Количество страниц памяти в кэше
        /// </summary>
        protected int CountOfPages;

        /// <summary>
        ///     Количество прерываний
        /// </summary>
        public int Interuptions;


        public CacheReplacementPolicy(int countOfPages)
        {
            CountOfPages = countOfPages;
            Pages = new List<T>();
            Interuptions = 0;
        }


        /// <summary>
        ///     Текущие страницы в кэше
        /// </summary>
        public List<T> Pages { get; }


        /// <summary>
        ///     Добавляет страницу в кэш в соответстии с алгоритмом
        /// </summary>
        /// <param name="page">Страница для добавления</param>
        public abstract void AddPage(T page); //todo Переименовать более удачно
    }
}