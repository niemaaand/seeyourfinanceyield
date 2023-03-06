using SYFY_Domain.model;
using System;
using System.Collections.Generic;

namespace SYFY_Plugin_DatabaseSimulation.DataDBSimulators
{
    internal abstract class DataDBSimulator<T> where T : DeleteableData
    {
        protected Dictionary<Guid, T> _Data;

        protected DataDBSimulator()
        {
            _Data = new Dictionary<Guid, T>();
        }

        internal void DeleteData(T data)
        {
            if (_Data.ContainsKey(data.Guid))
            {
                _Data[data.Guid].Delete();
            }
        }

        internal bool ExistsData(Guid guid)
        {
            if (_Data.ContainsKey(guid))
            {
                return true;
            }

            return false;
        }

        internal Dictionary<Guid, T> GetAllData()
        {
            // load bank accounts from data base

            //TODO
            return _Data;
        }

        internal T GetDataById(Guid guid)
        {
            //TODO
            return _Data.ContainsKey(guid) ? _Data[guid] : null;
        }

        internal T SaveData(T data)
        {
            //TODO
            (bool save, Guid id) = Util.ShouldDataBeSaved(data, GetDataById(data.Guid), _Data.Keys);

            if (save)
            {
                data.Guid = id;
                _Data.Add(data.Guid, data);
            }
            else
            {
                // update
                _Data[data.Guid] = data;
            }

            return _Data[data.Guid];
        }

        internal abstract T GetDefaultData();

    }
}
