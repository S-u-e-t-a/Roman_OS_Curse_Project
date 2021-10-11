using System;

namespace CacheReplacementPolicies
{
    public class RR<T> : CacheReplacementPolicy<T>
    {
        private readonly Random rnd;


        public RR(int countOfPages) : base(countOfPages)
        {
            rnd = new Random();
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
                    var index = rnd.Next(0, CountOfPages);
                    Pages.RemoveAt(index);
                    Pages.Insert(index, page);
                }
            }
        }
    }
}