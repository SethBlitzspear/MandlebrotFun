using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MandlebrotFun
{
    class GraphAreaVM : BaseVM
    {
        private WriteableBitmap writeableBitmap;

        private GraphArea theGraph;
        bool movingPrime, movingOffset;
        double viewHeight, viewWidth;
        private BitmapImage unlocked, locked;
        private bool lockPrimeReal, lockPrimeImaginary, lockOffsetReal, lockOffsetImaginary;

        public ObservableCollection<GraphLineVM> GraphLines = new ObservableCollection<GraphLineVM>();

        public ObservableCollection<BaseVM> PlotItems => new ObservableCollection<BaseVM>(theGraph.Items.Select((item) => GetVMObject(item)));

        public bool MovingPrime { get => movingPrime; set => movingPrime = value; }
        public bool MovingOffset { get => movingOffset; set => movingOffset = value; }
        public BitmapImage Unlocked { get => unlocked; set => unlocked = value; }
        public BitmapImage Locked { get => locked; set => locked = value; }
        public bool LockPrimeReal
        {
            get => lockPrimeReal;
            set
            {
                lockPrimeReal = value;
                OnPropertyChanged("PrimeRealPadlock");
            }
        }
        public bool LockPrimeImaginary
        {
            get => lockPrimeImaginary;
            set
            {
                lockPrimeImaginary = value;
                OnPropertyChanged("PrimeImaginaryPadlock");
            }
        }
        public bool LockOffsetReal
        {
            get => lockOffsetReal;
            set
            {
                lockOffsetReal = value;
                OnPropertyChanged("OffsetRealPadlock");
            }
        }
        public bool LockOffsetImaginary
        {
            get => lockOffsetImaginary;
            set
            {
                lockOffsetImaginary = value;
                OnPropertyChanged("OffsetImaginaryPadlock");
            }
        }

        public string DivergentText
        {
            get
            {
                if (theGraph.PrimeIteration.DivergesAt < 0)
                {
                    return "Sequence converges";
                }
                else
                {
                    return "Diverges at " + theGraph.PrimeIteration.DivergesAt;
                }
            }
        }

        public ImageSource PrimeRealPadlock
        {
            get
            {
                if (LockPrimeReal)
                {
                    return Locked;
                }
                else
                {
                    return Unlocked;
                }
            }
        }

        public ImageSource PrimeImaginaryPadlock
        {
            get
            {
                if (LockPrimeImaginary)
                {
                    return Locked;
                }
                else
                {
                    return Unlocked;
                }
            }
        }

        public ImageSource OffsetRealPadlock
        {
            get
            {
                if (LockOffsetReal)
                {
                    return Locked;
                }
                else
                {
                    return Unlocked;
                }
            }
        }

        public ImageSource OffsetImaginaryPadlock
        {
            get
            {
                if (LockOffsetImaginary)
                {
                    return Locked;
                }
                else
                {
                    return Unlocked;
                }
            }
        }
  

        
        public ImageSource CanvasImage
        {
            get
            {
                int width = (int)viewWidth;
                int height = (int)viewHeight;
               
                if (writeableBitmap == null && height > 0)
                {
                    writeableBitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgr24, null);

                    ClearImage();


                }

                return writeableBitmap;
            }
        }
        private void ClearImage()
        {
            int width = (int)viewWidth;
            int height = (int)viewHeight;
            byte[] BGColourPixels = new byte[width * height * 4];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    BGColourPixels[(x + width * y) * 4] = (byte)(255);
                    BGColourPixels[(x + width * y) * 4 + 1] = (byte)(255);
                    BGColourPixels[(x + width * y) * 4 + 2] = (byte)(255);
                    BGColourPixels[(x + width * y) * 4 + 3] = (byte)(255);
                }
            }

            Int32Rect rect = new Int32Rect(0, 0, width, height);
            writeableBitmap.WritePixels(rect, BGColourPixels, width * 4, 0);
        }

        public string PrimeRealValue { get { return Convert.ToString(theGraph.PrimeIteration.RealPart); } }
        public string PrimeImaginaryValue { get { return Convert.ToString(theGraph.PrimeIteration.ImaginaryPart); } }
        public string OffsetRealValue { get { return Convert.ToString(theGraph.Offset.RealPart); } }
        public string OffsetImaginaryValue { get { return Convert.ToString(theGraph.Offset.ImaginaryPart); } }


        public GraphAreaVM(GraphArea newArea)
        {
            theGraph = newArea;
            theGraph.GraphUpdated += GraphAreaUpdated;
            Locked = new BitmapImage();
            Locked.BeginInit();
            Locked.UriSource = new Uri("Images\\locked.png", UriKind.Relative);
            Locked.EndInit();
            Unlocked = new BitmapImage();
            Unlocked.BeginInit();
            Unlocked.UriSource = new Uri("Images\\unlocked.png", UriKind.Relative);
            Unlocked.EndInit();

            OnPropertyChanged("PrimeRealValue");
            OnPropertyChanged("PrimeImaginaryValue");
           



        }

        public bool PlotFractal
        {
            get => theGraph.PlotFractal;
            set
            {
                theGraph.PlotFractal = value;
                OnPropertyChanged("PlotText");
            }
        }
        public string PlotText
        {
            get
            {
                if(theGraph.PlotFractal)
                {
                    return "Plotting Fractal";
                }
                else
                {
                    return "Not Plotting";
                }
            }
        }

        private static BaseVM GetVMObject(BaseItem item)
        {
            if (item is GraphLine)
            {
                return new GraphLineVM((GraphLine)item);
            }
            else
            {
                return new GraphEllipseVM((ComplexNumber)item);
            }
        }
        public void GraphAreaUpdated(object sender, EventArgs e)
        {
            if (!disableUI)
            {
                OnPropertyChanged("PlotItems");
                OnPropertyChanged("DivergentText");
            }
        }

        public void SetPrimeIteration(double newReal, double newImaginary)
        {
            theGraph.PrimeIteration = new ComplexNumber() { RealPart = newReal, ImaginaryPart = newImaginary };
        }
        public void SetOffset(double newReal, double newImaginary)
        {
            SetOffset(newReal, newImaginary, theGraph);
        }
        public void SetOffset(double newReal, double newImaginary, GraphArea area)
        {
            area.Offset.RealPart = newReal;
            area.Offset.ImaginaryPart = newImaginary;
            area.BuildTail();
           
        }

        public void DrawMandlebrotSet()
        {
          ClearImage();
            for (int xCount = 0; xCount < (int)viewWidth; xCount++)
            {
                for (int yCount = 0; yCount < (int)viewHeight; yCount++)
                {
                    int iterationCount = -1;
                    bool keepIterating = true;
                    double offReal = xCount / XScale - (double)(theGraph.Scale * 2 + 1) / 2; 
                    double offImaginary = yCount / YScale - (double)(theGraph.Scale * 2 + 1) / 2;
                    double realPart = 0;
                    double imaginaryPart = 0;
                    int divergesAt = 0;

                    do
                    {
                        double newRealPart = (realPart * realPart - imaginaryPart * imaginaryPart) + offReal;
                        double newImaginaryPart = (realPart * imaginaryPart * 2) + offImaginary;



                        if (Math.Sqrt(realPart * realPart + imaginaryPart * imaginaryPart) > theGraph.Limit)
                        {
                            divergesAt = iterationCount;
                            keepIterating = false;
                        }

                        if (iterationCount++ > 50)
                        {
                            keepIterating = false;
                        }
                        realPart = newRealPart;
                        imaginaryPart = newImaginaryPart;
                    } while (keepIterating);
                    LightPixel(xCount, yCount, divergesAt, 1);
                    //Console.WriteLine(xCount + ", " + yCount + " set to " + divergesAt);
                }
                Console.WriteLine("Completed column " + xCount);
            }
            OnPropertyChanged("CanvasImage");
            /*
            GraphArea Mandlebrot = new GraphArea();
            Mandlebrot.Tail = 50;
            Mandlebrot.Limit = 10;
            Mandlebrot.PrimeIteration = new ComplexNumber() { RealPart = 0, ImaginaryPart = 0, IsPrime = true };
            
            ClearImage();
            double backupReal = theGraph.PrimeIteration.RealPart;
            double backupImaginary = theGraph.PrimeIteration.ImaginaryPart;

            for (int widthCount = 0; widthCount < Math.Floor(viewWidth); widthCount++)
            {
                for (int heightCount = 0; heightCount < Math.Floor(viewHeight); heightCount++)
                {
                    SetOffsetIterationLocation(widthCount, heightCount, Mandlebrot);
                    LightPixel(widthCount, heightCount, Mandlebrot.PrimeIteration.DivergesAt, 1);
                    Console.WriteLine(widthCount + ", " + heightCount + " set to " + Mandlebrot.PrimeIteration.DivergesAt);
                }
                Console.WriteLine("Completed column " + widthCount);
                
                OnPropertyChanged("CanvasImage");
              
            }
            
        
            OnPropertyChanged("CanvasImage");
            */
        }
        public void DrawJuliaSet()
        {
            /*
            disableUI = true;
            ClearImage();
            double backupReal = theGraph.PrimeIteration.RealPart;
            double backupImaginary = theGraph.PrimeIteration.ImaginaryPart;

            for (int widthCount = 0; widthCount < Math.Floor(viewWidth); widthCount++)
            {
                for (int heightCount = 0; heightCount < Math.Floor(viewHeight); heightCount++)
                {
                    
                    SetPrimeIterationLocation(widthCount, heightCount);
                    LightPixel(widthCount, heightCount, theGraph.PrimeIteration.DivergesAt, 1);
                }
                Console.WriteLine("Completed column " + widthCount);
            }

            SetPrimeIteration(backupReal, backupImaginary);
            disableUI = false;
            OnPropertyChanged("CanvasImage");
            */
            ClearImage();
            for (int xCount = 0; xCount < (int)viewWidth; xCount++)
            {
                for (int yCount = 0; yCount < (int)viewHeight; yCount++)
                {
                    int iterationCount = -1;
                    bool keepIterating = true;
                    double realPart = xCount / XScale - (double)(theGraph.Scale * 2 + 1) / 2; ;
                    double imaginaryPart = yCount / YScale - (double)(theGraph.Scale * 2 + 1) / 2;
                    int divergesAt = 0;

                    do
                    {
                        double newRealPart = (realPart * realPart - imaginaryPart * imaginaryPart) + theGraph.Offset.RealPart;
                        double newImaginaryPart = (realPart * imaginaryPart * 2) + theGraph.Offset.ImaginaryPart;



                        if (Math.Sqrt(realPart * realPart + imaginaryPart * imaginaryPart) > theGraph.Limit)
                        {
                            divergesAt = iterationCount;
                            keepIterating = false;
                        }

                        if (iterationCount++ > 50)
                        {
                            keepIterating = false;
                        }
                        realPart = newRealPart;
                        imaginaryPart = newImaginaryPart;
                    } while (keepIterating);
                    LightPixel(xCount, yCount, divergesAt, 1);
                   // Console.WriteLine(xCount + ", " + yCount + " set to " + divergesAt);
                }
                Console.WriteLine("Completed column " + xCount);
            }
            OnPropertyChanged("CanvasImage");
        }

        private void LightPixel(int plotX, int plotY, int divergesAt, int size)
        {
            byte[] ColorData = new byte[size * size * 4];

            for (int y = 0; y < size; y++)
            {
                if (divergesAt < 1)
                {
                    ColorData[(size * y) * 4] = 0;
                    ColorData[(size * y) * 4 + 1] = 0;
                    ColorData[(size * y) * 4 + 2] = 0;
                    ColorData[(size * y) * 4 + 3] = 0;
                }
                else
                {
                    ColorData[(size * y) * 4] = 255;
                    ColorData[(size * y) * 4 + 1] = 255;
                    ColorData[(size * y) * 4 + 2] = (byte)(255 - (divergesAt < 1 ? 255 : (divergesAt * 10)));
                    ColorData[(size * y) * 4 + 3] = 255;
                }
            }

          

            Int32Rect rect = new Int32Rect((int)(plotX - (size/2)), (int)(plotY - (size / 2)), size, size);

            writeableBitmap.WritePixels(rect, ColorData, 4* size, 0);
        }
        public void SetPrimeIterationLocation(double newXPos, double newYPos)
        {
            newPrimeX = newXPos / XScale - (double)(theGraph.Scale * 2 + 1) / 2;
            newPrimeY = newYPos / YScale - (double)(theGraph.Scale * 2 + 1) / 2;
            SetPrimeIteration(newPrimeX, newPrimeY);
        }
        public void SetOffsetIterationLocation(double newXPos, double newYPos)
        {
            SetOffsetIterationLocation(newXPos, newYPos, theGraph);
        }
        public void SetOffsetIterationLocation(double newXPos, double newYPos, GraphArea area)
        {
            newPrimeX = newXPos / XScale - (double)(theGraph.Scale * 2 + 1) / 2;
            newPrimeY = newYPos / YScale - (double)(theGraph.Scale * 2 + 1) / 2;
            SetOffset(newPrimeX, newPrimeY, area);
        }

        public void SetPrimeIterationLocation(double newXPos, double newYPos, TextBox PrimeReal, TextBox PrimeImaginary, TextBox OffsetReal, TextBox OffsetImaginary)
        {
            if (MovingPrime)
            {
                newPrimeX = newXPos / XScale - (double)(theGraph.Scale * 2 + 1) / 2; //(newXPos - (theGraph.Scale * 2 + 1)) / XScale - (theGraph.Scale * 2 + 1)/2;
                if(LockPrimeReal)
                {
                    newPrimeX = Convert.ToDouble(PrimeReal.Text);
                }

                newPrimeY = newYPos / YScale - (double)(theGraph.Scale * 2 + 1) / 2; //-((theGraph.Scale * 2 + 1)/2 - ((newYPos - (theGraph.Scale * 2 + 1)) / YScale));
                if (LockPrimeImaginary)
                {
                    newPrimeY = Convert.ToDouble(PrimeImaginary.Text);
                }
                OnPropertyChanged("PrimeRealValue");
                OnPropertyChanged("PrimeImaginaryValue");
            
                SetPrimeIteration(newPrimeX, newPrimeY);
            }

            if (MovingOffset)
            {
                double newOffsetX = newXPos / XScale - (double)(theGraph.Scale * 2 + 1) / 2;
                if(LockOffsetReal)
                {
                    newOffsetX = Convert.ToDouble(OffsetReal.Text);
                }
                double newOffsetY = newYPos / YScale - (double)(theGraph.Scale * 2 + 1) / 2;
                if (LockOffsetImaginary)
                {
                    newOffsetY = Convert.ToDouble(OffsetImaginary.Text);
                }
                OnPropertyChanged("OffsetRealValue");
                OnPropertyChanged("OffsetImaginaryValue");
                OffsetReal.Text = newOffsetX.ToString();
                OffsetImaginary.Text = newOffsetY.ToString();
                SetOffset(newOffsetX, newOffsetY, theGraph);
                if (PlotFractal)
                {
                    LightPixel((int)newXPos, (int)newYPos, theGraph.PrimeIteration.DivergesAt, 3);
               // theGraph.Plots.Add(new ComplexNumber() { RealPart = newReal, ImaginaryPart = newImaginary, IsPlot = true, DivergesAt = theGraph.PrimeIteration.DivergesAt });
            }
            }

        }

        internal void UpdateTextBoxes()
        {
            OnPropertyChanged("PrimeRealValue");
            OnPropertyChanged("PrimeImaginaryValue");

            OnPropertyChanged("OffsetRealValue");
            OnPropertyChanged("OffsetImaginaryValue");
        }

        public void FindPrime(double XPos, double YPos)
        {

            if (!MovingPrime)
            {
                GraphEllipseVM prime = null;
                foreach (BaseVM item in PlotItems)
                {
                    if (item is GraphEllipseVM)
                    {
                        GraphEllipseVM ellipse = (GraphEllipseVM)item;
                        if (ellipse.IsPrime)
                        {
                            prime = ellipse;
                        }
                    }
                }
                double diff = Math.Sqrt(Math.Pow(XPos - (prime.XPos + prime.Width/ 2),2) + Math.Pow(YPos - (prime.YPos + prime.Height / 2), 2));
                if (diff < 5)
                {
                    MovingPrime = true;
                }
            }

            if (!MovingOffset )
            {
                GraphEllipseVM offset = null;
                foreach (BaseVM item in PlotItems)
                {
                    if (item is GraphEllipseVM)
                    {
                        GraphEllipseVM ellipse = (GraphEllipseVM)item;
                        if (ellipse.IsOffset)
                        {
                            offset = ellipse;
                        }
                    }
                }
                double diff = Math.Sqrt(Math.Pow(XPos - (offset.XPos + offset.Width / 2), 2) + Math.Pow(YPos - (offset.YPos + offset.Height / 2), 2));
                if (diff < 5)
                {
                    MovingOffset = true;
                }
            }

        }

        public void SetScale(double newViewWidth, double newViewHeight)
        {
            viewHeight = newViewHeight;
            viewWidth = newViewWidth;
            setOffsets();
            writeableBitmap = null;
            OnPropertyChanged("CanvasImage");
        }

        private void setOffsets()
        {
            XOffset = viewWidth / 2;
            YOffset = viewHeight / 2;

            XScale = viewWidth / (theGraph.Scale * 2 + 1);
            YScale = viewHeight / (theGraph.Scale * 2 + 1);
            OnPropertyChanged("PlotItems");
        }
    }
}
