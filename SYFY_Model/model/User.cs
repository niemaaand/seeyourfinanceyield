using SYFY_Domain.data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Domain.model
{
    public class User: DeleteableData
    {
        private string _UserName;
        private string _Password;
        private string _EMailAddress;

        public User(IBasicEntityOperations basicEntityOperations, string username, string password, string email): 
            base(basicEntityOperations)
        {

        }

        public override void Changed(IChangeManager changeManager, bool deleted = false)
        {
            throw new NotImplementedException();
        }

        public override object Clone()
        {
            throw new NotImplementedException();
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
