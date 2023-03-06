using SYFY_Domain.model;
using System;

namespace SYFY_Plugin_DatabaseSimulation.DataDBSimulators
{
    internal class TransactionTagDBSimulator : DataDBSimulator<TransactionTag>
    {
        public TransactionTagDBSimulator() : base() { }

        internal override TransactionTag GetDefaultData()
        {
            throw new NotImplementedException();
        }
    }
}
