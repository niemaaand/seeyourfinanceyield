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

        private IDataManager _DataManger;

        public TransactionExecution(IDataManager dataManager)
        {
            _DataManger = dataManager;
        }


        public void SaveBankingTransaction(BankingTransaction transaction)
        {
            BankAccount from = _DataManger.getBankAccountByID(transaction.FromBankAccount);
            BankAccount to = _DataManger.getBankAccountByID(transaction.ToBankAccount);

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
            BankAccount fromOld = _DataManger.getBankAccountByID(oldTransaction.FromBankAccount);
            BankAccount toOld = _DataManger.getBankAccountByID(oldTransaction.ToBankAccount);
            BankAccount fromNew = _DataManger.getBankAccountByID(newTransaction.FromBankAccount);
            BankAccount toNew = _DataManger.getBankAccountByID(newTransaction.ToBankAccount);
            decimal amountOld = oldTransaction.Amount;
            decimal amountNew = newTransaction.Amount;

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
                _DataManger.SaveBankingTransaction(oldTransaction);


                _DataManger.Commit();

            }catch(Exception ex)
            {
                _DataManger.Rollback();
            }

        }


    }
}
