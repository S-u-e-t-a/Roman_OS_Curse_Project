namespace OS_Curse_Project
{
    internal class FIFO : CacheReplacementPolicy
    {
        public FIFO(int countOfPages) : base(countOfPages)
        {
        }


        public override void AddPage(int page)
        {
            if (!Pages.Contains(page)) // если кэш не содержит данную страницу
            {
                if (Pages.Count < CountOfPages) // если кэш не заполнен
                {
                    Pages.Add(page);
                }
                else
                {
                    Pages.RemoveAt(0);
                    Pages.Add(page);
                    // можно было реализоовать через контейнер стека или очереди (забыл как), но так будет хуже наверное
                }
            }
        }
    }
}