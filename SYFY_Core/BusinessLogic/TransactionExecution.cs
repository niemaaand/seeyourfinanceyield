using SYFY_Application.DatabaseAccess;
using SYFY_Domain.model;
using System;

namespace SYFY_Application.BusinessLogic
{
    internal class TransactionExecution
    {

        private IDataBaseConnectorAdapter _DataManger;

        public TransactionExecution(IDataBaseConnectorAdapter dataManager)
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
            return DeleteAlterTransaction(oldTransaction, newTransaction, false);            
        }

        private BankingTransaction DeleteAlterTransaction(BankingTransaction oldTransaction, BankingTransaction newTransaction, bool delete)
        {

            // check money balances
            BankAccount fromOld = _DataManger.GetBankAccountByID(oldTransaction.FromBankAccount);
            BankAccount toOld = _DataManger.GetBankAccountByID(oldTransaction.ToBankAccount);

            BankAccount fromNew;
            BankAccount toNew;
            long amountOld = oldTransaction.Amount;


            fromOld.AddAmount(amountOld);
            toOld.SubAmount(amountOld);

            // save data to db
            try
            {
                _DataManger.StartDBTransaction();
                _DataManger.SaveBankAccount(fromOld);
                _DataManger.SaveBankAccount(toOld);

                if (!delete && oldTransaction != null && newTransaction != null)
                {
                    long amountNew = newTransaction.Amount;

                    fromNew = _DataManger.GetBankAccountByID(newTransaction.FromBankAccount);
                    toNew = _DataManger.GetBankAccountByID(newTransaction.ToBankAccount);

                    fromNew.SubAmount(amountNew);
                    toNew.AddAmount(amountNew);

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
                }
                else
                {
                    // delete transaction
                    _DataManger.DeleteBankingTransaction(oldTransaction);
                    newTransaction = _DataManger.GetBankingTransactionById(oldTransaction.Guid);
                }

                _DataManger.Commit();

            }
            catch (Exception ex)
            {
                _DataManger.Rollback();
                if (!delete)
                {
                    throw new Exception("Updating transaction failed. \n" + ex.Message);
                }
                else
                {
                    throw new Exception("Deleting transaction failed. \n" + ex.Message);
                }
            }

            return newTransaction;

        }


        internal void DeleteTransaction(BankingTransaction transaction)
        {
            DeleteAlterTransaction(transaction, null, true);
        }
    }
}
