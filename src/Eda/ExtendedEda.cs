using MathNet.Numerics.Distributions;
using OuladEtlEda.DataAccess;
using OuladEtlEda.Domain;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using Serilog;
using static OuladEtlEda.Eda.EdaUtils;

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
        PlotBarChart(context, Path.Combine("plots", "barchart.png"));
        PlotNormalDistribution(context, Path.Combine("plots", "normal.png"));
        PlotScatter(context, Path.Combine("plots", "scatter.png"));

        File.WriteAllText(Path.Combine("plots", "ttest.txt"), PerformTTest(context));
        File.WriteAllText(Path.Combine("plots", "anova.txt"), PerformAnova(context));

        Log.Information("Extended EDA completed");
        OpenDirectory("plots");
    }

    public static void PlotConfusionMatrix(OuladContext ctx, string path)
    {
        Log.Information("Generating confusion matrix: {Path}", path);

        var genders = Enum.GetNames<Gender>();
        var results = Enum.GetNames<FinalResult>();
        var counts = new double[genders.Length, results.Length];

        foreach (var s in ctx.StudentInfos)
            counts[(int)s.Gender, (int)s.FinalResult]++;

        var palette = OxyPalettes.Viridis(200);

        var model = new PlotModel
        {
            Title = "Final-result distribution by gender",
            Background = OxyColors.White
        };

        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Gender",
            ItemsSource = genders
        });
        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Final result",
            ItemsSource = results
        });
        model.Axes.Add(new LinearColorAxis
        {
            Position = AxisPosition.Right,
            Title = "Number of students",
            Minimum = counts.Cast<double>().Min(),
            Maximum = counts.Cast<double>().Max(),
            AxisDistance = 8,
            AxislineThickness = 20,
            Palette = palette
        });

        model.Series.Add(new HeatMapSeries
        {
            X0 = 0,
            X1 = results.Length,
            Y0 = 0,
            Y1 = genders.Length,
            Data = counts,
            Interpolate = false,
            RenderMethod = HeatMapRenderMethod.Rectangles
        });

        var max = counts.Cast<double>().Max();
        for (var g = 0; g < genders.Length; g++)
        for (var r = 0; r < results.Length; r++)
        {
            var v = counts[g, r];
            model.Annotations.Add(new TextAnnotation
            {
                Text = v.ToString("N0"),
                TextPosition = new DataPoint(r + 0.5, g + 0.5),
                TextHorizontalAlignment = HorizontalAlignment.Center,
                TextVerticalAlignment = VerticalAlignment.Middle,
                FontSize = 8,
                StrokeThickness = 0,
                TextColor = v > max * 0.5 ? OxyColors.White : OxyColors.Black
            });
        }

        ExportPlot(model, path);
    }

    public static void PlotCorrelationMatrix(OuladContext ctx, string path)
    {
        Log.Information("Generating correlation matrix: {Path}", path);

        var rows = ctx.StudentInfos.Select(s => new[]
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
            var i1 = i;
            var j1 = j;
            corr[i, j] = Pearson(
                rows.Select(r => r[i1]),
                rows.Select(r => r[j1]));
        }

        var palette = OxyPalette.Interpolate(
            201, OxyColors.DarkBlue, OxyColors.White, OxyColors.DarkRed);

        var model = new PlotModel
        {
            Title = "Correlation Matrix",
            Background = OxyColors.White
        };

        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Left,
            ItemsSource = labels
        });
        model.Axes.Add(new CategoryAxis
        {
            Position = AxisPosition.Bottom,
            ItemsSource = labels
        });
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

        for (var y = 0; y < n; y++)
        for (var x = 0; x < n; x++)
        {
            var v = corr[y, x];
            model.Annotations.Add(new TextAnnotation
            {
                Text = v.ToString("0.00"),
                TextPosition = new DataPoint(x + 0.5, y + 0.5),
                FontSize = 8,
                StrokeThickness = 0,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                TextVerticalAlignment = VerticalAlignment.Middle,
                TextColor = Math.Abs(v) > 0.5 ? OxyColors.White : OxyColors.Black
            });
        }

        ExportPlot(model, path);
    }

    public static void PlotBoxplot(OuladContext db, string path)
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
            LegendBorderThickness = 0,
            LegendFontSize = 10
        });

        var catAxis = new CategoryAxis { Position = AxisPosition.Bottom, Title = "Age band" };
        var valAxis = new LinearAxis { Position = AxisPosition.Left, Title = "Studied credits" };
        model.Axes.Add(catAxis);
        model.Axes.Add(valAxis);

        var bands = Enum.GetValues<AgeBand>();
        var palette = OxyPalettes.HueDistinct(bands.Length);

        var x = 0;
        foreach (var band in bands)
        {
            var credits = db.StudentInfos
                .Where(s => s.AgeBand == band)
                .Select(s => (double)s.StudiedCredits)
                .OrderBy(v => v).ToList();

            catAxis.Labels.Add(band.ToString());
            if (credits.Count == 0)
            {
                x++;
                continue;
            }

            var q1 = Quantile(credits, 0.25);
            var q2 = Quantile(credits, 0.50);
            var q3 = Quantile(credits, 0.75);
            var iqr = q3 - q1;
            var lower = credits.First(v => v >= q1 - 1.5 * iqr);
            var upper = credits.Last(v => v <= q3 + 1.5 * iqr);

            var series = new BoxPlotSeries
            {
                Title = band.ToString(),
                Fill = palette.Colors[x],
                Stroke = OxyColors.Black,
                StrokeThickness = 1
            };

            var item = new BoxPlotItem(x, lower, q1, q2, q3, upper);
            foreach (var v in credits.Where(v => v < lower || v > upper))
                item.Outliers.Add(v);

            series.Items.Add(item);
            model.Series.Add(series);
            x++;
        }

        ExportPlot(model, path);
    }

    public static void PlotNormalDistribution(OuladContext ctx, string path)
    {
        Log.Information("Generating normal distribution: {Path}", path);

        var values = ctx.StudentInfos
            .Select(s => (double)s.StudiedCredits)
            .Where(v => v > 0).ToList();

        if (values.Count == 0) return;

        var iqr = Percentile(values, 0.75) - Percentile(values, 0.25);
        var binW = iqr == 0 ? 1 : 2 * iqr / Math.Pow(values.Count, 1.0 / 3);
        var bins = Math.Max(10, (int)Math.Round((values.Max() - values.Min()) / binW));

        var histItems = new List<HistogramItem>(bins);
        double min = values.Min(), max = values.Max(), width = (max - min) / bins;

        for (var i = 0; i < bins; i++)
        {
            double lo = min + i * width, up = lo + width;
            var count = values.Count(v => v >= lo && (i == bins - 1 ? v <= up : v < up));
            histItems.Add(new HistogramItem(lo, up, count, count));
        }

        var hist = new HistogramSeries
        {
            Title = "Observed",
            FillColor = OxyColor.FromAColor(140, OxyColors.SteelBlue),
            StrokeThickness = 0
        };
        hist.Items.AddRange(histItems);

        var mean = values.Average();
        var std = Math.Sqrt(values.Sum(v => (v - mean) * (v - mean)) / values.Count);

        var norm = new LineSeries
        {
            Title = "Normal PDF",
            Color = OxyColors.DarkOrange
        };

        var scale = values.Count * width;
        double xMin = histItems[0].RangeStart, xMax = histItems[^1].RangeEnd;
        for (var i = 0; i <= 200; i++)
        {
            var x = xMin + (xMax - xMin) * i / 200;
            var y = NormalPdf(x, mean, std) * scale;
            norm.Points.Add(new DataPoint(x, y));
        }

        var model = new PlotModel
        {
            Title = "Studied-credits distribution",
            Background = OxyColors.White
        };
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Studied credits" });
        model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Number of students" });

        model.Series.Add(hist);
        model.Series.Add(norm);

        model.AddVerticalLine(mean, "μ");
        model.AddVerticalLine(mean + std, "μ + σ");
        model.AddVerticalLine(mean - std, "μ – σ");

        ExportPlot(model, path);
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
            var slope = xs.Zip(ys, (x, y) => (x - mx) * (y - my)).Sum()
                        / xs.Sum(x => (x - mx) * (x - mx));
            var b = my - slope * mx;
            trend.Points.Add(new DataPoint(xs.Min(), b + slope * xs.Min()));
            trend.Points.Add(new DataPoint(xs.Max(), b + slope * xs.Max()));
        }

        var model = new PlotModel
        {
            Title = "Attempts vs. Studied credits",
            Background = OxyColors.White
        };
        model.Legends.Add(new Legend { LegendPosition = LegendPosition.TopRight, LegendBorderThickness = 0 });

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

        ExportPlot(model, path);
    }

    public static void PlotBarChart(OuladContext ctx, string path)
    {
        Log.Information("Generating bar chart: {Path}", path);

        var ageBands = Enum.GetValues<AgeBand>();
        var finalResults = Enum.GetValues<FinalResult>();

        var model = new PlotModel
        {
            Title = "Final result by age band",
            Background = OxyColors.White
        };
        model.Legends.Add(new Legend
        {
            LegendPosition = LegendPosition.TopRight,
            LegendBorderThickness = 0
        });

        var catAxis = new CategoryAxis
        {
            Position = AxisPosition.Left,
            Title = "Age band",
            ItemsSource = ageBands.Select(b => b.ToString()).ToList()
        };
        model.Axes.Add(catAxis);
        model.Axes.Add(new LinearAxis
        {
            Position = AxisPosition.Bottom,
            Title = "Number of students",
            MinimumPadding = 0,
            AbsoluteMinimum = 0
        });

        var palette = OxyPalettes.HueDistinct(finalResults.Length);

        for (var i = 0; i < finalResults.Length; i++)
        {
            var result = finalResults[i];
            var series = new BarSeries
            {
                Title = result.ToString(),
                FillColor = palette.Colors[i],
                StrokeThickness = 0,
                BarWidth = 0.8
            };

            foreach (var band in ageBands)
            {
                var count = ctx.StudentInfos.Count(s => s.AgeBand == band && s.FinalResult == result);
                series.Items.Add(new BarItem(count));
            }

            model.Series.Add(series);
        }

        ExportPlot(model, path);
    }

    public static string PerformTTest(OuladContext ctx)
    {
        var males = ctx.StudentInfos.Where(s => s.Gender == Gender.Male)
            .Select(s => (double)s.StudiedCredits).ToArray();
        var females = ctx.StudentInfos.Where(s => s.Gender == Gender.Female)
            .Select(s => (double)s.StudiedCredits).ToArray();

        if (males.Length == 0 || females.Length == 0)
            return "Insufficient data for t-test.";

        // sample means & variances
        var mM = males.Average();
        var mF = females.Average();
        var vM = males.Select(v => (v - mM) * (v - mM)).Sum() / (males.Length - 1);
        var vF = females.Select(v => (v - mF) * (v - mF)).Sum() / (females.Length - 1);

        // Welch standard error
        var se = Math.Sqrt(vM / males.Length + vF / females.Length);
        if (se == 0) return "No variance in at least one group.";

        // Welch degrees of freedom
        var dfNum = Math.Pow(vM / males.Length + vF / females.Length, 2);
        var dfDen = Math.Pow(vM / males.Length, 2) / (males.Length - 1) +
                    Math.Pow(vF / females.Length, 2) / (females.Length - 1);
        var df = dfNum / dfDen;

        var t = (mM - mF) / se;
        var p = 2 * (1 - StudentT.CDF(0, 1, df, Math.Abs(t))); // two-tailed

        // Cohen's d (pooled SD, classic version)
        var sp = Math.Sqrt(((males.Length - 1) * vM + (females.Length - 1) * vF) /
                           (males.Length + females.Length - 2));
        var d = (mM - mF) / sp;

        return $"Welch t({df:F1}) = {t:F3}, p = {p:E2}, Cohen d = {d:F2}";
    }

    public static string PerformAnova(OuladContext ctx)
    {
        var ageGroups = Enum.GetValues<AgeBand>()
            .Select(b => ctx.StudentInfos.Where(s => s.AgeBand == b)
                .Select(s => (double)s.StudiedCredits).ToArray())
            .Where(g => g.Length > 0)
            .ToArray();

        if (ageGroups.Length < 2)
            return "Insufficient data for ANOVA.";

        var grandMean = ageGroups.SelectMany(g => g).Average();

        double ssBetween = 0, ssWithin = 0;
        foreach (var g in ageGroups)
        {
            var mean = g.Average();
            ssBetween += g.Length * Math.Pow(mean - grandMean, 2);
            ssWithin += g.Select(v => Math.Pow(v - mean, 2)).Sum();
        }

        var dfB = ageGroups.Length - 1;
        var dfW = ageGroups.Sum(g => g.Length) - ageGroups.Length;
        if (dfW <= 0) return "Insufficient within-group df.";

        var msBetween = ssBetween / dfB;
        var msWithin = ssWithin / dfW;
        var f = msBetween / msWithin;

        var p = 1 - FisherSnedecor.CDF(dfB, dfW, f);

        return $"F({dfB}, {dfW}) = {f:F3}, p = {p:E2}";
    }
}