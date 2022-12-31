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
            Name= name;
            Comment= comment;
        }

        public string Name { get => _Name; set => _Name = value; }
        public string Comment { get => _Comment; set => _Comment = value; }
    }
}
