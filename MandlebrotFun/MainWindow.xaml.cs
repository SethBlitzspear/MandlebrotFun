using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MandlebrotFun
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        Ellipse[] IterationCircles = new Ellipse[10];
        Line[] IterationLines = new Line[10];
        GraphAreaVM theVM;
        public MainWindow()
        {
            InitializeComponent();


            theVM= new GraphAreaVM(new GraphArea());
            DataContext = theVM;
            
            PlotIteration(0, 0);
        }

        private void PlotButton_Click(object sender, RoutedEventArgs e)
        {

            //theVM.SetPrimeIteration(-0.5, 0.70);
            //   double x = Convert.ToDouble(RealValue.Text);
            //  double y = Convert.ToDouble(ImaginaryValue.Text);
            // PlotIteration(x, y);
            theVM.DrawJuliaSet();
        }

        private void PlotAxis()
        {/*
            XAxisLine.X1 = XOffset;
            XAxisLine.X2 = XOffset;
            XAxisLine.Y1 = YOffset - 2 * YScale;
            XAxisLine.Y2 = YOffset + 2 * YScale;
            XAxisLine.InvalidateVisual();
          

            YAxisLine.X1 = XOffset - 2 * XScale;
            YAxisLine.X2 = XOffset + 2 * XScale;
            YAxisLine.Y1 = YOffset;
            YAxisLine.Y2 = YOffset;

            XPos1Line.X1 = XOffset;
            XPos1Line.X2 = XOffset + 5;
            XPos1Line.Y1 = YOffset - 2 * YScale;
            XPos1Line.Y2 = YOffset - 2 * YScale;

            XPos2Line.X1 = XOffset;
            XPos2Line.X2 = XOffset + 5;
            XPos2Line.Y1 = YOffset - 1 * YScale;
            XPos2Line.Y2 = YOffset - 1 * YScale;

            XNeg1Line.X1 = XOffset;
            XNeg1Line.X2 = XOffset - 5;
            XNeg1Line.Y1 = YOffset + 2 * YScale;
            XNeg1Line.Y2 = YOffset + 2 * YScale;

            XNeg2Line.X1 = XOffset;
            XNeg2Line.X2 = XOffset - 5;
            XNeg2Line.Y1 = YOffset + 1 * YScale;
            XNeg2Line.Y2 = YOffset + 1 * YScale;


            YPos1Line.X1 = XOffset - 2 * XScale;
            YPos1Line.X2 = XOffset - 2 * XScale;
            YPos1Line.Y1 = YOffset;
            YPos1Line.Y2 = YOffset + 5;

            YPos2Line.X1 = XOffset - 1 * XScale;
            YPos2Line.X2 = XOffset - 1 * XScale;
            YPos2Line.Y1 = YOffset;
            YPos2Line.Y2 = YOffset + 5;

            YNeg1Line.X1 = XOffset + 1 * XScale;
            YNeg1Line.X2 = XOffset + 1 * XScale;
            YNeg1Line.Y1 = YOffset;
            YNeg1Line.Y2 = YOffset - 5;

            YNeg2Line.X1 = XOffset + 2 * XScale;
            YNeg2Line.X2 = XOffset + 2 * XScale;
            YNeg2Line.Y1 = YOffset;
            YNeg2Line.Y2 = YOffset - 5;*/

           

        }
        private void PlotIteration(double x, double y)
        {
            /*
            PlotCanvas.Children.Clear();

           
           
          int Radius = 11;
            ComplexNumber start = new ComplexNumber() { RealPart = x, ImaginaryPart = y };
           
            for (int i = 0; i < 10; i++)
            {
                ComplexNumber end = start.Square();

                double x1 = XOffset + start.RealPart * XScale;
                double x2 = XOffset + end.RealPart * XScale;
                double y1 = YOffset - start.ImaginaryPart * YScale;
                double y2 = YOffset - end.ImaginaryPart * YScale;
                Ellipse circ = new Ellipse();
                circ.Width = Radius;
                circ.Height = Radius;
                circ.Stroke = new SolidColorBrush(Colors.Black);
                Canvas.SetLeft(circ, x1 - Radius / 2);
                Canvas.SetTop(circ, y1 - Radius / 2);
                PlotCanvas.Children.Add(circ);
                Radius--;

                if (!Double.IsInfinity(x2) && !Double.IsInfinity(y2))
                {
                    PlotCanvas.Children.Add(new Line()
                    {
                        X1 = x1,
                        X2 = x2,
                        Y1 = y1,
                        Y2 = y2,
                        Stroke = new SolidColorBrush(Colors.Black)
                    });

                    start = end;
                }
                else
                {
                    i = 10;
                }
            }*/
        }

        private void PlotCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            theVM.SetPrimeIterationLocation(e.GetPosition((Canvas)sender).X, e.GetPosition((Canvas)sender).Y, PrimeRealValue, PrimeImaginaryValue, OffsetRealValue, OffsetImaginaryValue);
        }
        private void PlotCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            theVM.FindPrime(e.GetPosition((Canvas)sender).X, e.GetPosition((Canvas)sender).Y);

        }

        private void PlotCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            theVM.MovingPrime = false;
            theVM.MovingOffset = false;
        }

        private void PlotCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            theVM.SetScale(((Canvas)sender).ActualWidth, ((Canvas)sender).ActualHeight);
        }

        private void PrimeRealPartLock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            theVM.LockPrimeReal = !theVM.LockPrimeReal;
        }

        private void PrimeImaginaryPartLock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            theVM.LockPrimeImaginary = !theVM.LockPrimeImaginary;
        }

        private void OffsetRealPartLock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            theVM.LockOffsetReal = !theVM.LockOffsetReal;
        }

        private void OffsetImaginaryPartLock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            theVM.LockOffsetImaginary = !theVM.LockOffsetImaginary;
        }

        private void PrimeRealValue_TextChanged(object sender, TextChangedEventArgs e)
        {   
            try
            {
                double RealValue = Convert.ToDouble(PrimeRealValue.Text);
                theVM.SetPrimeIteration(RealValue, Convert.ToDouble(theVM.PrimeImaginaryValue));
            }
            catch (Exception ex)
            {
                theVM.UpdateTextBoxes();
            }
        }

     

        private void PrimeImaginaryValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double ImaginaryValue = Convert.ToDouble(PrimeImaginaryValue.Text);
                theVM.SetPrimeIteration(Convert.ToDouble(theVM.PrimeRealValue), ImaginaryValue);
            }
            catch (Exception ex)
            {
                theVM.UpdateTextBoxes();
            }
        }

        private void OffsetRealValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double RealValue = Convert.ToDouble(OffsetRealValue.Text);
                theVM.SetOffset(RealValue, Convert.ToDouble(theVM.OffsetImaginaryValue));
            }
            catch (Exception ex)
            {
                theVM.UpdateTextBoxes();
            }
        }

        private void OffsetImaginaryValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double ImaginaryValue = Convert.ToDouble(OffsetImaginaryValue.Text);
                theVM.SetOffset(Convert.ToDouble(theVM.OffsetRealValue), ImaginaryValue);
            }
            catch (Exception ex)
            {
                theVM.UpdateTextBoxes();
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            theVM.PlotFractal = !theVM.PlotFractal;
        }

        private void MandlebrotPlot_Click(object sender, RoutedEventArgs e)
        {
            theVM.DrawMandlebrotSet();
        }
    }
}
