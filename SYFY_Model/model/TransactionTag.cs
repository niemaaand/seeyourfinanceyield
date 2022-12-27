using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Model.model
{
    public class TransactionTag: DeleteableData
    {
        private string _Name;
        private string _Comment;


        public TransactionTag(string name, string comment=""): base() 
        {
            _Name= name;
            _Comment= comment;
        }
    }
}
