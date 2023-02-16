using System;
using System.Collections.Generic;

namespace SYFY_Domain.model
{
    public class BankingTransaction : DeleteableData, ICloneable
    {

        private DateTime _TransactionDate;
        private DateTime _PostingDate;
        private Guid _FromBankAccount;
        private Guid _ToBankAccount;
        private long _Amount;
        private string _Comment;
        private HashSet<Guid> _TransactionTags;

        public BankingTransaction(Guid from, Guid to, long amount, DateTime transactionDate,
            DateTime postingDate = new DateTime(),
            string comment = "", HashSet<Guid> tags = null) : base()
        {
            FromBankAccount = from;
            ToBankAccount = to;
            _Amount = amount;
            _TransactionDate = transactionDate;

            if (postingDate < transactionDate)
            {
                _PostingDate = transactionDate;
            }
            else
            {
                _PostingDate = postingDate;
            }

            _Comment = comment;

            if (tags == null)
            {
                tags = new HashSet<Guid>();
            }
            _TransactionTags = tags;

        }

        public DateTime TransactionDate { get => _TransactionDate; set => _TransactionDate = /*checkTransactionDate(value)*/ value; }
        public DateTime PostingDate { get => _PostingDate; set => _PostingDate = /*checkPostingDate(value)*/ value; }
        public Guid FromBankAccount { get => _FromBankAccount; set => _FromBankAccount = value; }
        public Guid ToBankAccount { get => _ToBankAccount; set => _ToBankAccount = value; }

        public long Amount { get => _Amount; set => _Amount = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
        public HashSet<Guid> TransactionTags { get => _TransactionTags; set => _TransactionTags = value; }
               

        public object Clone()
        {
            HashSet<Guid> tagsCopy = new HashSet<Guid>();
            foreach (Guid tag in _TransactionTags) {
                tagsCopy.Add(tag);
            }

            BankingTransaction b = new BankingTransaction(this._FromBankAccount, this._ToBankAccount, this._Amount,
                this._TransactionDate, this._PostingDate, this._Comment, tagsCopy);

            b.Guid = this.Guid;

            if (Deleted)
            {
                b.Delete();
            }

            return b;
        }


        private DateTime checkPostingDate(DateTime newPostingDate)
        {
            if (newPostingDate < _TransactionDate)
            {
                return _TransactionDate;
            }

            return newPostingDate;
        }

        private DateTime checkTransactionDate(DateTime newTransactionDate)
        {
            if (newTransactionDate > _PostingDate)
            {
                _PostingDate = newTransactionDate;
            }

            return newTransactionDate;
        }
    }
}
