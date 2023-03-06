using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Application.DatabaseAccess
{
    /// <summary>
    /// Interface, which should be implemented by every data base connector. 
    /// Contains all the methods needed to connect to the data base and perform data base transactons (ACID-rules). 
    /// </summary>
    public interface IDataBaseConnectorAdapter
    {
        public void StartDBTransaction();

        public void Commit();

        public void Rollback();

        /// <summary>
        /// Saves the given bank account to the data base. If bank account with same GUID already exists, 
        /// this bank account is updated. 
        /// </summary>
        /// <param name="bankAccount"></param>
        /// <returns>"same" bank account with GUID</returns>
        public BankAccount SaveBankAccount(BankAccount bankAccount);

        /// <summary>
        /// Saves the given banking transaction to the data base. If a banking transaction with the same GUID 
        /// already exists in data base, it is updated. 
        /// </summary>
        /// <param name="bankingTransaction"></param>
        /// <returns>"same" transaction but with GUID</returns>
        public BankingTransaction SaveBankingTransaction(BankingTransaction bankingTransaction);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionTag"></param>
        /// <returns>"same" tag but with GUID</returns>
        public TransactionTag SaveTransactionTag(TransactionTag transactionTag);
       
        /// <summary>
        /// Gets bank accounts from data base. Might also cache them. 
        /// </summary>
        /// <returns>All bank accounts which match the given filter.</returns>
        public Dictionary<Guid, BankAccount> GetAllBankAccounts();

        /// <summary>
        /// Gets banking transaction from data base. Might also cache them. 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Guid, BankingTransaction> GetAllBankingTransactions();

        public Dictionary<Guid, TransactionTag> GetAllTransactionTags();

        public BankAccount GetBankAccountByID(Guid guid);

       
        public BankingTransaction GetBankingTransactionById(Guid guid);
        public bool ExistsBankingTransaction(Guid guid);
        void DeleteBankingTransaction(BankingTransaction oldTransaction);
        void DeleteTransactionTag(TransactionTag tag);
        void DeleteBankAccount(BankAccount bankAccount);
        BankAccount GetDefaultBankAccount();
        TransactionTag GetTransactionTagById(Guid guid);
        bool ExistsTransactionTag(Guid guid);
        bool ExistsBankAccount(Guid guid);
    }
}
