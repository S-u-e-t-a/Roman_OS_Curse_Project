using System.Collections.Generic;

namespace CacheReplacementPolicies
{
    internal class Clock<T> : CacheReplacementPolicy<T>
    {
        private readonly List<int> rbits;
        private int _clockArrow;


        public Clock(int countOfPages) : base(countOfPages)
        {
            rbits = new List<int>();
            for (var i = 0; i < CountOfPages; i++)
            {
                rbits.Add(0);
            }
        }


        private int ClockArrow
        {
            get { return _clockArrow; }
            set
            {
                _clockArrow = value;
                if (_clockArrow == CountOfPages)
                {
                    _clockArrow = 0;
                }
            }
        }


        public override void AddPage(T page)
        {
            if (Pages.Contains(page))
            {
                rbits[Pages.IndexOf(page)] = 1;
                ClockArrow += 1;
            }
            else
            {
                Interuptions += 1;
                if (Pages.Count < CountOfPages)
                {
                    Pages.Add(page);
                    rbits[Pages.IndexOf(page)] = 1;
                    ClockArrow += 1;
                }
                else
                {
                    if (rbits[ClockArrow] == 0)
                    {
                        Pages[ClockArrow] = page;
                        rbits[ClockArrow] = 1;
                        ClockArrow += 1;
                    }
                    else
                    {
                        rbits[ClockArrow] = 0;
                        ClockArrow += 1;
                        AddPage(page);
                    }
                }
            }
        }
    }
}