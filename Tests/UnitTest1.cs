using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using OS_Curse_Project;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        private bool IsIntListsSame(List<int> list1, List<int> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            for (var i = 0; i < list1.Count; i++)
            {
                if (list1[i] != list2[i])
                {
                    return false;
                }
            }

            return true;
        }


        [TestMethod]
        public void FIFOTest()
        {
            var fifo = new FIFO<int>(4);
            var b = new List<int>
                {3, 7, 4, 2, 4, 2, 6, 1, 0, 4, 2, 8, 9, 5, 7, 4, 2, 6, 9, 2};
            var expected = new List<List<int>>();
            expected.Add(new List<int> {3});
            expected.Add(new List<int> {3, 7});
            expected.Add(new List<int> {3, 7, 4});
            expected.Add(new List<int> {3, 7, 4, 2});
            expected.Add(new List<int> {3, 7, 4, 2});
            expected.Add(new List<int> {3, 7, 4, 2});
            expected.Add(new List<int> {7, 4, 2, 6});
            expected.Add(new List<int> {4, 2, 6, 1});
            expected.Add(new List<int> {2, 6, 1, 0});
            expected.Add(new List<int> {6, 1, 0, 4});
            expected.Add(new List<int> {1, 0, 4, 2});
            expected.Add(new List<int> {0, 4, 2, 8});
            expected.Add(new List<int> {4, 2, 8, 9});
            expected.Add(new List<int> {2, 8, 9, 5});
            expected.Add(new List<int> {8, 9, 5, 7});
            expected.Add(new List<int> {9, 5, 7, 4});
            expected.Add(new List<int> {5, 7, 4, 2});
            expected.Add(new List<int> {7, 4, 2, 6});
            expected.Add(new List<int> {4, 2, 6, 9});
            expected.Add(new List<int> {4, 2, 6, 9});


            for (var i = 0; i < b.Count; i++)
            {
                fifo.AddPage(b[i]);
                Assert.IsFalse(!IsIntListsSame(fifo.Pages, expected[i]));
            }
        }


        //todo дописать
        /*[TestMethod]
        public void LRUTest()
        {
            var fifo = new FIFO<int>(4);
            var b = new List<int>
                {3, 7, 4, 2, 4, 2, 6, 1, 0, 4, 2, 8, 9, 5, 7, 4, 2, 6, 9, 2};
            var expected = new List<List<int>>();
            expected.Add(new List<int> {3});
            expected.Add(new List<int> {3, 7});
            expected.Add(new List<int> {3, 7, 4});
            expected.Add(new List<int> {3, 7, 4, 2});
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());
            expected.Add(new List<int>());


            for (var i = 0; i < b.Count; i++)
            {
                fifo.AddPage(b[i]);
                Assert.IsFalse(!IsIntListsSame(fifo.Pages, expected[i]));
            }
        }*/
    }
}