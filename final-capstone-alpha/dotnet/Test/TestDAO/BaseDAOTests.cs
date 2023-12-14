using System.Data.SqlClient;
using System.Transactions;

namespace Test.TestDAO
{
    [TestClass]
    public class BaseDaoTests
    {
        private const string AdminConnectionString = @"Server=.\SQLEXPRESS;Database=final_capstone;Trusted_Connection=True;";
        protected const string connectionString = @"Server=.\SQLEXPRESS;Database=test_capstone;Trusted_Connection=True;";
        /// <summary>
        /// The transaction for each test.
        /// </summary>
        
        private TransactionScope transaction;
        [AssemblyInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            string sql = File.ReadAllText("./database/createTestData.sql");
            using (SqlConnection conn = new SqlConnection(AdminConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            string sql2 = File.ReadAllText("./database/testData.sql");
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql2, conn);
                SqlDataReader reader = cmd.ExecuteReader();
            }
        }
        [AssemblyCleanup]
        public static void AfterAllTests()
        {
            // drop the temporary database
            string sql = File.ReadAllText("./database/dropTestData.sql");
            using (SqlConnection conn = new SqlConnection(AdminConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
        }
        [TestInitialize]
        public virtual void Setup()
        {
            // Begin the transaction
            transaction = new TransactionScope();
        }
        [TestCleanup]
        public void Cleanup()
        {
            // Roll back the transaction
            transaction.Dispose();
        }
    }
}
