using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MandlebrotFun
{
    class GraphEllipseVM : BaseVM
    {

        ComplexNumber theNumber;

        public GraphEllipseVM(ComplexNumber newNumber)
        {
            theNumber = newNumber;
            theNumber.ComplexNumberUpdated += EllipseUpdated;
        }

        public bool IsPrime => theNumber.IsPrime;
        public bool IsOffset => theNumber.IsOffset;
        public bool IsEllipse => theNumber.IsEllipse;
        public double XPos
        {
            get
            {
                if (theNumber.InBounds)
                {
                    double x1 = XOffset + theNumber.RealPart * XScale;
                    return x1 - 5;
                }
                else
                {
                    return -XScale - 1;
                }
            }
        }
        public double YPos
        {
            get
            {
                if(theNumber.InBounds)
                {
                    double y1 = YOffset + theNumber.ImaginaryPart * YScale;
                    return y1 - 5;
                }
                else
                {
                    return YScale + 1;
                }
            }
        }
        public double Width
        {
            get
            {
                if (theNumber.IsPlot)
                {
                    return 5;
                }
                return 10;
            }
        }
        public double Height
        {
            get
            {
                if (theNumber.IsPlot)
                {
                    return 5;
                }
                return 10;
            }
        }

        public Brush StrokeColor
        {
            get
            {
                if (theNumber.IsPlot)
                {
                    switch (theNumber.DivergesAt)
                    {
                        case -1:
                            return new SolidColorBrush(Colors.Black);
                        case 1:
                            return new SolidColorBrush(Colors.White);
                        case 2:
                            return new SolidColorBrush(Colors.Azure);
                        case 3:
                            return new SolidColorBrush(Colors.MintCream);
                        case 4:
                            return new SolidColorBrush(Colors.Azure);
                        case 5:
                            return new SolidColorBrush(Colors.GhostWhite);
                        case 6:
                            return new SolidColorBrush(Colors.SeaShell);
                        case 7:
                            return new SolidColorBrush(Colors.LavenderBlush);
                        case 8:
                            return new SolidColorBrush(Colors.AntiqueWhite);
                        case 9:
                            return new SolidColorBrush(Colors.PaleGoldenrod);
                        case 10:
                            return new SolidColorBrush(Colors.Wheat);
                        default:
                            return new SolidColorBrush(Colors.Lavender);
                    }
                }
                else
                {
                    if (theNumber.IsPrime)
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        if (theNumber.IsOffset)
                        {
                            return new SolidColorBrush(Colors.Green);
                        }
                        else
                        {
                            return new SolidColorBrush(Colors.Black);
                        }
                    }
                }
            }
        }
        public void EllipseUpdated(object sender, EventArgs e)
        {
            if (!disableUI)
            {
                OnPropertyChanged("XPos");
                OnPropertyChanged("YPos");
                OnPropertyChanged("Width");
                OnPropertyChanged("Height");
                OnPropertyChanged("StrokeColor");
            }
        }

        

    }
}
