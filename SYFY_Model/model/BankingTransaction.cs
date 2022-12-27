using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Model.model
{
    public class BankingTransaction: DeleteableData, ICloneable
    {

        private DateTime _TransactionDate;
        private DateTime _PostingDate;
        private Guid _FromBankAccount;
        private Guid _ToBankAccount;
        private decimal _Amount;
        private string _Comment;
        private Dictionary<Guid, TransactionTag> _TransactionTags;

        public BankingTransaction(Guid guid): base(guid)
        {

        }

        public DateTime TransactionDate { get => _TransactionDate; set => _TransactionDate = value; }
        public DateTime PostingDate { get => _PostingDate; set => _PostingDate = value; }
        public Guid FromBankAccount { get => _FromBankAccount; set => _FromBankAccount = value; }
        public Guid ToBankAccount { get => _ToBankAccount; set => _ToBankAccount = value; }
        public decimal Amount { get => _Amount; set => _Amount = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
        public Dictionary<Guid, TransactionTag> TransactionTags { get => _TransactionTags; set => _TransactionTags = value; }


        public object Clone()
        {
            BankingTransaction b = new BankingTransaction(this.Guid);
            b.TransactionDate = TransactionDate;
            b.PostingDate = PostingDate;
            b.FromBankAccount = FromBankAccount;
            b.ToBankAccount = ToBankAccount;
            b.Amount = Amount;
            b.Comment = Comment;
            b.TransactionTags = TransactionTags;

            return b;
        }
    }
}
