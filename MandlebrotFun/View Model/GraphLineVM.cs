using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandlebrotFun
{
    class GraphLineVM : BaseVM
    {
        private GraphLine line;

        public bool IsEllipse => line.IsEllipse;
        public double X1
        {
            get
            {
                return XOffset + line.X1 * XScale;
            }
        }
        public double X2
        {
            get
            {
                return XOffset + line.X2 * XScale;
            }
        }
        public double Y1
        {
            get
            {
                return YOffset + line.Y1 * YScale;
            }
        }
        public double Y2
        {
            get
            {
                return YOffset + line.Y2 * YScale;
            }
        }

        public GraphLineVM(GraphLine newLine)
        {
            line = newLine;
        }

      
    }
}
