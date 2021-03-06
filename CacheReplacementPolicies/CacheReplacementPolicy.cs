using System.Collections.Generic;

namespace CacheReplacementPolicies
{
    public abstract class CacheReplacementPolicy // так надоЫ
    {
        //надо чтобы получить от этого класса детей, потому что от класса с дженериком е получить нормально, ну или я не нашел как (я искал)
    }

    /// <summary>
    ///     Абстрактный класс для создания алгоритма замещения страниц
    /// </summary>
    /// <typeparam name="T">Тип данных представляющий собой страницу</typeparam>
    public abstract class CacheReplacementPolicy<T> : CacheReplacementPolicy
    {
        /// <summary>
        ///     Количество прерываний
        /// </summary>
        public int Interuptions;

        /// <summary>
        ///     Количество страниц памяти в кэше
        /// </summary>
        protected int CountOfPages;


        /// <summary>
        ///     Текущие страницы в кэше
        /// </summary>
        public List<T> Pages { get; }


        public CacheReplacementPolicy(int countOfPages)
        {
            CountOfPages = countOfPages;
            Pages = new List<T>();
            Interuptions = 0;
        }


        /// <summary>
        ///     Добавляет страницу в кэш в соответстии с алгоритмом
        /// </summary>
        /// <param name="page">Страница для добавления</param>
        public abstract void AddPage(T page); //todo Переименовать более удачно
    }
}