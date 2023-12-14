using Capstone.DAO;
using Capstone.Models;
using System.Runtime.CompilerServices;

namespace Test.TestDAO
{

    [TestClass]
    public class HomeDaoTests : BaseDaoTests
    {
        private readonly Home home1 = new Home()
        {
            ViewId = 2,
            Name = "Christmas",
            Image = "src/img/AminoAcid.jpg",
            Header = "HO HO HO",
            Body = "Merry Christmas",
            Active = false
        };
        Home home2 = new Home()
        {
            Active = false,
            Body = "body",
            Header = "header",
            Image = "image.png",
            ViewId = 0,
            Name = "name"
        };
         HomeSqlDao dao;

        [TestInitialize]
        public virtual void Init()
        {
            dao = new HomeSqlDao(connectionString);
        }

        [TestMethod]
        public void GetViewReturnsActiveView()
        {
            Home home = dao.GetView();
            Assert.AreEqual("Protein Capture Science", home.Header);
        }

        [TestMethod]
        public void PostNewHomeViewCreatesNewHome()
        {
            Home home = new Home();
            home.Active = false;
            home.Body = "body";
            home.Header = "header";
            home.Image = "image.png";
            dao.PostNewHomeView(home);
            List<Home> homes = dao.GetAllViews();
            Assert.AreEqual(4, homes.Count);
        }

        [TestMethod]
        public void PostNewHomeViewCreatedHomeViewCorrectId()
        {

            Home newHome = dao.PostNewHomeView(home2);
            home2.ViewId = 4;
            AssertAll(home2, newHome);
        }

        [TestMethod]
        public void GetViewByIdHappyPath()
        {
            Home home = dao.GetViewById(2);
            AssertAll(home1, home);
        }

        [TestMethod]
        public void GetViewByIdBadId()
        {
            Home home = dao.GetViewById(543);
            Assert.IsNull(home.Body);            
        }

        [TestMethod]
        public void GetAllViewsReturnsAllViews()
        {
            Assert.AreEqual("MKSIYFVAGLFVMLVQGSWQRSLQDTEEKSRSFSASQADPLSDPDQMNEDKRHSQGTFTSDYSKYLDSRRAQDFVQWLMNTKRNRNNIAKRHDEFERHAEGTFTSDVSSYLEGQAAKEFIAWLVKGRGRRDFPEEVAIVEELGRRHADGSFSDEMNTILDNLAARDFINWLIQTKITDRK", "MKSIYFVAGLFVMLVQGSWQRSLQDTEEKSRSFSASQADPLSDPDQMNEDKRHSQGTFTSDYSKYLDSRRAQDFVQWLMNTKRNRNNIAKRHDEFERHAEGTFTSDVSSYLEGQAAKEFIAWLVKGRGRRDFPEEVAIVEELGRRHADGSFSDEMNTILDNLAARDFINWLIQTKITDRK");
            List<Home> home = dao.GetAllViews();
            Assert.AreEqual(3, home.Count);
            AssertAll(home1, home[1]);
        }
        [TestMethod]
        public void PutViewById()
        {
            Home home = dao.GetViewById(2);
            Assert.AreEqual(home.Active, false);
            dao.UpdateHomeView(2);
            home = dao.GetViewById(2);
            Assert.AreEqual(home.Active, true);
        }
        private void AssertAll(Home expected, Home actual)
        {
            Assert.AreEqual(expected.Body, actual.Body);
            Assert.AreEqual(expected.Active, actual.Active);
            Assert.AreEqual(expected.Header, actual.Header);
            Assert.AreEqual(expected.Image, actual.Image);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.ViewId, actual.ViewId);
        }
        [TestMethod]
        public void DeleteById()
        {
            dao.DeleteViewById(1);
            Home home = dao.GetViewById(1);
            AssertAll(home, new Home()); ;
        }
    }
}