using System.Linq;
using OuladEtlEda.DataAccess;

namespace OuladEtlEda.Eda;

public static class BasicEda
{
    public static void Run(OuladContext context)
    {
        var studentCount = context.StudentInfos.Count();
        var courseCount = context.Courses.Count();
        Console.WriteLine($"Students: {studentCount}");
        Console.WriteLine($"Courses: {courseCount}");

        var results = context.StudentInfos
            .GroupBy(s => s.FinalResult)
            .Select(g => new { Result = g.Key, Count = g.Count() })
            .ToList();
        Console.WriteLine("\nFinal results distribution:");
        foreach (var r in results)
            Console.WriteLine($"{r.Result}: {r.Count}");
    }
}
