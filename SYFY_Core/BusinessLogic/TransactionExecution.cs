using SYFY_Application.DatabaseAccess;
using SYFY_Model.model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SYFY_Application.BusinessLogic
{
    internal class TransactionExecution
    {

        private IDataBaseConnector _DataManger;

        public TransactionExecution(IDataBaseConnector dataManager)
        {
            _DataManger = dataManager;
        }


        public BankingTransaction SaveBankingTransaction(BankingTransaction transaction)
        {
            BankAccount from = _DataManger.GetBankAccountByID(transaction.FromBankAccount);
            BankAccount to = _DataManger.GetBankAccountByID(transaction.ToBankAccount);

            if (from.Amount <= transaction.Amount)
            {
                //TODO
                //throw new Exception("Not enough money!");
            }

            from.SubAmount(transaction.Amount);
            to.AddAmount(transaction.Amount);


            //save data to db
            try
            {
                _DataManger.StartDBTransaction();
                _DataManger.SaveBankAccount(from);
                _DataManger.SaveBankAccount(to);
                BankingTransaction newTransaction = _DataManger.SaveBankingTransaction(transaction);
                _DataManger.Commit();

                return newTransaction;

            }
            catch (Exception ex) 
            {
                _DataManger.Rollback();
                throw new Exception("Saving Transaction failed. \n" + ex.Message);
            }
        }

        public BankingTransaction AlterBankingTransaction(BankingTransaction oldTransaction, BankingTransaction newTransaction)
        {
            // check money balances
            BankAccount fromOld = _DataManger.GetBankAccountByID(oldTransaction.FromBankAccount);
            BankAccount toOld = _DataManger.GetBankAccountByID(oldTransaction.ToBankAccount);
            BankAccount fromNew = _DataManger.GetBankAccountByID(newTransaction.FromBankAccount);
            BankAccount toNew = _DataManger.GetBankAccountByID(newTransaction.ToBankAccount);
            long amountOld = oldTransaction.Amount;
            long amountNew = newTransaction.Amount;

            fromOld.AddAmount(amountOld);
            fromNew.SubAmount(amountNew);

            toOld.SubAmount(amountOld);
            toNew.AddAmount(amountNew);

            // save data to db
            try
            {
                _DataManger.StartDBTransaction();
                _DataManger.SaveBankAccount(fromOld);
                _DataManger.SaveBankAccount(toOld);

                if (!fromOld.Guid.Equals(fromNew.Guid))
                {
                    _DataManger.SaveBankAccount(fromNew);
                }

                if (!toOld.Guid.Equals(toNew.Guid))
                {
                    _DataManger.SaveBankAccount(toNew);
                }

                // transaction has to be saved only once, because it is the same transaction. It was just altered. 
                newTransaction = _DataManger.SaveBankingTransaction(newTransaction);


                _DataManger.Commit();

            }catch(Exception ex)
            {
                _DataManger.Rollback();
                throw new Exception("Updating transaction failed. \n" + ex.Message);
            }

            return newTransaction;

        }


    }
}
