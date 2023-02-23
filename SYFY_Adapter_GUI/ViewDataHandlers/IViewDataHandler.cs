using SYFY_Domain.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYFY_Adapter_GUI.ViewDataHandlers
{
    internal interface IViewDataHandler
    {
        internal void DataChanged(DeleteableData d, bool deleted);
        
        internal void SaveChanges();

        internal bool ExistUnsavedChanges();

        internal void DiscardChanges();
        
        internal void LoadData();

        internal bool Handles(DeleteableData d);
    }
}
