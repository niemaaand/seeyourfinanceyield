using System;
using SYFY_Model.model;
using SYFY_Core.DatabaseAccess;
using SYFY_Plugin_DatabaseSimulation;
using SYFY_Application.DatabaseAccess;
using SYFY_Application.BusinessLogic;
using System.Linq;

namespace SYFY
{
    class SYFYMain
    {
        static void Main(string[] args)
        {
            IDataBaseConnector dataManager = new DataBaseSimulator();

            BankAccount b1 = dataManager.GetAllBankAccounts().ElementAt(0).Value;
            BankAccount b2 = dataManager.GetAllBankAccounts().ElementAt(1).Value;

            BankingTransaction t = new BankingTransaction(b1.Guid, b2.Guid, 3456, DateTime.Today);

            TransactionExecution transactionExecution = new TransactionExecution(dataManager);
            transactionExecution.SaveBankingTransaction(t);

            BankingTransaction tnew = (BankingTransaction) t.Clone();
            tnew.Comment = "Neuer Kommentar";
            tnew.Amount = 108;
            transactionExecution.AlterBankingTransaction(t, tnew);
           

            // get data
            /*IBankingAccountLoader bankingAccountLoader = new BankingAccountLoadSimulator();
            bankingAccountLoader.saveBankAccount(bankingAccountLoader.createNewBankAccount("testfrom", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash));
            bankingAccountLoader.createNewBankAccount("testto", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash);
            
            IBankingTransactionLoader bankingTransactionLoader = new BankingTransactionLoadSimulator();
            //bankingTransactionLoader.SaveBankingTransaction(bankingTransactionLoader.CreateNewBankingTransaction());
            */
            Console.WriteLine("Hello World!");

        }
    }
}
