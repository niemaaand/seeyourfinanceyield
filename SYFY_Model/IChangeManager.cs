using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Domain
{
    public interface IChangeManager
    {
        public void BankAccountChanged(BankAccount account, bool deleted=false);
        public void BankingTransactionChanged(BankingTransaction b, bool deleted = false);
        void TransactionTagChanged(TransactionTag transactionTag, bool deleted);
    }
}
