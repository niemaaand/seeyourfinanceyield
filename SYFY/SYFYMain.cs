using System;
using SYFY_Model.model;
using SYFY_Core.DatabaseAccess;
using SYFY_Plugin_DatabaseSimulation;
using SYFY_Application.DatabaseAccess;
using SYFY_Application.BusinessLogic;

namespace SYFY
{
    class SYFYMain
    {
        static void Main(string[] args)
        {
            IDataManager dataManager = new DataSimulationManager();
            BankAccount ba1 = dataManager.createNewBankAccount("testfrom", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash);
            dataManager.saveBankAccount(ba1);
            BankAccount ba2 = dataManager.createNewBankAccount("testto", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash);
            dataManager.saveBankAccount(ba2);

            BankingTransaction t = dataManager.CreateNewBankingTransaction(new DateTime(2022, 03, 20), 
                new DateTime(2022, 03, 21), ba1.Guid, ba2.Guid, 20.03m, "", 
                new System.Collections.Generic.Dictionary<Guid, TransactionTag>());

            TransactionExecution transactionExecution = new TransactionExecution(dataManager);
            transactionExecution.SaveBankingTransaction(t);

            BankingTransaction tnew = (BankingTransaction) t.Clone();
            tnew.Comment = "Neuer Kommentar";
            tnew.Amount = 10.8m;
            transactionExecution.AlterBankingTransaction(t, tnew);
           

            // get data
            IBankingAccountLoader bankingAccountLoader = new BankingAccountLoadSimulator();
            bankingAccountLoader.saveBankAccount(bankingAccountLoader.createNewBankAccount("testfrom", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash));
            bankingAccountLoader.createNewBankAccount("testto", "", "Comment", CURRENCIES.CHF, ACCOUNTTYPE.Cash);

            IBankingTransactionLoader bankingTransactionLoader = new BankingTransactionLoadSimulator();
            //bankingTransactionLoader.SaveBankingTransaction(bankingTransactionLoader.CreateNewBankingTransaction());

            Console.WriteLine("Hello World!");

        }
    }
}
