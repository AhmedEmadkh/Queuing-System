namespace Queuing_System.Services
{
    using OxyPlot;
    using OxyPlot.Series;
    using OxyPlot.SkiaSharp;
    using System.IO;

    public class PlotService
    {
        public PlotModel CreateQueuePlot(List<double> timePoints, List<int> queueLengths)
        {
            var plotModel = new PlotModel { Title = "Queue Simulation" };

            var lineSeries = new LineSeries
            {
                Title = "Number of Customers in System",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Blue
            };

            for (int i = 0; i < timePoints.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(timePoints[i], queueLengths[i]));
            }

            plotModel.Series.Add(lineSeries);

            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Time",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            plotModel.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Number of Customers",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot
            });

            return plotModel;
        }

        // Generate the image from PlotModel and return it as a byte array
        public byte[] ExportPlotToPng(PlotModel plotModel, int width, int height)
        {
            using (var stream = new MemoryStream())
            {
                var exporter = new PngExporter { Width = width, Height = height };
                exporter.Export(plotModel, stream);
                return stream.ToArray();
            }
        }
    }
}
