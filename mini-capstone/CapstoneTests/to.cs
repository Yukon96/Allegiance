using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class to
    {
        [TestMethod]
        public void TestTakeMoney()
        {
            //arrange
            Store store = new Store();
            //act
            string actual1 = store.TakeMoney(20);
            string actual2 = store.TakeMoney(1001);
            //assert
            Assert.AreEqual("Funds Added", actual1);
            Assert.AreEqual("Balance cannot exceed 1000", actual2);        }

    }
}
