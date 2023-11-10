using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text;

namespace Capstone.Classes
{
    class UserInterface
    {
        private Store store = new Store();
        ShoppingCart shoppingBasket = new ShoppingCart();

        /// <summary>
        /// Provides all communication with human user.
        /// 
        /// All Console.Readline() and Console.WriteLine() statements belong 
        /// in this class.
        /// 
        /// NO Console.Readline() and Console.WriteLine() statements should be 
        /// in any other class
        /// 
        /// </summary>
        public void Run()
        {
            bool done = false;

            while (!done)
            {
                DisplayMenu();
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        ShowInventory();
                        break;
                    case "2":
                        MakeSale();
                        break;
                    case "3":
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice.");
                        break;
                }


            }
        }
        //Todo sort list alphabetcally//
        private void ShowInventory()
        {


            Console.WriteLine();

            Console.WriteLine("Id".PadRight(5) + "Name".PadRight(20) + "Wrapped".PadRight(10) + "Qty".PadRight(10) + "Price");
            Console.WriteLine();
            foreach (Items item in store.GetList())
            {
                Console.WriteLine(item.Id.PadRight(5) + item.Name.PadRight(20) + item.StringWrapped.PadRight(10) + item.Quantity.ToString().PadRight(10) + item.Price);
            }
            Console.WriteLine();


        }
        public void DisplayMenu()
        {
            Console.WriteLine("Welcome to the Main Menu");
            Console.WriteLine();
            Console.WriteLine("(1) Show Inventory");
            Console.WriteLine("(2) Make Sale");
            Console.WriteLine("(3) Quit");
        }

        public void MakeSale()
        {
            bool done = false;

            while (!done)
            {
                MakeSaleMenu();
                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":
                        Console.WriteLine();
                        Console.WriteLine("Please enter whole dollar amount");
                        decimal addfunds = decimal.Parse(Console.ReadLine());
                        Console.WriteLine(shoppingBasket.TakeMoney(addfunds));
                        break;
                    case "2":
                        SelectProduct();
                        break;
                    case "3":
                        CompleteSale();
                        done = true;
                        break;
                    default:
                        Console.WriteLine("Please enter a valid choice.");
                        return;
                }
            }

        }

        public void MakeSaleMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Sale Menu");
            Console.WriteLine();
            Console.WriteLine("(1) Take Money");
            Console.WriteLine("(2) Select Products");
            Console.WriteLine("(3) Complete Sale");
            Console.WriteLine($"Current Customer Balance: " + shoppingBasket.CurrentBalance);
        }
        public void SelectProduct()
        {
            ShowInventory();
            Console.WriteLine();
            Console.WriteLine("Please Enter a Product ID");
            string userinput = Console.ReadLine();

            if (!store.itemIds.Contains(userinput))
            {
                Console.WriteLine("Items does not exist");
                return;
            }
            Console.WriteLine("Enter in a quantity");
            int amount = int.Parse(Console.ReadLine());
            Console.WriteLine(shoppingBasket.AddToCart(amount, userinput, store.GetList()));
        }
        public void CompleteSale()
        {
            decimal totalPriceOfList = 0;
            Console.WriteLine();
            foreach (Items item in shoppingBasket.GetCart())
            {
                string totalPrice = (item.ShoppingCartQuantity * item.Price).ToString();
                Console.WriteLine($"{item.ShoppingCartQuantity.ToString().PadRight(2)} {item.Name.PadRight(20)} {item.FullCandyTypeName.PadRight(25)} {item.Price.ToString().PadRight(8)} {totalPrice} ");
                totalPriceOfList += decimal.Parse(totalPrice);
            }

            Console.WriteLine();
            Console.WriteLine($"Total Price: {totalPriceOfList}");
            Console.WriteLine();
            Console.WriteLine($"Change: {shoppingBasket.CurrentBalance}");
            List<int> change = shoppingBasket.GetChange();
            Console.WriteLine($"({change[0]}) Twenties, ({change[1]}) Tens, ({change[2]}) Fives, ({change[3]}) Ones, ({change[4]}) Quarters, ({change[5]}) Dimes, ({change[6]}) Nickels,");
            return;
        }



    }
}

