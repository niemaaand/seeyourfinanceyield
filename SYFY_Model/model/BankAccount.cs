using SYFY_Domain.data;
using System;


namespace SYFY_Domain.model
{
    public enum ACCOUNTTYPE
    {
        Cash,
        Giro, 
        Depot, 
        FixedDeposit
    }

    public class BankAccount: DeleteableData
    {

        private string _Name;
        private string _Iban;
        private string _Comment;
        private long _Amount;
        private CURRENCIES _Currency;
        private ACCOUNTTYPE _Accounttype;

        public string Name { get => _Name; set => _Name = value; }
        public string Iban { get => _Iban; set => _Iban = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
        public long Amount { get => _Amount; }
        public CURRENCIES Currency { get => _Currency; set => _Currency = value; }
        public ACCOUNTTYPE Accounttype { get => _Accounttype; set => _Accounttype = value; }

        public BankAccount(IBasicEntityOperations basicEntityOperations, string name, string iban="", string comment="", 
            CURRENCIES currency = CURRENCIES.EUR, ACCOUNTTYPE type=ACCOUNTTYPE.Giro): base(basicEntityOperations)
        {
            _Name= name;
            _Iban= iban;
            _Comment= comment;
            _Amount= 0;
            _Currency= currency;
            _Accounttype= type;

        }

        public void AddAmount(long amount)
        {
            _Amount += amount;
        }

        public void SubAmount(long amount)
        {
            _Amount -= amount;
        }

        new public void Delete()
        {
            if(_Amount == 0)
            {
                base.Delete();
            }
            else
            {
                throw new InvalidOperationException("Bank Account can only be deleted, if amount is zero.");
            }

        }

      
        public override object Clone()
        {
            BankAccount b = new BankAccount(this.dbOperationer, this._Name, this._Iban, this._Comment,
                           this._Currency, this._Accounttype);

            b.Guid = this.Guid;
            b._Amount = this._Amount;

            if (Deleted)
            {
                b.Delete();
            }

            return b;
        }

        public override void Changed(bool deleted)
        {
            throw new NotImplementedException();
        }

        public override DeleteableData Save()
        {
            return dbOperationer.SaveBankAccount(this);
        }

        public override DeleteableData Reload()
        {
            throw new NotImplementedException();
        }

    }
}
