using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandlebrotFun
{
    class GraphLine : BaseItem
    {
        ComplexNumber endPoint, startPoint;

        double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
        public event EventHandler GraphLineUpdated;

        internal ComplexNumber EndPoint { get => endPoint; set => endPoint = value; }
        internal ComplexNumber StartPoint { get => startPoint; set => startPoint = value; }
        public double X1
        {
            get
            {
                if (StartPoint == null)
                {
                    return x1;
                }
                else
                {
                    if(!StartPoint.InBounds || !EndPoint.InBounds)
                    {
                        return 0;
                    }
                    return StartPoint.RealPart;
                }
            }
            set
            {
                x1 = value;
                update();
            }
        }

        public double Y1
        {
            get
            {
                if (StartPoint == null)
                {
                    return y1;
                }
                else
                {
                    if (!StartPoint.InBounds || !EndPoint.InBounds)
                    {
                        return 0;
                    }
                    return StartPoint.ImaginaryPart;
                }
            }
            set
            {
                x2 = value;
                update();
            }
        }

        public double X2
        {
            get
            {
                if (EndPoint == null)
                {
                    return x2;
                }
                else
                {
                    if (!StartPoint.InBounds || !EndPoint.InBounds)
                    {
                        return 0;
                    }
                    return EndPoint.RealPart;
                }
            }
            set
            {
                y1 = value;
                update();
            }
        }

        public double Y2
        {
            get
            {
                if (EndPoint == null)
                {
                    return y2;
                }
                else
                {
                    if (!StartPoint.InBounds || !EndPoint.InBounds)
                    {
                        return 0;
                    }
                    return EndPoint.ImaginaryPart;
                }
            }
            set
            {
                y2 = value;
                update();
            }
        }

        public void update()
        {
            if (GraphLineUpdated != null)
            {
                GraphLineUpdated(this, new EventArgs());
            }
        }

        public GraphLine(ComplexNumber newStartPoint, ComplexNumber newEndPoint)
        {
            StartPoint = newStartPoint;
            EndPoint = newEndPoint;
            IsEllipse = false;
        }
        public GraphLine(double x1, double x2, double y1, double y2)
        {
            this.x1 = x1;
            this.x2 = x2;
            this.y1 = y1;
            this.y2 = y2;
            IsEllipse = false;
        }
    }
}
