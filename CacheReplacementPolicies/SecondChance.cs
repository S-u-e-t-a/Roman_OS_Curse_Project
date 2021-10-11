//using System.Collections.Generic;

//namespace CacheReplacementPolicies
//{
//    internal class SecondChance<T> : CacheReplacementPolicy<T>
//    {
//        private int _pointer;

//        private readonly List<int> SC;


//        public SecondChance(int countOfPages) : base(countOfPages)
//        {
//            SC = new List<int>();
//            for (var i = 0; i < countOfPages; i++)
//            {
//                SC.Add(0);
//            }
//        }


//        private int pointer
//        {
//            get { return _pointer; }
//            set
//            {
//                if (value == CountOfPages)
//                {
//                    pointer = 0;
//                }
//                else
//                {
//                    pointer = value;
//                }
//            }
//        }


//        public override void AddPage(T page)
//        {
//            if (Pages.Contains(page))
//            {
//                SC[Pages.IndexOf(page)] = 1;
//            }
//            else
//            {
//                Interuptions += 1;
//                if (Pages.Count < CountOfPages)
//                {
//                    Pages.Add(page);
//                    pointer += 1;
//                }
//                else
//                {
//                    if (SC.Contains(0))
//                    {
//                        if (SC[pointer] == 1)
//                        {
//                            pointer += 1;
//                            AddPage(page);
//                        }
//                    }
//                    else
//                    {
//                        Pages.RemoveAt(0);
//                        Pages.Add(page);
//                    }
//                }
//            }
//        }
//    }
//}

