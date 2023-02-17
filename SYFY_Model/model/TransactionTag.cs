using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Domain.model
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

        public override void Changed(bool deleted = false)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {

            TransactionTag t = new TransactionTag(_Name, _Comment);
            t.Guid = Guid;

            if (Deleted)
            {
                t.Delete();
            }

            return t;
        }

        public override DeleteableData Reload()
        {
            throw new NotImplementedException();
        }

        public override DeleteableData Save()
        {
            throw new NotImplementedException();
        }
    }
}
