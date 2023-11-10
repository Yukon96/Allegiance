using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Transactions;
using TenmoClient.Models;
using TenmoClient.Services;
using TenmoServer.Models;

namespace TenmoClient
{
    public class TenmoApp
    {
        private readonly TenmoConsoleService console = new TenmoConsoleService();
        private readonly TenmoApiService tenmoApiService;

        public TenmoApp(string apiUrl)
        {
            tenmoApiService = new TenmoApiService(apiUrl);
        }

        public void Run()
        {
            bool keepGoing = true;
            while (keepGoing)
            {
                // The menu changes depending on whether the user is logged in or not
                if (tenmoApiService.IsLoggedIn)
                {
                    keepGoing = RunAuthenticated();
                }
                else // User is not yet logged in
                {
                    keepGoing = RunUnauthenticated();
                }
            }
        }

        private bool RunUnauthenticated()
        {
            console.PrintLoginMenu();
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 2, 1);
            while (true)
            {
                if (menuSelection == 0)
                {
                    return false;   // Exit the main menu loop
                }

                if (menuSelection == 1)
                {
                    // Log in
                    Login();
                    return true;    // Keep the main menu loop going
                }

                if (menuSelection == 2)
                {
                    // Register a new user
                    Register();
                    return true;    // Keep the main menu loop going
                }
                console.PrintError("Invalid selection. Please choose an option.");
                console.Pause();
            }
        }

        private bool RunAuthenticated()
        {
            console.PrintMainMenu(tenmoApiService.Username);
            int menuSelection = console.PromptForInteger("Please choose an option", 0, 6);
            if (menuSelection == 0)
            {
                // Exit the loop
                return false;
            }

            if (menuSelection == 1)
            {
                GetBalance();
            }
            if (menuSelection == 2)
            {
                DisplayTransferLog();
            }

            if (menuSelection == 3)
            {
                DisplayPendingRequests();
            }

            if (menuSelection == 4)
            {
                TransferFunds(2, 2, "s");
            }

            if (menuSelection == 5)
            {
                TransferFunds(1, 1, "r");
            }

            if (menuSelection == 6)
            {
                // Log out
                tenmoApiService.Logout();
                console.PrintSuccess("You are now logged out");
            }

            return true;    // Keep the main menu loop going
        }

        private void Login()
        {
            LoginUser loginUser = console.PromptForLogin();
            if (loginUser == null)
            {
                return;
            }

            try
            {
                ApiUser user = tenmoApiService.Login(loginUser);
                if (user == null)
                {
                    console.PrintError("Login failed.");
                }
                else
                {
                    console.PrintSuccess("You are now logged in");
                }
            }
            catch (Exception)
            {
                console.PrintError("Login failed.");
            }
            console.Pause();
        }

        private void Register()
        {
            LoginUser registerUser = console.PromptForLogin();
            if (registerUser == null)
            {
                return;
            }
            try
            {
                bool isRegistered = tenmoApiService.Register(registerUser);
                if (isRegistered)
                {
                    console.PrintSuccess("Registration was successful. Please log in.");
                }
                else
                {
                    console.PrintError("Registration was unsuccessful.");
                }
            }
            catch (Exception)
            {
                console.PrintError("Registration was unsuccessful.");
            }
            console.Pause();
        }
        private void GetBalance()
        {
            Console.WriteLine($"Your current account balance is: {tenmoApiService.GetAccountBalance()}");
            console.Pause();
        }
        public void TransferFunds(int transferstatus, int transfertype, string requestType)
        {
            ListUser();
            int userIdTo = 0;
            if (requestType == "s") 
            {
               userIdTo = console.PromptForInteger("Enter id to transfer to, 0 to exit");
            }
            if (requestType == "r")
            {
                userIdTo = console.PromptForInteger("Enter id to requesnt transfer from");
            }
            if (tenmoApiService.GetUser(userIdTo) == null)
            {
                console.PrintError("Invalid User Id");
                TransferFunds(transferstatus, transfertype, requestType);
            }

            Console.WriteLine();
            decimal amount = console.PromptForDecimal("Enter amount to transfer");
            decimal expectBalance = tenmoApiService.GetAccountBalance() - amount;
            Transfer transfer = new Transfer();
            transfer.Amount = amount;
            transfer.OtherUserId = userIdTo;
            transfer.TransferStatusId = transferstatus;
            transfer.TransferTypeId = transfertype;
            Transfer completeTransfer = tenmoApiService.TransferFunds(transfer);
            decimal newBalance = tenmoApiService.GetAccountBalance();

            if (completeTransfer == null)
            {
                console.PrintError("Unsuccessful Transfer");
                console.Pause();
            }

            else if (completeTransfer.TransferId > 3000)
            {
                console.PrintSuccess("Successful Transfer!");
                console.Pause();
            }
        }


            public void ListUser()
            {
                foreach (User user in tenmoApiService.GetUsers())
                {
                    Console.WriteLine($"Username: " + user.Username.PadRight(4) + "User ID: " + user.UserId);
                }
            }
            public void DisplayTransferLog()
            {
                bool keepGoing = true;
                List<Transfer> transfers = new List<Transfer>();
                transfers = tenmoApiService.TransferLog();
                while (keepGoing)
                {
                Console.WriteLine();
                    Console.WriteLine("Transfers");
                    Console.WriteLine("ID      FROM                 TO                     Amount");
                    foreach (Transfer transfer in transfers)
                    {
                        Console.WriteLine(transfer.TransferId.ToString().PadRight(8) + transfer.AccountFromOwner.PadRight(21) + transfer.AccountToOwner.PadRight(22) + transfer.Amount);
                    }
                    keepGoing = DisplayTransactionDetails();


                }
            }
            public void DisplayPendingRequests()
            {
                bool keepGoing = true;
                List<Transfer> transfers = new List<Transfer>();
                transfers = tenmoApiService.GetPendingRequests();
                while (keepGoing)
                {
                Console.WriteLine();
                    Console.WriteLine("Pending Transfers");
                    Console.WriteLine("ID      TO                     Amount");
                    foreach (Transfer transfer in transfers)
                    {
                        Console.WriteLine(transfer.TransferId.ToString().PadRight(8) + transfer.AccountToOwner.PadRight(22) + transfer.Amount);
                    }
                    keepGoing = DisplayTransactionDetails();
                }
            }
            public bool DisplayTransactionDetails()
            {
                bool result = true;
            Console.WriteLine();
                int transferID = console.PromptForInteger("Enter Transaction Id for details, 0 to exit");
                if (transferID == 0)
                {
                    result = false;
                }
                Transfer transfer = tenmoApiService.GetTransferByTransferId(transferID);

                if (transfer.AccountTo == 0 && transfer.AccountFrom == 0 && transferID != 0)
                {
                    console.PrintError("Invalid Id");
                    console.Pause();
                    Console.WriteLine();
                    DisplayTransactionDetails();

                }
                else if (transferID > 3000)
                {

                    Console.WriteLine("Transfer Details");
                    Console.WriteLine($"Id: {transferID}");
                    Console.WriteLine($"From: {transfer.AccountFromOwner}");
                    Console.WriteLine($"To: {transfer.AccountToOwner}");
                    Console.WriteLine($"Type: {transfer.TypeEnglish}");
                    Console.WriteLine($"Status: {transfer.StatusEnglish}");
                if (transfer.TransferStatusId == 1) 
                { 
                    ApproveRejectNeither(transfer); 
                }
                    console.Pause();
                }
                if (transfer.TransferStatusId == 1)
            {
            }
                return result;
            }
        public void ApproveRejectNeither(Transfer transfer)
        {
            int reprove = console.PromptForInteger("(0) To Exit, (1) To Approve, (2) To Reject");
            if (reprove == 0)
            {
                DisplayPendingRequests();
            }
            if (reprove == 1)
            {
                transfer.TransferStatusId = 2;
                tenmoApiService.UpdateTransfer(transfer);
                Console.WriteLine("Transfer apporved");
            }
            if (reprove == 2)
            {
                transfer.TransferStatusId = 3;
                tenmoApiService.UpdateTransfer(transfer);
                Console.WriteLine("Transfer rejcted");
            }
        } 
        }
    }
