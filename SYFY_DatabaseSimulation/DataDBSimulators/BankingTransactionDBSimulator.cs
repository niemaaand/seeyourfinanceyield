using SYFY_Domain.model;
using System;

namespace SYFY_Plugin_DatabaseSimulation.DataDBSimulators
{
    internal class BankingTransactionDBSimulator : DataDBSimulator<BankingTransaction>
    {

        public BankingTransactionDBSimulator() : base()
        {
        }

        internal override BankingTransaction GetDefaultData()
        {
            throw new NotImplementedException();
        }
    }
}
