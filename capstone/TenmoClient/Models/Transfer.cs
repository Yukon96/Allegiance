namespace TenmoServer.Models
{
    public class Transfer
    {
        public int TransferId { get; set; }
        public int TransferStatusId { get; set; }
        public int TransferTypeId { get; set; }
        public int AccountTo { get; set; }
        public int AccountFrom { get; set; }
        public decimal Amount { get; set; }
        public string AccountToOwner { get; set; }
        public string AccountFromOwner { get; set; }
        public int OtherUserId { get; set; }
        public string StatusEnglish
        {
            get
            {
                string result = "";
                if (TransferStatusId == 1)
                {
                    result = "Pending";
                }
                if (TransferStatusId == 2)
                {
                    result = "Approved";
                }
                if (TransferStatusId == 3)
                {
                    result = "Rejected";
                }
                return result;
            }
        }
        public string TypeEnglish
        {
            get
            {
                string result = "";
                if (TransferStatusId == 1)
                {
                    result = "Request";
                }
                if (TransferStatusId == 2)
                {
                    result = "Send";
                }
                return result;
            }
        }
    }
}
