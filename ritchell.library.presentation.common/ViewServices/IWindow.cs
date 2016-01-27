using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ritchell.library.presentation.common.ViewServices
{
    public interface IWindow
    {
        object WindowParam { get; set; }
        void Show();
        void ShowDialog();
        void CloseWindow();
    }
}
