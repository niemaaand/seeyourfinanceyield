using SYFY_Domain.data;
using System;

namespace SYFY_Domain.model
{
    abstract public class DeleteableData : ICloneable
    {
        private bool _Deleted;
        private Guid _Guid;
        protected IBasicEntityOperations dbOperationer;

        public bool Deleted { get => _Deleted;}
        public Guid Guid { get => _Guid; set => _Guid = SetGuid(value); }

        public DeleteableData(IBasicEntityOperations dbOperationer)
        {
            _Guid = Guid.Empty;
            _Deleted = false;
            this.dbOperationer = dbOperationer;
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

        public abstract object Clone();
        public abstract DeleteableData Save();

        public abstract DeleteableData Reload();

        public abstract void Changed(IChangeManager changeManager, bool deleted = false);

    }
}
