using System;
using SYFY_Domain.model;

namespace SYFY_Plugin_DatabaseSimulation.DataDBSimulators
{
    internal class BankAccountDBSimulator : DataDBSimulator<BankAccount>
    {
        public BankAccountDBSimulator() : base()
        {

        }

        internal override BankAccount GetDefaultData()
        {
            Guid defaultId = new Guid("F4635B58-8D25-40A1-95B3-C9CDB424205A");

            if (!_Data.ContainsKey(defaultId))
            {
                _Data.Add(defaultId, new BankAccount("", comment: "DEFAULT_EMPTY_BANKACCOUNT") { Guid = defaultId });
            }

            return _Data[defaultId];
        }
    }
}
