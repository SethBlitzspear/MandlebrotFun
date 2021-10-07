using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandlebrotFun
{
    class ComplexNumber : BaseItem
    {
        static double limit;
        double realPart;
        double imaginaryPart;
        bool isPrime = false, isOffset = false, isPlot = false;

        private int divergesAt;

        public bool IsPrime { get => isPrime; set => isPrime = value; }
        public bool IsOffset { get => isOffset; set => isOffset = value; }

        public static double Limit { get => limit; set => limit = value; }
        public double Magnitude { get => Math.Sqrt(realPart * realPart + imaginaryPart * imaginaryPart); }
        public bool InBounds => (Magnitude <= Limit);
        public double RealPart
        {
            get => realPart;
            set
            {
                realPart = value;
                if (ComplexNumberUpdated != null)
                {
                    ComplexNumberUpdated(this, new EventArgs());
                }
            }
        }
        public double ImaginaryPart
        {
            get => imaginaryPart;
            set { 
                imaginaryPart = value;
                if (ComplexNumberUpdated != null)
                {
                    ComplexNumberUpdated(this, new EventArgs());
                }
            }
        }

        public int DivergesAt { get => divergesAt; set => divergesAt = value; }
        public bool IsPlot { get => isPlot; set => isPlot = value; }

        public ComplexNumber Square(ComplexNumber Offset)
        {
            if (!IsOffset)
            {
                ComplexNumber sq = new ComplexNumber() ;

                sq.RealPart = (RealPart * RealPart - ImaginaryPart * ImaginaryPart) + Offset.RealPart;
                sq.ImaginaryPart = (RealPart * ImaginaryPart * 2) + Offset.ImaginaryPart;

                return sq;
            }
            else
            {
                return this;
            }
        }

        public ComplexNumber()
        {
            IsEllipse = true;
            DivergesAt = -1;
        }

        public event EventHandler ComplexNumberUpdated;
    }
}
