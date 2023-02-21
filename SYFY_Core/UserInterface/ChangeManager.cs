using SYFY_Adapter_GUI.ViewDataHandlers;
using SYFY_Application.BusinessLogic;
using SYFY_Domain;
using SYFY_Domain.data;
using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Application.UserInterface
{
    public class ChangeManager : IBasicEntityOperations, IChangeManager
    {

        public IViewDataHandler bankAccountHandler { get; }
        public IViewDataHandler bankingTransactionsHandler { get; }
        public IViewDataHandler transactionTagsHandler { get; }
        private List<IViewDataHandler> dataHandlers;



        public ChangeManager(DataManagement dataManager)
        {

            bankAccountHandler = new ViewDataBankAccountHandler(new Collection<BankAccount>(), dataManager);
            bankingTransactionsHandler = new ViewDataBankingTransactionHandler(new Collection<BankingTransaction>(), dataManager);
            transactionTagsHandler = new ViewDataTransactionTagHandler(new Collection<TransactionTag>(), dataManager);

            dataHandlers = new List<IViewDataHandler>
            {
                bankAccountHandler, bankingTransactionsHandler, transactionTagsHandler
            };

        }

        public void SaveChanges()
        {
            try
            {
                foreach (IViewDataHandler dataHandler in dataHandlers)
                {
                    dataHandler.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void LoadData()
        {
            foreach (IViewDataHandler dataHandler in dataHandlers)
            {
                dataHandler.LoadData();
            }
        }

        public void DiscardChanges()
        {
            foreach (IViewDataHandler dataHandler in dataHandlers)
            {
                dataHandler.DiscardChanges();
            }
        }

        public bool ExistUnsavedChanges()
        {

            foreach (IViewDataHandler dataHandler in dataHandlers)
            {
                if (dataHandler.ExistUnsavedChanges())
                {
                    return true;
                }
            }

            return false;
        }

        public BankAccount SaveBankAccount(BankAccount b)
        {
            throw new NotImplementedException();
        }


        public void BankAccountChanged(BankAccount account, bool deleted = false)
        {
            bankAccountHandler.DataChanged(account,deleted);
        }

        public void BankingTransactionChanged(BankingTransaction b, bool deleted = false)
        {
            bankingTransactionsHandler.DataChanged(b, deleted);
        }

        public void TransactionTagChanged(TransactionTag transactionTag, bool deleted)
        {
            transactionTagsHandler.DataChanged(transactionTag, deleted);
        }

    }
}
