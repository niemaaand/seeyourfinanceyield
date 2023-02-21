using SYFY_Domain.data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Domain.model
{
    public class TransactionTag: DeleteableData
    {
        private string _Name;
        private string _Comment;


        public TransactionTag(IBasicEntityOperations basicEntityOperations, string name, string comment=""): 
            base(basicEntityOperations) 
        {
            Name= name;
            Comment= comment;
        }

        public string Name { get => _Name; set => _Name = value; }
        public string Comment { get => _Comment; set => _Comment = value; }

        public override void Changed(IChangeManager changeManager, bool deleted = false)
        {
            changeManager.TransactionTagChanged(this, deleted);
        }

        public override object Clone()
        {

            TransactionTag t = new TransactionTag(dbOperationer, _Name, _Comment);
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
