using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Core
{
    public interface IDataEditorView
    {

        public void ShowEditor();

        public void CancelEditing();

        public void SaveChanges();


    }
}
