using System;
using System.Collections.Generic;
using System.Text;



namespace SYFY_Model.model
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
        private Decimal _Amount;
        private CURRENCIES _Currency;
        private ACCOUNTTYPE _Accounttype;   

       
        public string Name { get => _Name; set => _Name = value; }
        public string Iban { get => _Iban; set => _Iban = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
        public decimal Amount { get => _Amount; }
        public CURRENCIES Currency { get => _Currency; set => _Currency = value; }
        public ACCOUNTTYPE Accounttype { get => _Accounttype; set => _Accounttype = value; }

        public BankAccount(Guid guid): base(guid)
        {
           
        }

        public void AddAmount(decimal amount)
        {
            _Amount += amount;
        }

        public void SubAmount(decimal amount)
        {
            _Amount -= amount;
        }
    }
}
