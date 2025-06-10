using OuladEtlEda.DataAccess;
using OuladEtlEda.Domain;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using OxyPlot.SkiaSharp;
using Serilog;

namespace OuladEtlEda.Eda;

public static class ExtendedEda
{
    public static void Run(OuladContext context)
    {
        Log.Information("Running Extended EDA");
        Directory.CreateDirectory("plots");
        PlotConfusionMatrix(context, Path.Combine("plots", "confusion.png"));
        PlotCorrelationMatrix(context, Path.Combine("plots", "correlation.png"));
        PlotBoxplot(context, Path.Combine("plots", "boxplot.png"));
        PlotNormalDistribution(context, Path.Combine("plots", "normal.png"));
        PlotScatter(context, Path.Combine("plots", "scatter.png"));
        Log.Information("Extended EDA completed");
    }

    private static void Export(PlotModel model, string path)
    {
        using var stream = File.Create(path);

        var png = new PngExporter
        {
            Width = 600,
            Height = 400
        };

        png.Export(model, stream);
    }

    public static void PlotConfusionMatrix(OuladContext ctx, string path)
    {
        Log.Information("Generating confusion matrix: {Path}", path);

        var genderLabels = Enum.GetNames<Gender>();
        var resultLabels = Enum.GetNames<FinalResult>();

        var counts = new double[genderLabels.Length, resultLabels.Length];
        foreach (var s in ctx.StudentInfos)
            counts[(int)s.Gender, (int)s.FinalResult]++;

        var model = new PlotModel
        {
            Title = "Final-result distribution by gender",
            Background = OxyColors.White
        };

        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Gender",
            ItemsSource = genderLabels.Select(l => l.ToString()).ToList()
        });

        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Final result",
            ItemsSource = resultLabels.Select(l => l.ToString()).ToList()
        });

        var palette = OxyPalettes.Viridis(200);
        model.Axes.Add(new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Number of students",
            Palette = palette
        });

        model.Series.Add(new HeatMapSeries
        {
            X0 = 0,
            X1 = resultLabels.Length,
            Y0 = 0,
            Y1 = genderLabels.Length,
            Data = counts,
            Interpolate = false,
            RenderMethod = HeatMapRenderMethod.Rectangles
        });

        var maxCount = counts.Cast<double>().Max();
        for (int g = 0; g < genderLabels.Length; g++)
        for (int r = 0; r < resultLabels.Length; r++)
        {
            var v = counts[g, r];
            model.Annotations.Add(new TextAnnotation
            {
                Text = v.ToString("0"),
                TextPosition = new DataPoint(r + 0.5, g + 0.5),
                TextHorizontalAlignment = HorizontalAlignment.Center,
                TextVerticalAlignment = VerticalAlignment.Middle,
                StrokeThickness = 0,
                TextColor = v > maxCount * 0.5 ? OxyColors.White : OxyColors.Black
            });
        }

        Export(model, path);
    }

    public static void PlotCorrelationMatrix(OuladContext ctx, string path)
    {
        Log.Information("Generating correlation matrix: {Path}", path);

        var matrixRows = ctx.StudentInfos.Select(s => new[]
        {
            (double)(int)s.AgeBand,
            s.NumOfPrevAttempts,
            s.StudiedCredits
        }).ToList();

        var labels = new[] { "Age", "Attempts", "Credits" };
        const int n = 3;
        var corr = new double[n, n];

        for (var i = 0; i < n; i++)
        for (var j = 0; j < n; j++)
        {
            var j1 = j;
            var i1 = i;
            corr[i, j] = Pearson(
                matrixRows.Select(r => r[i1]),
                matrixRows.Select(r => r[j1]));
        }

        var model = new PlotModel
        {
            Title = "Correlation Matrix",
            Background = OxyColors.White
        };

        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = string.Empty,
            ItemsSource = labels
        });
        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = string.Empty,
            ItemsSource = labels
        });

        var palette = OxyPalette.Interpolate(
            201,
            OxyColors.DarkBlue, OxyColors.White, OxyColors.DarkRed);

        model.Axes.Add(new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Correlation",
            Minimum = -1,
            Maximum = 1,
            AxisDistance = 8,
            AxislineThickness = 20,
            Palette = palette
        });

        model.Series.Add(new HeatMapSeries
        {
            X0 = 0,
            X1 = n,
            Y0 = 0,
            Y1 = n,
            Data = corr,
            Interpolate = false,
            RenderMethod = HeatMapRenderMethod.Rectangles
        });

        for (int y = 0; y < n; y++)
        for (int x = 0; x < n; x++)
        {
            var v = corr[y, x];
            model.Annotations.Add(new TextAnnotation
            {
                Text = v.ToString("0.00"),
                TextPosition = new DataPoint(x + 0.5, y + 0.5),
                FontSize = 8,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                TextVerticalAlignment = VerticalAlignment.Middle,
                StrokeThickness = 0,
                TextColor = Math.Abs(v) > 0.5 ? OxyColors.White : OxyColors.Black
            });
        }

        Export(model, path);

        static double Pearson(IEnumerable<double> xs, IEnumerable<double> ys)
        {
            var xArr = xs.ToArray();
            var yArr = ys.ToArray();
            if (xArr.Length == 0) return 0;

            var meanX = xArr.Average();
            var meanY = yArr.Average();

            double num = 0, varX = 0, varY = 0;
            for (var i = 0; i < xArr.Length; i++)
            {
                var dx = xArr[i] - meanX;
                var dy = yArr[i] - meanY;
                num += dx * dy;
                varX += dx * dx;
                varY += dy * dy;
            }

            return num / Math.Sqrt(varX * varY);
        }
    }

    public static void PlotBoxplot(OuladContext dbContext, string path)
    {
        Log.Information("Generating box-plot: {Path}", path);

        var model = new PlotModel
        {
            Title = "Studied credits by age band",
            Background = OxyColors.White
        };

        model.Legends.Add(new Legend
        {
            LegendPosition = LegendPosition.TopRight,
            LegendFontSize = 10,
            LegendBorderThickness = 0,
            LegendSymbolLength = 20
        });

        var categoryAxis = new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Age band"
        };
        var valueAxis = new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Studied credits"
        };
        model.Axes.Add(categoryAxis);
        model.Axes.Add(valueAxis);

        var ageBands = Enum.GetValues<AgeBand>();
        var palette = OxyPalettes.HueDistinct(ageBands.Length);
        var bandIdx = 0;

        foreach (var ageBand in ageBands)
        {
            var credits = dbContext.StudentInfos
                .Where(s => s.AgeBand == ageBand)
                .Select(s => (double)s.StudiedCredits)
                .OrderBy(v => v)
                .ToList();

            categoryAxis.Labels.Add(ageBand.ToString());

            if (credits.Count == 0)
            {
                bandIdx++;
                continue;
            }

            var q1 = Quantile(credits, 0.25);
            var median = Quantile(credits, 0.50);
            var q3 = Quantile(credits, 0.75);
            var iqr = q3 - q1;

            var lower = credits.First(v => v >= q1 - 1.5 * iqr);
            var upper = credits.Last(v => v <= q3 + 1.5 * iqr);

            var series = new BoxPlotSeries
            {
                Title = ageBand.ToString(),
                Fill = palette.Colors[bandIdx],
                Stroke = OxyColors.Black,
                StrokeThickness = 1
            };

            var item = new BoxPlotItem(bandIdx, lower, q1, median, q3, upper);
            foreach (var v in credits.Where(v => v < lower || v > upper))
                item.Outliers.Add(v);

            series.Items.Add(item);
            model.Series.Add(series);

            bandIdx++;
        }

        Export(model, path);

        static double Quantile(IReadOnlyList<double> ordered, double p)
        {
            var idx = (ordered.Count - 1) * p;
            var lo = (int)Math.Floor(idx);
            var hi = (int)Math.Ceiling(idx);
            return lo == hi
                ? ordered[lo]
                : ordered[lo] + (ordered[hi] - ordered[lo]) * (idx - lo);
        }
    }

    public static void PlotNormalDistribution(OuladContext ctx, string path)
    {
        Log.Information("Generating normal distribution: {Path}", path);

        var values = ctx.StudentInfos
            .Select(s => (double)s.StudiedCredits)
            .Where(v => v > 0)
            .ToList();

        if (values.Count == 0) return;

        var iqr = Percentile(values, 0.75) - Percentile(values, 0.25);
        var binW = 2 * iqr / Math.Pow(values.Count, 1.0 / 3);
        var bins = Math.Max(10, (int)Math.Round((values.Max() - values.Min()) / binW));

        var histItems = new List<HistogramItem>(bins);
        var min = values.Min();
        var max = values.Max();
        var binWidth = (max - min) / bins;

        for (int i = 0; i < bins; i++)
        {
            var lower = min + i * binWidth;
            var upper = lower + binWidth;
            int count = values.Count(v => v >= lower && (i == bins - 1 ? v <= upper : v < upper));
            double area = count;

            histItems.Add(new HistogramItem(lower, upper, area, count));
        }

        var hist = new HistogramSeries
        {
            Title = "Observed",
            FillColor = OxyColor.FromAColor(140, OxyColors.SteelBlue),
            StrokeThickness = 0
        };
        foreach (var h in histItems) hist.Items.Add(h);

        var mean = values.Average();
        var std = Math.Sqrt(values.Sum(v => (v - mean) * (v - mean)) / values.Count);

        var norm = new LineSeries { Title = "Normal PDF", Color = OxyColors.DarkOrange };

        var scale = values.Count * histItems[0].Width;

        var xMin = histItems[0].RangeStart;
        var xMax = histItems[^1].RangeEnd;

        const int curveSteps = 200;
        for (int i = 0; i <= curveSteps; i++)
        {
            var x = xMin + (xMax - xMin) * i / curveSteps;
            var y = NormalPdf(x, mean, std) * scale;
            norm.Points.Add(new DataPoint(x, y));
        }

        var model = new PlotModel
        {
            Title = "Studied-credits distribution",
            Background = OxyColors.White
        };

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Studied credits"
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Number of students"
        });

        model.Series.Add(hist);
        model.Series.Add(norm);

        AddVerticalLine(mean, model, "μ");
        AddVerticalLine(mean + std, model, "μ + σ");
        AddVerticalLine(mean - std, model, "μ – σ");

        Export(model, path);

        static double NormalPdf(double x, double mu, double sigma) =>
            1.0 / (Math.Sqrt(2 * Math.PI) * sigma) *
            Math.Exp(-(x - mu) * (x - mu) / (2 * sigma * sigma));

        static double Percentile(IReadOnlyList<double> ordered, double p)
        {
            var sorted = ordered.OrderBy(v => v).ToList();
            var idx = (sorted.Count - 1) * p;
            var lo = (int)Math.Floor(idx);
            var hi = (int)Math.Ceiling(idx);
            return lo == hi
                ? sorted[lo]
                : sorted[lo] + (sorted[hi] - sorted[lo]) * (idx - lo);
        }

        static void AddVerticalLine(double x, PlotModel model, string text)
        {
            model.Annotations.Add(new LineAnnotation
            {
                Type = LineAnnotationType.Vertical,
                X = x,
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Dash,
                Text = text,
                TextVerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 8
            });
        }
    }

    public static void PlotScatter(OuladContext ctx, string path)
    {
        Log.Information("Generating scatter plot: {Path}", path);

        var rng = new Random(42);
        var points = new List<ScatterPoint>();
        foreach (var s in ctx.StudentInfos)
        {
            var x = s.NumOfPrevAttempts + (rng.NextDouble() - 0.5) * 0.30;
            var y = s.StudiedCredits;
            points.Add(new ScatterPoint(x, y));
        }

        var scatter = new ScatterSeries
        {
            Title = "Students",
            MarkerType = MarkerType.Circle,
            MarkerSize = 3,
            MarkerFill = OxyColor.FromAColor(0x99, OxyColors.ForestGreen)
        };
        scatter.Points.AddRange(points);

        var trend = new LineSeries
        {
            Title = "Trend",
            Color = OxyColors.DarkSlateGray,
            StrokeThickness = 1
        };
        if (points.Count > 1)
        {
            var xs = points.Select(p => p.X).ToArray();
            var ys = points.Select(p => p.Y).ToArray();
            double mx = xs.Average(), my = ys.Average();
            double slope = xs.Zip(ys, (x, y) => (x - mx) * (y - my)).Sum()
                           / xs.Sum(x => (x - mx) * (x - mx));
            double b = my - slope * mx;
            trend.Points.Add(new DataPoint(xs.Min(), b + slope * xs.Min()));
            trend.Points.Add(new DataPoint(xs.Max(), b + slope * xs.Max()));
        }

        var model = new PlotModel
        {
            Title = "Attempts vs. Studied credits",
            Background = OxyColors.White
        };

        model.Legends.Add(new Legend
        {
            LegendPosition = LegendPosition.TopRight,
            LegendBorderThickness = 0
        });

        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Attempts",
            MajorStep = 1,
            Minimum = -0.5,
            MinorTickSize = 0,
            MajorGridlineStyle = LineStyle.Dot
        });

        var yMax = Math.Min(points.Max(p => p.Y) * 1.05, 650);
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Left,
            Title = "Credits",
            Maximum = yMax,
            MajorGridlineStyle = LineStyle.Dot
        });

        model.Series.Add(scatter);
        if (trend.Points.Count == 2) model.Series.Add(trend);

        Export(model, path);
    }
}