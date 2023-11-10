using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Items
    {
        public int ShoppingCartQuantity { get; set; } = 0;
        public string FullCandyTypeName
        {
            get
            {
                string result = "";

                if (CandyType == "CH")
                {
                    result = "Chocolate Confectionery";
                }
                if (CandyType == "LI")
                {
                    result = "Licorce and Jellies";
                }
                if (CandyType == "HC")
                {
                    result = "Hard Tack Confectionery";
                }
                if (CandyType == "SR")
                {
                    result = "Sour Flavor Candies";
                }
                return result;
            }
        }
        public string CandyType { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Quantity { get; set; } = "100";
        public bool Wrapped { get; set; }
        public string Id { get; set; }

        public string StringWrapped
        {
            get
            {
                string result = "";

                if (Wrapped == true)
                {
                    result = "Y";
                }
                if (Wrapped == false)
                {
                    result = "N";
                }
                return result;
            }

        }
        public override string ToString()

        {
            return Id;
        }

    }
}
