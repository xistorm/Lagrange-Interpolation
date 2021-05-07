using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
using ExtendedNumerics;
using OxyPlot;
using OxyPlot.Series;

namespace FuncDrawAndApproximate
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            UpdateData();

            DataContext = this;
            this.MyModel = new PlotModel { Title = "Функция и интерполяционный многочлен"};
            this.MyModel.Series.Add(new FunctionSeries(approx, x[0], x[n - 1], 1e-3, "Approximation"));
            this.MyModel.Series.Add(new FunctionSeries(func, x[0], x[n - 1], 1e-3, "Function"));
            
            this.MyModelEps = new PlotModel { Title = "Погрешность"};
            this.MyModelEps.Series.Add(new FunctionSeries(delta, x[0], x[n - 1], 1e-3, "Epsilon"));
        }

        private void UpdateData()
        {
            h = 3.0 / n;
            x = new double[n];
            y = new double[n];

            for (int i = 0; i < n; ++i)
            {
                x[i] = i * h + 1;
                y[i] = F(x[i]);
            }
        }
        
        private void HandleClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(double.MaxValue);
            
            int.TryParse(Amount.Text, out n);
            
            UpdateData();
            
            this.MyModel.Series.Clear();
            this.MyModel.Series.Add(new FunctionSeries(approx, x[0], x[n - 1], 1e-3, "Approximation"));
            this.MyModel.Series.Add(new FunctionSeries(func, x[0], x[n - 1], 1e-3, "Function"));
            Plot.InvalidatePlot(true);

            this.MyModelEps.Series.Clear();
            this.MyModelEps.Series.Add(new FunctionSeries(delta, x[0], x[n - 1], 1e-3, "Epsilon"));
            PlotEps.InvalidatePlot(true);
        }

        private static double F(double x1) => Math.Sin(x1);

        /*private static double Lagrange(double x1)
        {
            float res = 0.0f;

            for (int i = 0; i < n; ++i)
            {
                float currMultiplier = (float)y[i];
                float currDivisior = 1.0f;
                for (int j = 0; j < n; ++j)
                    if (i != j)
                    {
                        currMultiplier *= (float)(x1 - x[j]);
                        currDivisior *= (float)(x[i] - x[j]);
                    }

                res += currMultiplier / currDivisior;
            }

            return res;
        }*/
        
        private static double Lagrange(double x1)
        {
            double res = 0.0d;

            for (int i = 0; i < n; ++i)
            {
                double currMultiplier = y[i];
                double currDivisior = 1.0d;
                for (int j = 0; j < n; ++j)
                    if (i != j)
                    {
                        currMultiplier *= x1 - x[j];
                        currDivisior *= x[i] - x[j];
                    }

                res += currMultiplier / currDivisior;
            }

            return res;
        }
        
        private static double Eps(double x1) => Math.Abs(F(x1) - Lagrange(x1));
        
        private Func<double, double> func = F;
        private Func<double, double> approx = Lagrange;
        private Func<double, double> delta = Eps;
        private double h;
        private static int n = 5;
        private static double[] x, y;
        public PlotModel MyModel { get; set; }
        public PlotModel MyModelEps { get; set; }
    }
}