using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Plugin_DatabaseSimulation
{
    public class DataSimulationManager : IDataManager
    {

        private Dictionary<Guid, BankingTransaction> _Transactions;
        private Dictionary<Guid, BankAccount> _BankAccounts;
        private Dictionary<Guid, TransactionTag> _TransactionTags;

        public DataSimulationManager()
        {
            _Transactions = new Dictionary<Guid, BankingTransaction>();
            _BankAccounts = new Dictionary<Guid, BankAccount>();
            _TransactionTags = new Dictionary<Guid, TransactionTag>();
        }

        void IDataManager.Commit()
        {
            //TODO
        }

        BankAccount IDataManager.createNewBankAccount(string name, string iban, string comment, CURRENCIES currency, ACCOUNTTYPE accounttype)
        {
            //TODO
            BankAccount bankAccount = new BankAccount(NewGuid());
            return bankAccount;
        }

        BankingTransaction IDataManager.CreateNewBankingTransaction(DateTime transactionDate, DateTime postingDate, 
            Guid fromBankAccount, Guid toBankAccount, decimal amount, string comment, Dictionary<Guid, TransactionTag> tags)
        {           
        //TODO
            BankingTransaction bankingTransaction = new BankingTransaction(NewGuid());
            bankingTransaction.TransactionDate = transactionDate;
            bankingTransaction.PostingDate = postingDate;
            bankingTransaction.FromBankAccount= fromBankAccount;
            bankingTransaction.ToBankAccount= toBankAccount;
            bankingTransaction.Amount= amount;
            bankingTransaction.Comment= comment;
            bankingTransaction.TransactionTags= tags;

            return bankingTransaction;
        }

        Dictionary<Guid, BankAccount> IDataManager.getAllBankAccounts()
        {
            throw new NotImplementedException();
        }

        Dictionary<Guid, BankingTransaction> IDataManager.getAllBankingTransactions()
        {
            throw new NotImplementedException();
        }

        BankAccount IDataManager.getBankAccountByID(Guid guid)
        {
            //TODO
            return _BankAccounts[guid];
        }

        BankingTransaction IDataManager.GetBankingTransactionById(Guid guid)
        {
            throw new NotImplementedException();
        }

        public Guid NewGuid()
        {
            //TODO
            Guid g = Guid.NewGuid();
            return g;
        }

        void IDataManager.Rollback()
        {
            throw new NotImplementedException();
        }

        void IDataManager.saveBankAccount(BankAccount bankAccount)
        {
            //TODO
            if (!_BankAccounts.ContainsKey(bankAccount.Guid))
            {
                _BankAccounts.Add(bankAccount.Guid, bankAccount);
                return;
            }

            throw new Exception("Already in DB!");

        }

        void IDataManager.SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            //TODO
            if (!_Transactions.ContainsKey(bankingTransaction.Guid))
            {
                _Transactions.Add(bankingTransaction.Guid, bankingTransaction);
                return;
            }

            throw new Exception("Already in DB!");
        }

        void IDataManager.StartDBTransaction()
        {
            //TODO
        }

        void IDataManager.UpdateBankAccount(BankAccount bankAccount)
        {
            //TODO
            if (_BankAccounts.ContainsKey(bankAccount.Guid))
            {
                _BankAccounts[bankAccount.Guid] = bankAccount;
                return;
            }

            throw new Exception("Not in DB!");
        }

        void IDataManager.UpdateBankingTransaction(BankingTransaction bankingTransaction)
        {
            //TODO
            if (_Transactions.ContainsKey(bankingTransaction.Guid))
            {
                _Transactions[bankingTransaction.Guid] = bankingTransaction;
                return;
            }

            throw new Exception("Not in DB!");

        }
    }
}
