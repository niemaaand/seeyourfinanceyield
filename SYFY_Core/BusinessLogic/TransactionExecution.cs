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
    public class TransactionExecution
    {

        private IDataBaseConnector _DataManger;

        public TransactionExecution(IDataBaseConnector dataManager)
        {
            _DataManger = dataManager;
        }


        public void SaveBankingTransaction(BankingTransaction transaction)
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
                _DataManger.UpdateBankAccount(from);
                _DataManger.UpdateBankAccount(to);
                _DataManger.SaveBankingTransaction(transaction);
                _DataManger.Commit();

            }catch (Exception ex) 
            {
                _DataManger.Rollback();
            }


        }

        public void AlterBankingTransaction(BankingTransaction oldTransaction, BankingTransaction newTransaction)
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
                _DataManger.UpdateBankAccount(fromOld);
                _DataManger.UpdateBankAccount(toOld);

                if (!fromOld.Guid.Equals(fromNew.Guid))
                {
                    _DataManger.UpdateBankAccount(fromNew);
                }

                if (!toOld.Guid.Equals(toNew.Guid))
                {
                    _DataManger.UpdateBankAccount(toNew);
                }

                // transaction has to be saved only once, because it is the same transaction. It was just altered. 
                _DataManger.UpdateBankingTransaction(oldTransaction);


                _DataManger.Commit();

            }catch(Exception ex)
            {
                _DataManger.Rollback();
            }

        }


    }
}
