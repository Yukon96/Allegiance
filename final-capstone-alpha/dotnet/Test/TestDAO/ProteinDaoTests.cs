using Capstone.DAO;
using Capstone.Models;

namespace Test.TestDAO
{
    [TestClass]
    public class ProteinDaoTests : BaseDaoTests
    {
        ProteinSqlDao dao;
        [TestInitialize]
        public virtual void Init()
        {
            dao = new ProteinSqlDao(connectionString);
        }

        [TestMethod]
        public void GetListofProteinsTest()
        {
            IList<Protein> proteins = dao.GetProteins();
            Assert.AreEqual(1, proteins.Count);
        }

        [TestMethod]
        public void GetProteinByIdHappyPath()
        {
            Protein protein = dao.GetProteinById(1);
            Assert.AreEqual(protein.SequenceName, "Insulin");
        }

        [TestMethod]
        public void GetProteinByIdInvalidId()
        {
            Protein protein = dao.GetProteinById(100);
            Assert.IsNull(protein);
        }

        [TestMethod]
        public void GetProteinsBySequenceNameHappyPath()
        {
            IList<Protein> proteins = dao.GetProteinsBySequenceName("Insulin");
            Assert.AreEqual(1, proteins.Count);
        }

        [TestMethod]
        public void GetProteinsBySequenceNameInvalidSequenceName()
        {
            IList<Protein> proteins = dao.GetProteinsBySequenceName("abc");
            Assert.AreEqual(0, proteins.Count);
        }

        [TestMethod]
        public void GetProteinsByUserIdHappyPath()
        {
            IList<Protein> proteins = dao.GetProteinsByUsername("user");
            Assert.AreEqual(1, proteins.Count);
        }

        [TestMethod]
        public void GetProteinsByUsernameInvalidName()
        {
            IList<Protein> proteins = dao.GetProteinsByUsername("hello");
            Assert.AreEqual(0, proteins.Count);
        }


        [TestMethod]
        public void CreateProteinAddsProtein()
        {
            dao.CreateProtein("name", "sequence", "description", "user", 1);
            IList<Protein> proteins = dao.GetProteins();
            Assert.AreEqual(2, proteins.Count);
        }

        [TestMethod]
        public void CreateProteinCreatedProteinNotNull()
        {
            Protein newProtein = dao.CreateProtein("name", "sequence", "description", "user", 1);
            IList<Protein> protein = dao.GetProteinsBySequenceName("name");
            AssertProperties(newProtein, protein[0]);
        }

        [TestMethod]
        public void UpdateProteinChangesProtein()
        {
            Protein newProtein = dao.UpdateProtein(1, "name", "sequence", "description", 2);
            Protein protein = dao.GetProteinById(1);
            AssertProperties(newProtein, protein);
        }


        [TestMethod]
        public void DeleteProteinDeletesProtein()
        {
            dao.DeleteProteinById(1);
            IList <Protein> proteins = dao.GetProteins();
            Assert.AreEqual(0, proteins.Count);
        }

        [TestMethod]
        public void DetectFormatDetectsNormalFormat()
        {
            int result = 0;
            string sequence = "MALWMRLLPLLALLALWGPDPAAAFVNQHLCGSHLVEALYLVCGERGFFYTPKTRREAEDLQASALSLSSSTSTWPE\r\nGLDATARAPPALVVTANIGQAGGSSSRQFRQRALGTSDSPVLFIHCPGAAGTAQGLEYRGRRVTTELVWEEVDSSPQ\r\nPQGSESLPAQPPAQPAPQPEPQQAREPSPEVSCCGLWPRRPQRSQN";
            result = dao.DetectFormat(sequence);
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void DetectFormatDetectsSpacedFormat()
        {
            int result = 0;
            string sequence = "mdamkrglcc vlllcgavfv spsqeiharf rrgarsyqvi crdektqmiy qqhqswlrpv\r\nlrsnrveycw cnsgraqchs vpvkscsepr cfnggtcqqa lyfsdfvcqc pegfagkcce";
            result = dao.DetectFormat(sequence);
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void DetectFormatDetectsNumberedAndSpacedFormat()
        {
            int result = 0;
            string sequence = "1 mqielstcff lcllrfcfsa trryylgave lswdymqsdl gelpvdarfp prvpksfpfn\r\n61 tsvvykktlf veftdhlfni akprppwmgl lgptiqaevy dtvvitlknm ashpvslhav";
            result = dao.DetectFormat(sequence);
            Assert.AreEqual(3, result);
        }


        private void AssertProperties(Protein newProtein, Protein protein)
        {
            Assert.AreEqual(newProtein.ProteinId, protein.ProteinId);
            Assert.AreEqual(newProtein.SequenceName, protein.SequenceName);
            Assert.AreEqual(newProtein.ProteinSequence, protein.ProteinSequence);
            Assert.AreEqual(newProtein.Description, protein.Description);
            Assert.AreEqual(newProtein.FormatType, protein.FormatType);
            Assert.AreEqual(newProtein.UserId, protein.UserId);
        }
    }
}
