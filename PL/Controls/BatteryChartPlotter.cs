using ScottPlot;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;


namespace PL.Controls
{
    public class BatteryChartPlotter
    {
        public WpfPlot WpfPlotter { get; } = new();
        private double[] _seconds = new double[1000];
        private double[] _batteries = new double[1000];
        private const int Delta = 8;
        private static int _elapsedTime;

        public BatteryChartPlotter()
        {
            WpfPlotter.Plot.XAxis.Label("Time (hours)");
            WpfPlotter.Plot.YAxis.Label("Battery (%)");
            WpfPlotter.Plot.Grid(lineStyle: LineStyle.Dot);
            WpfPlotter.Plot.Style(ScottPlot.Style.Seaborn);
            WpfPlotter.Plot.SetAxisLimitsY(0, 100);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Update(double battery)
        {
            _batteries = _batteries.Append(battery).ToArray();
            _seconds = _seconds.Append(_elapsedTime++).ToArray();
            WpfPlotter.Plot.AddScatter(_seconds, _batteries);
            WpfPlotter.Plot.AddFill(_seconds, _batteries, 0, System.Drawing.Color.FromArgb(255, 66, 88, 255));
            WpfPlotter.Plot.SetAxisLimitsX(_elapsedTime - Delta, _elapsedTime + Delta);
            Application.Current.Dispatcher.Invoke(() => WpfPlotter.Refresh());
        }
    }
}
