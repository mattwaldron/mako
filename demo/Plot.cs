using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Wpf;

namespace ADC.EngTools.UI
{
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    public static class Plot
    {
        private static List<OxyColor> colors = new List<OxyColor>()
        {   OxyColors.CornflowerBlue,
            OxyColors.DarkGreen,
            OxyColors.IndianRed,
            OxyColors.Orange,
            OxyColors.Black
        };

        public static PlotView Create()
        {
            var plot = new PlotView();
            plot.MinHeight = 200;
            plot.Model = new PlotModel();
            plot.InvalidatePlot(true);
            return plot;
        }

        public static PlotView AddLine(this PlotView plot, IEnumerable<OxyPlot.DataPoint> points)
        {
            var line = new LineSeries();
            line.Points.AddRange(points);
            line.Color = colors[plot.Model.Series.Count % colors.Count];
            plot.Model.Series.Add(line);
            plot.InvalidatePlot(true);
            return plot;
        }

        public static PlotView AddLine(this PlotView plot, IEnumerable<Point> points)
        {
            return plot.AddLine(points.Select(p => new OxyPlot.DataPoint(p.X, p.Y)));
        }

        public static PlotView AddLabelX(this PlotView plot, string label)
        {
            plot.Model.Axes[0].Title = label;
            return plot;
        }

        public static PlotView AddLabelY(this PlotView plot, string label)
        {
            plot.Model.Axes[1].Title = label;
            return plot;
        }

        public static PlotView AddTitle(this PlotView plot, string title)
        {
            plot.Model.Title = title;
            return plot;
        }
    }
}
