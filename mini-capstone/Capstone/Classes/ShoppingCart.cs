using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class ShoppingCart
    {
        FileIO fileIO = new FileIO();
        private List<Items> shoppingCart = new List<Items>();
        public decimal CurrentBalance { get; set; } = 0;
        public List<Items> GetCart()
        {
            return shoppingCart;
        }

        public string AddToCart(int amount, string userinput, List<Items> itemList)
        {
            Items theItem = new Items();
            string result = "";
            foreach (Items item in itemList)
            {
                if (item.Quantity != "SOLD OUT")
                {
                    int quantity = int.Parse(item.Quantity);
                    if (userinput == item.ToString() && quantity >= amount && amount > 0)
                    {
                        theItem = item;
                        if (shoppingCart.Contains(theItem))
                        {
                            theItem.ShoppingCartQuantity += amount;
                            CurrentBalance -= (amount * theItem.Price);
                        }
                        if (CurrentBalance < (amount * theItem.Price))
                        {
                            result = "Insufficient Funds";
                        }
                        if (CurrentBalance > (amount * theItem.Price) && !shoppingCart.Contains(item))
                        {
                            quantity -= amount;
                            item.Quantity = quantity.ToString();
                            theItem.ShoppingCartQuantity = amount;
                            shoppingCart.Add(theItem);
                            CurrentBalance -= (amount * theItem.Price);
                        }
                        if (quantity == 0)
                        {
                            item.Quantity = "SOLD OUT";
                            result = "SOLD OUT";
                        }
                    }
                }

                if (amount < 0)
                {
                    result = "Please Enter A Valid Amount";
                }
            }
            return result;
        }
        public string TakeMoney(decimal addFunds)
        {
            string result = "";
            if (CurrentBalance + addFunds < 1000 && addFunds <= 100)
            {
                CurrentBalance += addFunds;
                result = "Funds Added";
            }
            else if (addFunds > 100)
            {
                result = "Dollar amount cannot exceed $100.00";
            }
            else if (CurrentBalance + addFunds >= 1000)
            {
                result = "Balance cannot exceed $1000.00";
            }
            fileIO.WriteFileAddMoney(addFunds, CurrentBalance);
            return result;
        }
        public List<int> GetChange()
        {
            List<int> denominations = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
            fileIO.WriteFilePurchaseRecipet(shoppingCart, CurrentBalance);
            while (CurrentBalance > 0)
            {
                if (CurrentBalance - 20 >= 0)
                {
                    denominations[0] += 1;
                    CurrentBalance -= 20;
                }
                else if (CurrentBalance - 10 >= 0)
                {
                    denominations[1] += 1;
                    CurrentBalance -= 10;
                }
                else if (CurrentBalance - 5 >= 0)
                {
                    denominations[2] += 1;
                    CurrentBalance -= 5;
                }
                else if (CurrentBalance - 1 >= 0)
                {
                    denominations[3] += 1;
                    CurrentBalance -= 1;
                }
                else if (CurrentBalance - .25m >= 0)
                {
                    denominations[4] += 1;
                    CurrentBalance -= .25m;
                }
                else if (CurrentBalance - .10m >= 0)
                {
                    denominations[5] += 1;
                    CurrentBalance -= .10m;
                }
                else if (CurrentBalance - .05m >= 0)
                {
                    denominations[6] += 1;
                    CurrentBalance -= .05m;
                }
            }
            return denominations;
        }
    }
}

