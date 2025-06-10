using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.SkiaSharp;
using Serilog;
using System.Diagnostics;

namespace OuladEtlEda.Eda;

public static class EdaUtils
{
    public static void ExportPlot(PlotModel model, string path)
    {
        using var stream = File.Create(path);
        new PngExporter { Width = 600, Height = 400 }.Export(model, stream);
    }

    public static double Quantile(IList<double> sorted, double p)
    {
        var pos = (sorted.Count - 1) * p;
        int lo = (int)Math.Floor(pos), hi = (int)Math.Ceiling(pos);
        return lo == hi
            ? sorted[lo]
            : sorted[lo] + (sorted[hi] - sorted[lo]) * (pos - lo);
    }

    public static double Percentile(IReadOnlyList<double> list, double p)
    {
        var sorted = list.OrderBy(v => v).ToList();
        return Quantile(sorted, p);
    }

    public static double NormalPdf(double x, double m, double s)
    {
        return 1.0 / (Math.Sqrt(2 * Math.PI) * s) *
               Math.Exp(-Math.Pow(x - m, 2) / (2 * s * s));
    }

    public static double Pearson(IEnumerable<double> xs, IEnumerable<double> ys)
    {
        var x = xs.ToArray();
        var y = ys.ToArray();
        if (x.Length == 0) return 0;
        double meanX = x.Average(), meanY = y.Average();
        double cov = 0, varX = 0, varY = 0;
        for (var i = 0; i < x.Length; i++)
        {
            var dx = x[i] - meanX;
            var dy = y[i] - meanY;
            cov += dx * dy;
            varX += dx * dx;
            varY += dy * dy;
        }

        return cov / Math.Sqrt(varX * varY);
    }

    public static double Mean(IEnumerable<double> values)
    {
        var data = values as double[] ?? values.ToArray();
        return data.Length == 0 ? 0 : data.Average();
    }

    public static double StandardDeviation(IEnumerable<double> values)
    {
        var data = values as double[] ?? values.ToArray();
        if (data.Length == 0) return 0;
        var mean = data.Average();
        var variance = data.Sum(v => (v - mean) * (v - mean)) / data.Length;
        return Math.Sqrt(variance);
    }

    public static double Kurtosis(IEnumerable<double> values)
    {
        var data = values as double[] ?? values.ToArray();
        if (data.Length < 2) return 0;
        var mean = data.Average();
        double m2 = 0, m4 = 0;
        foreach (var v in data)
        {
            var d = v - mean;
            m2 += d * d;
            m4 += d * d * d * d;
        }
        m2 /= data.Length;
        m4 /= data.Length;
        return m4 / (m2 * m2) - 3.0;
    }

    public static void AddVerticalLine(this PlotModel model, double x, string text)
    {
        model.Annotations.Add(new LineAnnotation
        {
            Type = LineAnnotationType.Vertical,
            X = x,
            LineStyle = LineStyle.Dash,
            Color = OxyColors.Gray,
            Text = text,
            TextVerticalAlignment = VerticalAlignment.Bottom,
            FontSize = 8
        });
    }

    public static void OpenDirectory(string path)
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                Process.Start(new ProcessStartInfo("explorer.exe", path) { UseShellExecute = true });
            }
            else if (OperatingSystem.IsMacOS())
            {
                Process.Start("open", path);
            }
            else if (OperatingSystem.IsLinux())
            {
                Process.Start("xdg-open", path);
            }
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Could not open directory {Path}", path);
        }
    }
}