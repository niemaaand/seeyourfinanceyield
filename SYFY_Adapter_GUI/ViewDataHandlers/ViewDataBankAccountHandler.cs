using SYFY_Application.BusinessLogic;
using SYFY_Domain.model;
using System.Collections.ObjectModel;

namespace SYFY_Adapter_GUI.ViewDataHandlers
{
    /*
    internal class DataHandler<T> : IViewDataHandler where T : DeleteableData{

        private Collection<T> data { get; }
        private HashSet<DeleteableData> changedData;
        private HashSet<DeleteableData> deletedData;
        private DataManagement dataManager;
        private IDataLoadable dataLoader;

        public DataHandler(Collection<T> data, DataManagement dataManager)
        {
            //TODO:
            //dataManager implements IDataLoadable -> spezifisch pro Datentyp -> Transaction/BankAccount/Tag 
            //BankAccount needs attribute bankAccountManager


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
                data.Remove((T)d);
            }
        }

        public void SaveChanges()
        {
            int index;

            foreach (DeleteableData d in changedData)
            {
                index = data.IndexOf((T)d);

                if (index != -1)
                {
                    //data[index] = dataManager.SaveBankAccount((BankAccount)d);
                    data[index] = (T)d.Save();
                }
                else if (!deletedData.Contains(d))
                {
                    //data.Add(dataManager.SaveBankAccount((BankAccount)d));
                    data.Add((T)d.Save());
                }
            }

            foreach (DeleteableData d in deletedData)
            {
                d.Delete();
                d.Save();
                //dataManager.DeleteBankAccount((BankAccount)d);
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
                data.Add((T)d);
            }

            foreach (T b in changedData)
            {
                index = data.IndexOf(b);
                data[index] = (T)b.Reload(); //dataManager.GetBankAccountByID(b.Guid);
            }
        }

        void IViewDataHandler.LoadData()
        {
            data.Clear();
            foreach (T b in dataLoader.LoadData().Values /*dataManager.GetAllBankAccounts().Values*//*)
            {
                if (!b.Deleted)
                {
                    data.Add(b);
                }
            }
        }



    }*/



    public class ViewDataBankAccountHandler : IViewDataHandler
    {
        private Collection<BankAccount> data { get; }
        private HashSet<DeleteableData> changedData;
        private HashSet<DeleteableData> deletedData;
        private DataManagement dataManager;

        public ViewDataBankAccountHandler(Collection<BankAccount> data, DataManagement dataManager)
        {
            this.data = data;
            this.dataManager = dataManager;
            changedData = new HashSet<DeleteableData>();
            deletedData = new HashSet<DeleteableData>();
        }

        public void DataChanged(DeleteableData d, bool deleted = false, bool newlyCreated = false)
        {
            if (Handles(d))
            {
                if (newlyCreated)
                {
                    data.Add((BankAccount)d);
                }

                //TODO       
                if (!deleted)
                {
                    changedData.Add(d);
                }
                else
                {
                    deletedData.Add(d);
                    data.Remove((BankAccount)d);
                }
            }
        }

        public void SaveChanges()
        {
            int index;

            foreach (DeleteableData d in changedData)
            {
                index = data.IndexOf((BankAccount)d);

                if (index != -1)
                {
                    data[index] = dataManager.SaveBankAccount((BankAccount)d);
                }
                else if (!deletedData.Contains(d))
                {
                    data.Add(dataManager.SaveBankAccount((BankAccount)d));
                }
            }

            foreach (DeleteableData d in deletedData)
            {
                d.Delete();
                //d.Save();
                dataManager.DeleteBankAccount((BankAccount)d);
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
                data.Add((BankAccount)d);
            }

            foreach (BankAccount b in changedData)
            {
                index = data.IndexOf(b);
                if (dataManager.ExistsBankAccount(b.Guid))
                {
                    data[index] = dataManager.GetBankAccountByID(b.Guid);
                }
                else
                {
                    data.Remove(b);
                }
            }

            deletedData.Clear();
            changedData.Clear();
        }

        void IViewDataHandler.LoadData()
        {
            data.Clear();
            foreach (BankAccount b in dataManager.GetAllBankAccounts().Values)
            {
                if (!b.Deleted)
                {
                    data.Add(b);
                }
            }
        }

        public bool Handles(DeleteableData d)
        {
            return d is BankAccount;
        }
    }
}
