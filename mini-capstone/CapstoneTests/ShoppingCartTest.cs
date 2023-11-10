using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class ShoppingCartTest
    {
        [TestMethod]

        public void TestCurrentBalance()
        {
            //arrange
            ShoppingCart cart = new ShoppingCart();
            //act
            cart.CurrentBalance = 0;
            //assert
            Assert.AreEqual(0, cart.CurrentBalance);
        }

        [TestMethod]

        public void TestGetList()
        {
            //arrange
            ShoppingCart cart = new ShoppingCart();
            //act
            List<Items> items = cart.GetCart();
            //assert
            CollectionAssert.AllItemsAreNotNull(items);
        }


        [TestMethod]
        public void TestAddToCart()
        {

            {
                // Arrange
                ShoppingCart shoppingCart = new ShoppingCart();
                List<Items> itemList = new List<Items>
                {
                     new Items { Name = "Candy1", Quantity = "5", Price = 10.0M, Id = "C1" },
                     new Items { Name = "Candy2", Quantity = "10", Price = 5.0M, Id = "C2" }
                };

                // Act
                shoppingCart.TakeMoney(100M);
                string result1 = shoppingCart.AddToCart(2, "C1", itemList);
                string result2 = shoppingCart.AddToCart(3, "C2", itemList);
                List<Items> cartItems = shoppingCart.GetCart();

                // Assert

                Assert.AreEqual(2, cartItems[0].ShoppingCartQuantity);
                Assert.AreEqual(65.0M, shoppingCart.CurrentBalance);
            }
        }


        [TestMethod]
        public void AddToCartInsufficientFunds()
        {
            // Arrange
            ShoppingCart shoppingCart = new ShoppingCart();
            List<Items> itemList = new List<Items>
            {
             new Items { Name = "Candy1", Quantity = "5", Price = 10.0M, Id = "C1" }
            };
            // Act
            string result = shoppingCart.AddToCart(3, "C1", itemList);
            // Assert
            Assert.AreEqual("Insufficient Funds", result);
        }

        [TestMethod]
        public void AddToCartItemNotAvailable()
        {
            // Arrange
            ShoppingCart shoppingCart = new ShoppingCart();
            List<Items> itemList = new List<Items>
            {
              new Items { Name = "Candy1", Quantity = "30", Price = 1.0M, Id = "C1" }

            };

            // Act
            shoppingCart.TakeMoney(100M);
            string result = shoppingCart.AddToCart(30, "C1", itemList);
            // Assert
            Assert.AreEqual("SOLD OUT", result);
        }

        [TestMethod]
        public void AddToCartInvalidAmount()
        {
            // Arrange
            ShoppingCart shoppingCart = new ShoppingCart();
            List<Items> itemList = new List<Items>
        {
            new Items { Name = "Candy1", Quantity = "5", Price = 10.0M, Id = "C1" }
        };

            // Act
            string result = shoppingCart.AddToCart(-1, "Item1", itemList);

            // Assert
            Assert.AreEqual("Please Enter A Valid Amount", result);
        }
        [TestMethod]

        public void GetChangeTest()
        {
            // Arrange
            ShoppingCart shoppingCart = new ShoppingCart();

            //act
            shoppingCart.TakeMoney(100M);
            List<int> money = new List<int>() { 5, 0, 0, 0, 0, 0, 0 };
            List<int> newMoney = shoppingCart.GetChange();
            shoppingCart.TakeMoney(50M);
            List<int> money2 = new List<int>() { 2, 1, 0, 0, 0, 0, 0 };
            List<int> newMoney2 = shoppingCart.GetChange();
            //assert
            CollectionAssert.AreEqual(money, newMoney);
            CollectionAssert.AreEqual(money2, newMoney2);
        }
    }
}

