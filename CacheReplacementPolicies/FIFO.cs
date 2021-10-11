namespace CacheReplacementPolicies
{
    public class FIFO<T> : CacheReplacementPolicy<T>
    {
        public FIFO(int countOfPages) : base(countOfPages)
        {
        }


        public override void AddPage(T page)
        {
            if (Pages.Contains(page))
            {
            }
            else
            {
                Interuptions += 1;
                if (Pages.Count < CountOfPages)
                {
                    Pages.Add(page);
                }
                else
                {
                    Pages.RemoveAt(0);
                    Pages.Add(page);
                }
            }
        }
    }
}