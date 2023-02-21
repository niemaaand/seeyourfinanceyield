using SYFY_Application.BusinessLogic;
using SYFY_Domain.model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SYFY_Adapter_GUI.ViewDataHandlers
{
    internal class ViewDataTransactionTagHandler : IViewDataHandler
    {
        public Collection<TransactionTag> data { get; }
        private HashSet<DeleteableData> changedData;
        private HashSet<DeleteableData> deletedData;
        private DataManagement dataManager;

        public ViewDataTransactionTagHandler(Collection<TransactionTag> data, DataManagement dataManager)
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
                data.Remove((TransactionTag)d);
            }
        }

        public void SaveChanges()
        {
            int index;

            foreach (DeleteableData d in changedData)
            {
                index = data.IndexOf((TransactionTag)d);

                if (index != -1)
                {
                    data[index] = dataManager.SaveTransactionTag((TransactionTag)d);
                }
                else if (!deletedData.Contains(d))
                {
                    data.Add(dataManager.SaveTransactionTag((TransactionTag)d));
                }
            }

            foreach (DeleteableData d in deletedData)
            {
                d.Delete();
                //d.Save();
                dataManager.DeleteTransactionTag((TransactionTag)d);
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
                data.Add((TransactionTag)d);
            }

            foreach (TransactionTag b in changedData)
            {
                index = data.IndexOf(b);
                data[index] = dataManager.GetTransactionTagByID(b.Guid);
            }
        }

        void IViewDataHandler.LoadData()
        {
            data.Clear();
            foreach (TransactionTag tag in dataManager.GetAllTransactionTags().Values)
            {
                if (!tag.Deleted)
                {
                    data.Add(tag);
                }
            }
        }
    }
}
