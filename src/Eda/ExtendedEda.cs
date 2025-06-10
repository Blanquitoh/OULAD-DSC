using OuladEtlEda.DataAccess;
using OuladEtlEda.Domain;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;

namespace OuladEtlEda.Eda;

public static class ExtendedEda
{
    public static void Run(OuladContext context)
    {
        Directory.CreateDirectory("plots");
        PlotConfusionMatrix(context, Path.Combine("plots", "confusion.png"));
        PlotCorrelationMatrix(context, Path.Combine("plots", "correlation.png"));
        PlotBoxplot(context, Path.Combine("plots", "boxplot.png"));
        PlotNormalDistribution(context, Path.Combine("plots", "normal.png"));
        PlotScatter(context, Path.Combine("plots", "scatter.png"));
    }

    public static void PlotConfusionMatrix(OuladContext ctx, string path)
    {
        var genders = Enum.GetValues<Gender>();
        var results = Enum.GetValues<FinalResult>();
        var matrix = new double[genders.Length, results.Length];

        foreach (var s in ctx.StudentInfos)
            matrix[(int)s.Gender, (int)s.FinalResult]++;

        var model = new PlotModel { Title = "Confusion Matrix", Background = OxyColors.White };
        model.Axes.Add(new CategoryAxis
            { Position = AxisPosition.Left, Key = "y", ItemsSource = genders.Select(g => g.ToString()).ToList() });
        model.Axes.Add(new CategoryAxis
            { Position = AxisPosition.Bottom, Key = "x", ItemsSource = results.Select(r => r.ToString()).ToList() });

        var heatMap = new HeatMapSeries
        {
            X0 = 0,
            X1 = results.Length,
            Y0 = 0,
            Y1 = genders.Length,
            Data = matrix
        };
        model.Series.Add(heatMap);
        PngExporter.Export(model, path, 600, 400);
    }

    public static void PlotCorrelationMatrix(OuladContext ctx, string path)
    {
        var rows = ctx.StudentInfos
            .Select(s => new double[]
            {
                (int)s.AgeBand,
                s.NumOfPrevAttempts,
                s.StudiedCredits
            }).ToList();

        var cols = 3;
        var matrix = new double[cols, cols];
        for (var i = 0; i < cols; i++)
        for (var j = 0; j < cols; j++)
        {
            var i1 = i;
            var j1 = j;
            matrix[i, j] = Pearson(rows.Select(r => r[i1]), rows.Select(r => r[j1]));
        }

        var model = new PlotModel { Title = "Correlation Matrix", Background = OxyColors.White };
        model.Axes.Add(new CategoryAxis
            { Position = AxisPosition.Left, ItemsSource = new[] { "Age", "Attempts", "Credits" } });
        model.Axes.Add(new CategoryAxis
            { Position = AxisPosition.Bottom, ItemsSource = new[] { "Age", "Attempts", "Credits" } });
        var heatMap = new HeatMapSeries
        {
            X0 = 0,
            X1 = cols,
            Y0 = 0,
            Y1 = cols,
            Data = matrix
        };
        model.Series.Add(heatMap);
        PngExporter.Export(model, path, 600, 400);

        static double Pearson(IEnumerable<double> xs, IEnumerable<double> ys)
        {
            var x = xs.ToArray();
            var y = ys.ToArray();
            if (x.Length == 0) return 0;
            var meanX = x.Average();
            var meanY = y.Average();
            double sumXY = 0, sumX2 = 0, sumY2 = 0;
            for (var i = 0; i < x.Length; i++)
            {
                var dx = x[i] - meanX;
                var dy = y[i] - meanY;
                sumXY += dx * dy;
                sumX2 += dx * dx;
                sumY2 += dy * dy;
            }

            return sumXY / Math.Sqrt(sumX2 * sumY2);
        }
    }

    public static void PlotBoxplot(OuladContext ctx, string path)
    {
        var model = new PlotModel { Title = "StudiedCredits by Age", Background = OxyColors.White };
        var series = new BoxPlotSeries();
        var ageBands = Enum.GetValues<AgeBand>();
        var x = 0;
        foreach (var band in ageBands)
        {
            var values = ctx.StudentInfos.Where(s => s.AgeBand == band).Select(s => (double)s.StudiedCredits)
                .OrderBy(v => v).ToList();
            if (values.Count == 0)
            {
                x++;
                continue;
            }

            var q1 = Percentile(values, 0.25);
            var median = Percentile(values, 0.5);
            var q3 = Percentile(values, 0.75);
            var iqr = q3 - q1;
            var lower = values.Where(v => v >= q1 - 1.5 * iqr).DefaultIfEmpty(values.First()).First();
            var upper = values.Where(v => v <= q3 + 1.5 * iqr).DefaultIfEmpty(values.Last()).Last();
            var item = new BoxPlotItem(x, lower, q1, median, q3, upper);
            foreach (var v in values.Where(v => v < lower || v > upper))
                item.Outliers.Add(v);
            series.Items.Add(item);
            x++;
        }

        model.Series.Add(series);
        model.Axes.Add(new CategoryAxis
            { Position = AxisPosition.Bottom, ItemsSource = ageBands.Select(a => a.ToString()).ToList() });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
        PngExporter.Export(model, path, 600, 400);

        static double Percentile(IList<double> ordered, double p)
        {
            if (ordered.Count == 0) return 0;
            var i = (ordered.Count - 1) * p;
            var lo = (int)Math.Floor(i);
            var hi = (int)Math.Ceiling(i);
            if (lo == hi) return ordered[lo];
            return ordered[lo] + (ordered[hi] - ordered[lo]) * (i - lo);
        }
    }

    public static void PlotNormalDistribution(OuladContext ctx, string path)
    {
        var values = ctx.StudentInfos.Select(s => (double)s.StudiedCredits).ToList();
        if (values.Count == 0) return;
        var mean = values.Average();
        var std = Math.Sqrt(values.Sum(v => Math.Pow(v - mean, 2)) / values.Count);

        var bins = 20;
        var min = values.Min();
        var max = values.Max();
        var binWidth = (max - min) / bins;

        var hist = new BarSeries { Title = "Histogram" };
        for (var i = 0; i < bins; i++)
        {
            var start = min + i * binWidth;
            var end = start + binWidth;
            var count = values.Count(v => v >= start && v < end);
            hist.Items.Add(new BarItem(count));
        }

        var normal = new LineSeries { Title = "Normal" };
        for (var i = 0; i <= 100; i++)
        {
            var x = min + (max - min) * i / 100.0;
            var y = Normal(x, mean, std) * values.Count * binWidth;
            normal.Points.Add(new DataPoint(x, y));
        }

        var model = new PlotModel { Title = "Normal Distribution", Background = OxyColors.White };
        model.Series.Add(hist);
        model.Series.Add(normal);
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
        PngExporter.Export(model, path, 600, 400);

        static double Normal(double x, double mu, double sigma)
        {
            return 1.0 / (Math.Sqrt(2 * Math.PI) * sigma) * Math.Exp(-Math.Pow(x - mu, 2) / (2 * sigma * sigma));
        }
    }

    public static void PlotScatter(OuladContext ctx, string path)
    {
        var series = new ScatterSeries { MarkerType = MarkerType.Circle };
        foreach (var s in ctx.StudentInfos)
            series.Points.Add(new ScatterPoint(s.NumOfPrevAttempts, s.StudiedCredits));
        var model = new PlotModel { Title = "Attempts vs Credits", Background = OxyColors.White };
        model.Series.Add(series);
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Attempts" });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Credits" });
        PngExporter.Export(model, path, 600, 400);
    }
}