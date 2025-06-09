using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using OuladEtlEda.DataAccess;
using OuladEtlEda.Domain;
using OuladEtlEda.Eda;
using Xunit;

namespace OuladEtlEda.Tests.EdaTests;

public class ExtendedEdaTests
{
    private static OuladContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<OuladContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        var context = new OuladContext(options);
        context.StudentInfos.AddRange(
            new StudentInfo
            {
                CodeModule = "AAA",
                CodePresentation = "2021",
                IdStudent = 1,
                AgeBand = AgeBand.From18To25,
                StudiedCredits = 60,
                NumOfPrevAttempts = 0,
                Gender = Gender.Male,
                FinalResult = FinalResult.Pass
            },
            new StudentInfo
            {
                CodeModule = "AAA",
                CodePresentation = "2021",
                IdStudent = 2,
                AgeBand = AgeBand.From26To35,
                StudiedCredits = 80,
                NumOfPrevAttempts = 1,
                Gender = Gender.Female,
                FinalResult = FinalResult.Fail
            });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public void Plot_methods_do_not_throw()
    {
        using var context = CreateContext();
        var tmp1 = Path.GetTempFileName();
        var tmp2 = Path.GetTempFileName();
        var tmp3 = Path.GetTempFileName();
        var tmp4 = Path.GetTempFileName();
        var tmp5 = Path.GetTempFileName();

        ExtendedEda.PlotConfusionMatrix(context, tmp1);
        ExtendedEda.PlotCorrelationMatrix(context, tmp2);
        ExtendedEda.PlotBoxplot(context, tmp3);
        ExtendedEda.PlotNormalDistribution(context, tmp4);
        ExtendedEda.PlotScatter(context, tmp5);
    }
}
