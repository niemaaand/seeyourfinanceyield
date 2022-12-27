using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Plugin_DatabaseSimulation
{
    public class DataBaseSimulator : IDataBaseConnector
    {

        private Dictionary<Guid, BankingTransaction> _Transactions;
        private Dictionary<Guid, BankAccount> _BankAccounts;
        private Dictionary<Guid, TransactionTag> _TransactionTags;
        private bool _CurrentlyPerformingTransaction;

        public DataBaseSimulator()
        {
            _Transactions = new Dictionary<Guid, BankingTransaction>();
            _BankAccounts = new Dictionary<Guid, BankAccount>();
            _TransactionTags = new Dictionary<Guid, TransactionTag>();

            _CurrentlyPerformingTransaction= false;

            TestDataCreator.CreateTestData(this);

        }

        void IDataBaseConnector.Commit()
        {
            //TODO
            if (_CurrentlyPerformingTransaction == false)
            {
                throw new NullReferenceException("No data base transaction running currently!");
            }

            _CurrentlyPerformingTransaction = false;
            Console.WriteLine("Commit Transaction!");
        }


        Dictionary<Guid, BankAccount> IDataBaseConnector.GetAllBankAccounts()
        {
            // load bank accounts from data base


            //TODO
            return _BankAccounts;
        }

        Dictionary<Guid, BankingTransaction> IDataBaseConnector.GetAllBankingTransactions()
        {
            //TODO
            return _Transactions;
        }

        BankAccount IDataBaseConnector.GetBankAccountByID(Guid guid)
        {
            //TODO
            return _BankAccounts[guid];
        }

        BankingTransaction IDataBaseConnector.GetBankingTransactionById(Guid guid)
        {
            //TODO
            return _Transactions[guid];
        }

        public Guid NewGuid()
        {
            //TODO
            Guid g = Guid.NewGuid();
            return g;
        }

        void IDataBaseConnector.Rollback()
        {
            //TODO
            _CurrentlyPerformingTransaction = false;
            Console.WriteLine("Rollback!");
        }

        BankAccount IDataBaseConnector.SaveBankAccount(BankAccount bankAccount)
        {
            //TODO
            bankAccount.Guid= Guid.NewGuid();

            if (!_BankAccounts.ContainsKey(bankAccount.Guid))
            {
                _BankAccounts.Add(bankAccount.Guid, bankAccount);
                return bankAccount;
            }

            throw new Exception("Already in DB!");

        }

        BankingTransaction IDataBaseConnector.SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            //TODO
            bankingTransaction.Guid= Guid.NewGuid();

            if (!_Transactions.ContainsKey(bankingTransaction.Guid))
            {
                _Transactions.Add(bankingTransaction.Guid, bankingTransaction);
                return bankingTransaction;
            }

            throw new Exception("Already in DB!");
        }

        void IDataBaseConnector.StartDBTransaction()
        {
            _CurrentlyPerformingTransaction = true;
            Console.WriteLine("Transaction started...");
            //TODO
        }

        void IDataBaseConnector.UpdateBankAccount(BankAccount bankAccount)
        {
            //TODO
            if (_BankAccounts.ContainsKey(bankAccount.Guid))
            {
                _BankAccounts[bankAccount.Guid] = bankAccount;
                return;
            }

            throw new Exception("Not in DB!");
        }

        void IDataBaseConnector.UpdateBankingTransaction(BankingTransaction bankingTransaction)
        {
            //TODO
            if (_Transactions.ContainsKey(bankingTransaction.Guid))
            {
                _Transactions[bankingTransaction.Guid] = bankingTransaction;
                return;
            }

            throw new Exception("Not in DB!");

        }
               

        TransactionTag IDataBaseConnector.SaveTransactionTag(TransactionTag transactionTag)
        {
            throw new NotImplementedException();
        }

    }


    internal static class TestDataCreator
    {       
        public static void CreateTestData(IDataBaseConnector dataBaseConnector)
        {
            BankAccount giroSparkasse = dataBaseConnector.SaveBankAccount(new BankAccount("Giro Sparkasse"));
            BankAccount giroVolksbank = dataBaseConnector.SaveBankAccount(new BankAccount("Giro Volksbank"));
            BankAccount depotSmartbroker = dataBaseConnector.SaveBankAccount(new BankAccount("Depot Smartbroker"));
            BankAccount depotTradeRepublic = dataBaseConnector.SaveBankAccount(new BankAccount("Depot TradeRepublic"));
            BankAccount festgeld = dataBaseConnector.SaveBankAccount(new BankAccount("Festgeld"));
            BankAccount tagesgeld = dataBaseConnector.SaveBankAccount(new BankAccount("Tagesgeld"));


            dataBaseConnector.SaveBankingTransaction(new BankingTransaction(giroSparkasse.Guid, depotTradeRepublic.Guid,
                3078, new DateTime(2022, 12, 27)));
            dataBaseConnector.SaveBankingTransaction(new BankingTransaction(giroSparkasse.Guid, depotSmartbroker.Guid,
                201, new DateTime(2022, 12, 29)));
        }
    }
}
