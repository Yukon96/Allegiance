using RestSharp;
using System.Collections.Generic;
using TenmoClient.Models;
using TenmoServer.Models;

namespace TenmoClient.Services
{
    public class TenmoApiService : AuthenticatedApiService
    {


        public readonly string ApiUrl;

        public TenmoApiService(string apiUrl) : base(apiUrl)
        {
            if (client == null)
            {
                client = new RestClient(apiUrl);
            }

        }

        public decimal GetAccountBalance()
        {
            RestRequest request = new RestRequest("account/balance");
            IRestResponse<Account> response = client.Get<Account>(request);

            CheckForError(response);

            return response.Data.Balance;
        }
        public List<User> GetUsers()
        {
            RestRequest request = new RestRequest("userlist");
            IRestResponse<List<User>> response = client.Get<List<User>>(request);
            CheckForError(response);
            return response.Data;
        }
        public User GetUser(int id)
        {
            RestRequest request = new RestRequest($"account/{id}");
            IRestResponse<User> response = client.Get<User>(request);
            CheckForError(response);
            return response.Data;
        }

        public Transfer TransferFunds(Transfer transfer)
        {
            RestRequest request = new RestRequest(("account/send"));
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }

        public List<Transfer> TransferLog()
        {
            RestRequest request = new RestRequest("account/transferlist");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            CheckForError(response);
            return response.Data;
        }
        public Transfer GetTransferByTransferId(int id)
        {
            RestRequest request = new RestRequest($"account/transfer{id}");
            IRestResponse<Transfer> response = client.Get<Transfer>(request);
            CheckForError(response);

            return response.Data;
        }
        public List<Transfer> GetPendingRequests()
        {
            RestRequest request = new RestRequest("account/pending_requests");
            IRestResponse<List<Transfer>> response = client.Get<List<Transfer>>(request);
            CheckForError(response);
            return response.Data;
        }
        public Transfer UpdateTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest("account/transfer_confirm");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Put<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }
        public Transfer RequestTransfer(Transfer transfer)
        {
            RestRequest request = new RestRequest("");
            request.AddJsonBody(transfer);
            IRestResponse<Transfer> response = client.Post<Transfer>(request);
            CheckForError(response);
            return response.Data;
        }
        // Add methods to call api here...
    }
}



