using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Capstone.Classes
{
    /// <summary>
    /// Most of the "work" (the data and the methods) of dealing with inventory and money 
    /// should be created or controlled from this class, this class needs to hold information about the inventory, holding money, making change, valid id, 
    /// sufficient funds, all the work that interacts with the data should be in this class
    /// </summary>
    public class Store
    {

        private FileIO fileIO = new FileIO();
        

        private List<Items> ListOfItems { get; }

        public List<string> itemIds
        {
            get
            {

                List<string> strings = new List<string>();

                foreach (Items item in ListOfItems)
                {
                    strings.Add(item.Id);
                }
                return strings;
            }

        }

        Items items = new Items();

        private List<Items> shoppingCart = new List<Items>();


        //*************CONSTRUCTOR*****************\\

        public Store()
        {
            ListOfItems = fileIO.ReadFile();

        }

        //********************METHODS******************\\

        
        public List<Items> GetList()
        {
            return ListOfItems;
        }












    }
}
