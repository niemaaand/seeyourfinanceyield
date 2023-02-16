using SYFY_Application.BusinessLogic;
using SYFY_Domain.model;
using System.Collections.ObjectModel;

namespace SYFY_Adapter_GUI.ViewDataHandlers
{
    internal class ViewDataBankingTransactionHandler : IViewDataHandler
    {
        private ObservableCollection<BankingTransaction> data;
        private HashSet<DeleteableData> changedData;
        private HashSet<DeleteableData> deletedData;
        private DataManagement dataManager;

        public ViewDataBankingTransactionHandler(ObservableCollection<BankingTransaction> data, DataManagement dataManager)
        {
            this.data = data;
            this.dataManager = dataManager;
            changedData = new HashSet<DeleteableData>();
            deletedData = new HashSet<DeleteableData>();
        }

        public void DataChanged(DeleteableData d, bool deleted = false)
        {
            //TODO       
            if (!deleted)
            {
                changedData.Add(d);
            }
            else
            {
                deletedData.Add(d);
                data.Remove((BankingTransaction)d);
            }
        }

        public void SaveChanges()
        {
            int index;

            foreach (DeleteableData d in changedData)
            {
                index = data.IndexOf((BankingTransaction)d);

                if (index != -1)
                {
                    data[index] = dataManager.SaveBankingTransaction((BankingTransaction)d);
                }
                else if (!deletedData.Contains(d))
                {
                    data.Add(dataManager.SaveBankingTransaction((BankingTransaction)d));
                }
            }

            foreach (DeleteableData d in deletedData)
            {
                //d.Delete();
                //d.Save();
                dataManager.DeleteBankingTransaction((BankingTransaction)d);
            }

            changedData.Clear();
            deletedData.Clear();

        }

        public bool ExistUnsavedChanges()
        {
            if (changedData.Count != 0
                || deletedData.Count != 0)
            {
                return true;
            }

            return false;
        }

        public void DiscardChanges()
        {
            int index;

            foreach (DeleteableData d in deletedData)
            {
                data.Add((BankingTransaction)d);
            }

            foreach (BankingTransaction b in changedData)
            {
                index = data.IndexOf(b);
                data[index] = dataManager.GetBankingTransactionByID(b.Guid);
            }
        }

        void IViewDataHandler.LoadData()
        {
            data.Clear();
            foreach (BankingTransaction transaction in dataManager.GetAllBankingTransactions().Values)
            {
                if (!transaction.Deleted)
                {
                    data.Add(transaction);
                }
            }
        }
    }
}
