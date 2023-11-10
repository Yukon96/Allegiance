using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class FileIO
    {
        string fileName = @"C:\Store\inventory.csv";
        public List<Items> ReadFile() 
        {
            List<Items> inventoryList = new List<Items>();
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    
                    while (!sr.EndOfStream)
                    {
                        string item = sr.ReadLine();
                        string[] itemDetails = item.Split("|");

                        Items items = new Items();
                        items.Id = itemDetails[1];
                        items.CandyType = itemDetails[0];
                        items.Name = itemDetails[2];
                        items.Price = decimal.Parse(itemDetails[3]);
                        
                        if (itemDetails[4] == "T")
                        {
                            items.Wrapped = true;
                        }
                        if (itemDetails[4] == "F")
                        {
                            items.Wrapped = false;
                        }
                        inventoryList.Add(items);
                    }
                }
            }
            catch ( IOException ex )
            {
                Console.WriteLine("Not a valid file");
            }
            return inventoryList;
        }

        public void WriteFilePurchaseRecipet (List<Items> item, decimal currentBalance)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Store\Log.txt", true))
            {
                foreach (Items purchase in item)
                {
                    sw.WriteLine($"{DateTime.Now} { purchase.ShoppingCartQuantity} {purchase.Name} {purchase.Id} ${(purchase.Price * purchase.ShoppingCartQuantity)} ${currentBalance}");
                }
                sw.WriteLine($"{DateTime.Now} CHANGE GIVEN ${currentBalance} $0.00");
            }
        }
        public void WriteFileAddMoney(decimal recievedFunds, decimal currentBalance)
        {
            using (StreamWriter sw = new StreamWriter(@"C:\Store\Log.txt", true))
            {
                sw.WriteLine($"{DateTime.Now} MONEY RECIEVED: ${recievedFunds} ${currentBalance}");
            }
        }
        
    }
}
