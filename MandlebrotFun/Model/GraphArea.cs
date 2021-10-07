using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MandlebrotFun
{
    class GraphArea
    {

        private List<ComplexNumber> iterations;
        private List<ComplexNumber> plots;
        private List<GraphLine> lines;
        private List<GraphLine> iterationLines;
        private ComplexNumber offset;
        private bool plotFractal;
       
        int scale;
        int tail;

        public int Scale { get => scale; set => scale = value; }
        internal List<ComplexNumber> Iterations { get => iterations; set => iterations = value; }
        internal List<GraphLine> Lines { get => lines; set => lines = value; }
        internal List<GraphLine> IterationLines { get => iterationLines; set => iterationLines = value; }
        public double Limit { get => ComplexNumber.Limit; set => ComplexNumber.Limit = value; }
        internal ComplexNumber Offset
        {
            get => offset;
            set
            { 
                offset = value;
                BuildTail();
                
                
            }
        }

        internal List<BaseItem> Items 
        { 
            get
            {
                List<BaseItem> items = new List<BaseItem>(Iterations);
                items.AddRange(Lines);
                items.AddRange(IterationLines);
                items.AddRange(Plots);
                items.Add(Offset);
                return items;
            }
        }


        public GraphArea()
        {
            Iterations = new List<ComplexNumber>();
            lines = new List<GraphLine>();
            IterationLines = new List<GraphLine>();

            PrimeIteration = new ComplexNumber() ;

            Offset = new ComplexNumber() { ImaginaryPart = 0.5, RealPart = 0.5, IsOffset = true};
            Limit = 5;
            PlotFractal = false;
            

            PrimeIteration.RealPart = 0;
            PrimeIteration.ImaginaryPart = 0;
            Tail = 20;
            SetScale(1);
        }

        public void SetScale(int newScale)
        {
            Scale = newScale;
            Lines.Clear();

            Lines.Add(new GraphLine(-Scale, Scale, 0, 0));
            Lines.Add(new GraphLine(0, 0, Scale, -Scale));

            
            for (int scaleCount = 1; scaleCount <= newScale; scaleCount++)
            {
                Lines.Add(new GraphLine(-0.05, 0.05, scaleCount, scaleCount)); //XPos
                Lines.Add(new GraphLine(-0.05, 0.05, -scaleCount, -scaleCount)); //XNeg
                Lines.Add(new GraphLine(scaleCount, scaleCount, -0.05, 0.05)); //YPos
                Lines.Add(new GraphLine(-scaleCount, -scaleCount, -0.05, 0.05)); //YNeg
            }
        }

        public ComplexNumber PrimeIteration
        {
            get => Iterations[0];
            set
            {
                if (Iterations.Count == 0)
                {
                    Iterations.Add(value);
                }
                else
                {
                    Iterations[0] = value;
                }
                Iterations[0].IsPrime = true;
                BuildTail();
              
               
            }
        }
        public int Tail
        {
            get => tail;
            set
            {
                tail = value;
                while (tail != Iterations.Count)
                {
                    if (tail < Iterations.Count)
                    {
                        Iterations.RemoveAt(Iterations.Count - 1);
                    }
                    else
                    {
                        Iterations.Add(Iterations[Iterations.Count - 1].Square(Offset));
                        if(PrimeIteration.DivergesAt < 0 && !Iterations[Iterations.Count - 1].InBounds)
                        {
                            PrimeIteration.DivergesAt = Iterations.Count;
                        }
                    }
                }
                if (GraphUpdated != null)
                {
                    GraphUpdated(this, new EventArgs());
                }

            }
        }

        public bool PlotFractal
        {
            get => plotFractal;
            set
            {
                plotFractal = value;
                Plots = new List<ComplexNumber>();
            }
        }
        internal List<ComplexNumber> Plots { get => plots; set => plots = value; }

        public void BuildTail()
        {
            IterationLines.Clear();
            PrimeIteration.DivergesAt = -1;
            for (int iterationCount = 1; iterationCount < Iterations.Count; iterationCount++)
            {
                Iterations[iterationCount] = Iterations[iterationCount - 1].Square(Offset);
                if (PrimeIteration.DivergesAt < 0 && !Iterations[iterationCount].InBounds)
                {
                    PrimeIteration.DivergesAt = iterationCount + 1;
                }
                IterationLines.Add(new GraphLine(Iterations[iterationCount - 1], Iterations[iterationCount]));
            }
            if (GraphUpdated != null)
            {
                GraphUpdated(this, new EventArgs());
            }
        }



        public event EventHandler GraphUpdated;
    }
}
