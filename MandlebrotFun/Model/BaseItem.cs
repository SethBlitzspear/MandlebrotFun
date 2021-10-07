using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandlebrotFun
{
    class BaseItem
    {
        private bool isEllipse;
        

        public bool IsEllipse { get => isEllipse; set => isEllipse = value; }
        public static bool DisableUI => BaseVM.disableUI;
    }
}
