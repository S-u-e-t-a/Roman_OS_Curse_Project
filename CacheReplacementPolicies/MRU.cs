using System.Collections.Generic;
using System.Linq;

namespace CacheReplacementPolicies
{
    internal class MRU<T> : CacheReplacementPolicy<T>
    {
        private List<int> age;


        public MRU(int countOfPages) : base(countOfPages)
        {
            age = new List<int>();
            for (var i = 0; i < CountOfPages; i++)
            {
                age.Add(0);
            }
        }


        public override void AddPage(T page)
        {
            if (Pages.Contains(page)) // если страница уже есть
            {
                age = age.Select(x => x + 1).ToList();
                age[Pages.IndexOf(page)] = 0;
            }
            else // если страницы нет
            {
                Interuptions += 1;
                if (Pages.Count < CountOfPages)
                {
                    Pages.Add(page);
                    age = age.Select(x => x + 1).ToList();
                    age[Pages.IndexOf(page)] = 0;
                }
                else
                {
                    var indexYungest = age.IndexOf(age.Min());
                    age = age.Select(x => x + 1).ToList();
                    Pages[indexYungest] = page;
                    age[indexYungest] = 0;
                }
            }
        }
    }
}