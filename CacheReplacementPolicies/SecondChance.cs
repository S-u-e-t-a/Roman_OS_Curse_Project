using System.Collections.Generic;

namespace CacheReplacementPolicies
{
    internal class SecondChance<T> : CacheReplacementPolicy<T>
    {
        private readonly List<int> rbits;


        public SecondChance(int countOfPages) : base(countOfPages)
        {
            rbits = new List<int>();
            for (var i = 0; i < CountOfPages; i++)
            {
                rbits.Add(0);
            }
        }


        public override void AddPage(T page)
        {
            if (Pages.Contains(page))
            {
                rbits[Pages.IndexOf(page)] = 1;
            }
            else
            {
                if (Pages.Count < CountOfPages)
                {
                    Pages.Add(page);
                    rbits[Pages.IndexOf(page)] = 1;
                    Interuptions += 1;
                }
                else
                {
                    if (rbits[0] == 0)
                    {
                        Pages.RemoveAt(0);
                        Pages.Add(page);
                        Interuptions += 1;
                    }
                    else
                    {
                        rbits.RemoveAt(0);
                        rbits.Add(0);
                        Pages.Add(Pages[0]);
                        Pages.RemoveAt(0);
                        AddPage(page);
                    }
                }
            }
        }
    }
}