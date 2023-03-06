using SYFY_Application.DatabaseAccess;
using SYFY_Domain.model;
using SYFY_Plugin_DatabaseSimulation.DataDBSimulators;
using System;
using System.Collections.Generic;

namespace SYFY_Plugin_DatabaseSimulation
{
    public class DataBaseSimulator : IDataBaseConnectorAdapter
    {

        private BankAccountDBSimulator bankAccountDBSimulator;
        private BankingTransactionDBSimulator bankingTransactionDBSimulator;
        private TransactionTagDBSimulator transactionTagDBSimulator;
        private DBOperationer dBOperationer;

        public DataBaseSimulator()
        {
            bankAccountDBSimulator = new BankAccountDBSimulator();
            bankingTransactionDBSimulator = new BankingTransactionDBSimulator();
            transactionTagDBSimulator = new TransactionTagDBSimulator();

            dBOperationer = new DBOperationer();

        }

        bool IDataBaseConnectorAdapter.ExistsBankingTransaction(Guid guid)
        {
            return bankingTransactionDBSimulator.ExistsData(guid);
        }

        void IDataBaseConnectorAdapter.Commit()
        {
            dBOperationer.Commit();
        }

        Dictionary<Guid, BankAccount> IDataBaseConnectorAdapter.GetAllBankAccounts()
        {
            return bankAccountDBSimulator.GetAllData();
        }

        Dictionary<Guid, BankingTransaction> IDataBaseConnectorAdapter.GetAllBankingTransactions()
        {
            return bankingTransactionDBSimulator.GetAllData();
        }

        Dictionary<Guid, TransactionTag> IDataBaseConnectorAdapter.GetAllTransactionTags()
        {
            return transactionTagDBSimulator.GetAllData();
        }

        public BankAccount GetBankAccountByID(Guid guid)
        {
            return bankAccountDBSimulator.GetDataById(guid);
        }

        public BankingTransaction GetBankingTransactionById(Guid guid)
        {
            return bankingTransactionDBSimulator.GetDataById(guid);
        }

        public TransactionTag GetTransactionTagById(Guid guid)
        {
            return transactionTagDBSimulator.GetDataById(guid);
        }

        void IDataBaseConnectorAdapter.Rollback()
        {
            dBOperationer.Rollback();
        }

        BankAccount IDataBaseConnectorAdapter.SaveBankAccount(BankAccount bankAccount)
        {
            return bankAccountDBSimulator.SaveData(bankAccount);
        }

        BankingTransaction IDataBaseConnectorAdapter.SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            return bankingTransactionDBSimulator.SaveData(bankingTransaction);
        }

        TransactionTag IDataBaseConnectorAdapter.SaveTransactionTag(TransactionTag transactionTag)
        {
            return transactionTagDBSimulator.SaveData(transactionTag);
        }

        public BankAccount GetDefaultBankAccount()
        {
            return bankAccountDBSimulator.GetDefaultData();
        }

        void IDataBaseConnectorAdapter.StartDBTransaction()
        {
            dBOperationer.OpenTransaction();
        }

        public void DeleteBankingTransaction(BankingTransaction transaction)
        {
            bankingTransactionDBSimulator.DeleteData(transaction);
        }

        public void DeleteTransactionTag(TransactionTag tag)
        {
            transactionTagDBSimulator.DeleteData(tag);
        }

        public void DeleteBankAccount(BankAccount bankAccount)
        {
            bankAccountDBSimulator.DeleteData(bankAccount);
        }

        bool IDataBaseConnectorAdapter.ExistsTransactionTag(Guid guid)
        {
            return transactionTagDBSimulator.ExistsData(guid);
        }

        bool IDataBaseConnectorAdapter.ExistsBankAccount(Guid guid)
        {
            return bankAccountDBSimulator.ExistsData(guid);
        }
    }
}
