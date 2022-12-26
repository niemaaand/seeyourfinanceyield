using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Model.model
{
    public class BankingTransaction: DeleteableData
    {

        private DateTime _TransactionDate;
        private DateTime _PostingDate;
        private BankAccount _FromBankAccount;
        private BankAccount _ToBankAccount;
        private decimal _Amount;
        private string _Comment;
        private List<TransactionTag> _TransactionTags;

        public BankingTransaction(Guid guid): base(guid)
        {

        }

    }
}
