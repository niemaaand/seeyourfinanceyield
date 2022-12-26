using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Model.model
{
    public class User: DeleteableData
    {
        private string _UserName;
        private string _Password;
        private string _EMailAddress;

        public User(Guid guid): base(guid)
        {

        }
    }
}
