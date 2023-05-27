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

        public bool ExistsBankingTransaction(Guid guid)
        {
            return bankingTransactionDBSimulator.ExistsData(guid);
        }

        public void Commit()
        {
            dBOperationer.Commit();
        }

        public Dictionary<Guid, BankAccount> GetAllBankAccounts()
        {
            return bankAccountDBSimulator.GetAllData();
        }

        public Dictionary<Guid, BankingTransaction> GetAllBankingTransactions()
        {
            return bankingTransactionDBSimulator.GetAllData();
        }

        public Dictionary<Guid, TransactionTag> GetAllTransactionTags()
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

        public void Rollback()
        {
            dBOperationer.Rollback();
        }

        public BankAccount SaveBankAccount(BankAccount bankAccount)
        {
            return bankAccountDBSimulator.SaveData(bankAccount);
        }

        public BankingTransaction SaveBankingTransaction(BankingTransaction bankingTransaction)
        {
            return bankingTransactionDBSimulator.SaveData(bankingTransaction);
        }

        public TransactionTag SaveTransactionTag(TransactionTag transactionTag)
        {
            return transactionTagDBSimulator.SaveData(transactionTag);
        }

        public BankAccount GetDefaultBankAccount()
        {
            return bankAccountDBSimulator.GetDefaultData();
        }

        public void StartDBTransaction()
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

        public bool ExistsTransactionTag(Guid guid)
        {
            return transactionTagDBSimulator.ExistsData(guid);
        }

        public bool ExistsBankAccount(Guid guid)
        {
            return bankAccountDBSimulator.ExistsData(guid);
        }
    }
}
