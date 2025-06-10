using System;
using Microsoft.EntityFrameworkCore;
using OuladEtlEda.DataAccess;
using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Domain;
using OuladEtlEda.Pipeline;
using OuladEtlEda.Pipeline.Mappers;
using Xunit;

namespace OuladEtlEda.Tests.MapperTests;

public class StudentAssessmentCsvMapperTests
{
    [Fact]
    public void Mapper_resolves_module_and_presentation_from_assessment()
    {
        var options = new DbContextOptionsBuilder<OuladContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        using var context = new OuladContext(options);

        var mapper = new CategoricalOrdinalMapper();
        var assessmentId = mapper.GetOrAdd("assessment_id", "1");
        context.Assessments.Add(new Assessment
        {
            IdAssessment = assessmentId,
            CodeModule = "AAA",
            CodePresentation = "2021"
        });
        context.SaveChanges();

        var csvMapper = new StudentAssessmentCsvMapper(mapper, context);
        var csv = new StudentAssessmentCsv
        {
            IdAssessment = 1,
            IdStudent = 7,
            DateSubmitted = 5,
            IsBanked = false,
            Score = 75
        };

        var result = csvMapper.Map(csv)!;

        Assert.Equal("AAA", result.CodeModule);
        Assert.Equal("2021", result.CodePresentation);
    }
}