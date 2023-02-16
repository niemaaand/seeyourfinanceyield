using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SYFY_Domain.model
{
    abstract public class DeleteableData
    {
        private bool _Deleted;
        private Guid _Guid;

        public bool Deleted { get => _Deleted;}
        public Guid Guid { get => _Guid; set => _Guid = SetGuid(value); }

        public DeleteableData()
        {
            _Guid = Guid.Empty;
            _Deleted = false;
        }

        public void Delete()
        {
            //TODO 
            _Deleted = true;
        }

        private Guid SetGuid(Guid value)
        {
            if(Guid == Guid.Empty)
            {
                return value;
            }

            //TODO
            throw new InvalidOperationException("Object already has Guid!");
        } 

    }
}
