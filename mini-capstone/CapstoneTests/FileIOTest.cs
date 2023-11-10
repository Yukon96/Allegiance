using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class FileIOTest
    {
        [TestMethod]

        public void ReadFile()
        {
            //arrange

            FileIO fileAccess = new FileIO();

            //act
            List<Items> items = fileAccess.ReadFile();

            //assert
            Assert.IsTrue(items.Count > 0);
        }

        //[TestMethod]
        //public void WriteFile()
        //{
        //    //arrange
        //    FileIO fileWriter = new FileIO();
        //    //act
        //    List<Items> items = new List<Items>();
        //    Items candy= new Items();
        //    items.Add (candy);
        //    Items chocolate = new Items();
        //    chocolate.ShoppingCartQuantity = 2;
        //    chocolate.Name = "Chocolate";
        //    chocolate.Id = "CH";
        //    chocolate.Price = 1M;
        //    items.Add(chocolate);


        //    fileWriter.WriteFilePurchaseRecipet(items, 0);
        //    using (StreamReader sr = new StreamReader(@"C:\Store\Log.txt"))
        //    {
        //        string readBody = sr.ReadToEnd();
        //        int lastLineIndex = readBody.LastIndexOf('\n');
        //        string actual = readBody.Substring(lastLineIndex - 44);

        //        Assert.AreEqual($"{DateTime.Now} CHANGE GIVEN $0 $0.00", actual);
        //    }
        //}
        
    }
}
