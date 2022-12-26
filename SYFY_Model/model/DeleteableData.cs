using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Model.model
{
    public class DeleteableData
    {
        private bool _Deleted;
        private Guid _Guid;

        public bool Deleted { get => _Deleted;}
        public Guid Guid { get => _Guid; }

        public DeleteableData(Guid guid)
        {
            _Guid = guid;
            _Deleted = false;
        }
        public void Delete()
        {
            //TODO 
            _Deleted = true;
        }

    }
}
