using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandlebrotFun
{
    public class BaseVM : INotifyPropertyChanged
    {
        protected static double XOffset = 1, YOffset = 1, XScale = 1, YScale = 1, newPrimeX = 0, newPrimeY = 0;
        public static bool disableUI = false;
        
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                if (!disableUI)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        #endregion
    }
}
