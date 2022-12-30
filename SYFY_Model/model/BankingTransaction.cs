using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Model.model
{
    public class BankingTransaction : DeleteableData, ICloneable
    {

        private DateTime _TransactionDate;
        private DateTime _PostingDate;
        private Guid _FromBankAccount;
        private Guid _ToBankAccount;
        private long _Amount;
        private string _Comment;
        private Dictionary<Guid, TransactionTag> _TransactionTags;

        public BankingTransaction(Guid from, Guid to, long amount, DateTime transactionDate,
            DateTime postingDate = new DateTime(),
            string comment = "", Dictionary<Guid, TransactionTag> tags = null) : base()
        {
            _FromBankAccount = from;
            _ToBankAccount = to;
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
                tags = new Dictionary<Guid, TransactionTag>();
            }
            _TransactionTags = tags;

        }

        public DateTime TransactionDate { get => _TransactionDate; set => _TransactionDate = value; }
        public DateTime PostingDate { get => _PostingDate; set => _PostingDate = value; }
        public Guid FromBankAccount { get => _FromBankAccount; set => _FromBankAccount = value; }
        public Guid ToBankAccount { get => _ToBankAccount; set => _ToBankAccount = value; }
        public long Amount { get => _Amount; set => _Amount = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
        public Dictionary<Guid, TransactionTag> TransactionTags { get => _TransactionTags; set => _TransactionTags = value; }

        public string AmountAsString { get
            { 
                long eur = Amount / 100;
                long cents = Amount % 100;

                return eur + "." + cents;
            } }

        public object Clone()
        {
            BankingTransaction b = new BankingTransaction(this._FromBankAccount, this._ToBankAccount, this._Amount,
                this._TransactionDate, this._PostingDate, this._Comment, this._TransactionTags);
            
            b.Guid = this.Guid;

            return b;
        }               
    }
}
