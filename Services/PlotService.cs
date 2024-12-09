namespace Queuing_System.Services
{
    using OxyPlot;
    using OxyPlot.Annotations;
    using OxyPlot.Series;
    using OxyPlot.SkiaSharp;
    using System.IO;

    public class PlotService
    {
        public PlotModel CreateQueuePlot(List<double> timePoints, List<int> queueLengths)
        {
            var plotModel = new PlotModel
            {
                Title = "Queue Simulation - Column Chart",
            };

            // Define the ColumnSeries
            var rectangleBarSeries = new RectangleBarSeries
            {
                Title = "Number of Customers in System",
                FillColor = OxyColors.SkyBlue,
            };
            // Adding data points to the ColumnSeries
            for (int i = 0; i < queueLengths.Count; i++)
            {
                rectangleBarSeries.Items.Add(new RectangleBarItem
                {
                    X0 = i - 0.4, // Left side of the bar
                    X1 = i + 0.4, // Right side of the bar
                    Y0 = 0, // Bottom of the bar
                    Y1 = queueLengths[i] // Height of the bar
                });
            }

            // Add the ColumnSeries to the PlotModel
            plotModel.Series.Add(rectangleBarSeries);

            // X-Axis: CategoryAxis for Time Points
            var categoryAxis = new OxyPlot.Axes.CategoryAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                Title = "Time",
                ItemsSource = timePoints,
                LabelFormatter = index => timePoints[(int)index].ToString("F4"), // Format time points
                Minimum = -0.5,
                IntervalLength = 40, // Adjusts spacing between ticks
                MajorStep = 1, // Ensure tick marks align with each index
                MinorStep = 0, // Optional: Add minor ticks for finer detail
                MajorTickSize = 0,
                MinorTickSize = 0,
                Angle = -45, // Rotate labels for readability
                GapWidth = 0, // Adjust spacing between bars
                Unit = "sec"
            };
            plotModel.Axes.Add(categoryAxis);

            // Y-Axis: LinearAxis for Queue Lengths
            var linearAxis = new OxyPlot.Axes.CategoryAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                Title = "Number of Customers",
                LabelFormatter = index => ((int)index).ToString(), // Format time points
                Minimum = 0,
                MajorStep = 1, // Ensure tick marks align with each index
                MinorStep = 0, // Optional: Add minor ticks for finer detail
                MajorTickSize = 0,
                MinorTickSize = 0,
                MinimumPadding = 0.1, // Prevent bars from touching the axis
                MaximumPadding = 0.1,// Add padding at the top
                //MajorGridlineStyle = LineStyle.Automatic,
                //MinorGridlineStyle = LineStyle.Automatic,
                Unit = "Person"
            };
            plotModel.Axes.Add(linearAxis);
            foreach (var height in queueLengths)
            {
                var gridline = new LineAnnotation
                {
                    Type = LineAnnotationType.Horizontal,
                    Y = height, // Position of the gridline at bar peak
                    Color = OxyColors.Gray,
                    LineStyle = LineStyle.Dash, // Dashed style for custom gridlines
                    Layer = AnnotationLayer.BelowSeries
                };
                plotModel.Annotations.Add(gridline);
            }

            return plotModel;
        }

        // Generate the image from PlotModel and return it as a byte array
        public byte[] ExportPlotToPng(PlotModel plotModel, int width, int height)
        {
            using (var stream = new MemoryStream())
            {
                // Ensure the PlotModel is not null and valid
                if (plotModel == null)
                {
                    throw new ArgumentNullException(nameof(plotModel), "PlotModel cannot be null.");
                }

                // Use OxyPlot.SkiaSharp's PngExporter for rendering the plot to PNG
                var exporter = new PngExporter
                {
                    Width = width,
                    Height = height,
                };

                exporter.Export(plotModel, stream);

                // Return the image as a byte array
                return stream.ToArray();
            }
        }
    }
}
