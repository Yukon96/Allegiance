using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CapstoneTests
{
    [TestClass]
    public class StoreTest
    {
        [TestMethod]
        public void StoreTestObjectCreation()
        {
            //Arrange
            Store testObject = new Store();

            //Act (done in arrange above)

            //Assert
            Assert.IsNotNull(testObject);
        }

        [TestMethod]
        public void TestTakeMoney()
        {
            //arrange
            ShoppingCart cart = new ShoppingCart();
           
            //act
            string actual1 = cart.TakeMoney(20);
            string actual3 = cart.TakeMoney(101);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            cart.TakeMoney(100);
            string actual2 = cart.TakeMoney(100);
            
            //assert
            Assert.AreEqual("Funds Added", actual1);
            Assert.AreEqual("Dollar amount cannot exceed $100.00", actual3);
            Assert.AreEqual("Balance cannot exceed $1000.00", actual2);
            Assert.AreEqual(cart.CurrentBalance, 920);
        }



    }
}
