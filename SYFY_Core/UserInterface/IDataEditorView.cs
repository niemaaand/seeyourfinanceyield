using System;
using System.Collections.Generic;
using System.Text;

namespace SYFY_Application
{
    public interface IDataEditorView
    {

        public void ShowEditor();

        public void CancelEditing();

        public void SaveChanges();


    }
}
