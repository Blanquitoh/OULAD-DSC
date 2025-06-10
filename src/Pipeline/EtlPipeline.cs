using Microsoft.EntityFrameworkCore;
using OuladEtlEda.DataAccess;
using OuladEtlEda.DataImport;
using OuladEtlEda.DataImport.Models;
using OuladEtlEda.DataImport.Readers;
using OuladEtlEda.Domain;
using OuladEtlEda.Domain.Validators;
using OuladEtlEda.Infrastructure;
using OuladEtlEda.Pipeline.Mappers;
using Serilog;

namespace OuladEtlEda.Pipeline;

public class EtlPipeline(
    CsvReader<CourseCsv> courseReader,
    CsvReader<AssessmentCsv> assessmentReader,
    CsvReader<StudentInfoCsv> studentInfoReader,
    CsvReader<StudentRegistrationCsv> registrationReader,
    CsvReader<StudentAssessmentCsv> studentAssessmentReader,
    CsvReader<VleCsv> vleReader,
    CsvReader<StudentVleCsv> studentVleReader,
    CategoricalOrdinalMapper mapper,
    CourseValidator courseValidator,
    AssessmentValidator assessmentValidator,
    StudentInfoValidator studentInfoValidator,
    StudentRegistrationValidator registrationValidator,
    StudentAssessmentValidator studentAssessmentValidator,
    VleValidator vleValidator,
    StudentVleValidator studentVleValidator,
    BulkLoader loader,
    OuladContext context)
{
    private readonly AssessmentCsvMapper _assessmentMapper = new(mapper);

    private readonly CourseCsvMapper _courseMapper = new();

    private readonly StudentRegistrationCsvMapper _registrationMapper = new();
    private readonly StudentAssessmentCsvMapper _studentAssessmentMapper = new(mapper, context);
    private readonly StudentInfoCsvMapper _studentInfoMapper = new(mapper);
    private readonly StudentVleCsvMapper _studentVleMapper = new();
    private readonly VleCsvMapper _vleMapper = new(mapper);

    private async Task LoadAsync<TCsv, TEntity>(
        CsvReaderBase<TCsv> reader,
        Func<TCsv, TEntity> map,
        IDomainValidator<TEntity> validator,
        int logInterval = 1000) where TEntity : class
    {
        var entities = new List<TEntity>();
        var count = 0;
        await foreach (var csv in reader.ReadAsync())
        {
            var entity = map(csv);
            if (entity == null)
            {
                Log.Warning("Skipping record for {Entity} due to mapping failure", typeof(TEntity).Name);
                continue;
            }
            await validator.ValidateAsync(entity);
            entities.Add(entity);
            count++;
            if (count % logInterval == 0)
                Log.Information("Processed {Count} records for {Entity}", count, typeof(TEntity).Name);
        }

        await loader.BulkInsertAsync(context, entities);
        Log.Information("Inserted {Count} records for {Entity}", count, typeof(TEntity).Name);
    }

    public async Task RunAsync()
    {
        try
        {
            await TruncateTablesAsync();
            await LoadCoursesAsync();
            await LoadAssessmentsAsync();
            await LoadStudentInfoAsync();
            await LoadRegistrationsAsync();
            await LoadStudentAssessmentsAsync();
            await LoadVleAsync();
            await LoadStudentVleAsync();
        }
        catch (DomainException ex)
        {
            throw new EtlException("Domain validation failed", ex);
        }
    }

    private async Task TruncateTablesAsync()
    {
        if (!context.Database.IsRelational())
        {
            context.StudentVles.RemoveRange(context.StudentVles);
            context.StudentAssessments.RemoveRange(context.StudentAssessments);
            context.StudentRegistrations.RemoveRange(context.StudentRegistrations);
            context.Vles.RemoveRange(context.Vles);
            context.StudentInfos.RemoveRange(context.StudentInfos);
            context.Assessments.RemoveRange(context.Assessments);
            context.Courses.RemoveRange(context.Courses);
            await context.SaveChangesAsync();
            return;
        }

        await _context.StudentVles.ExecuteDeleteAsync();
        await _context.StudentAssessments.ExecuteDeleteAsync();
        await _context.StudentRegistrations.ExecuteDeleteAsync();
        await _context.Vles.ExecuteDeleteAsync();
        await _context.StudentInfos.ExecuteDeleteAsync();
        await _context.Assessments.ExecuteDeleteAsync();
        await _context.Courses.ExecuteDeleteAsync();
    }

    private Task LoadCoursesAsync()
    {
        return LoadAsync(
            courseReader,
            _courseMapper.Map,
            courseValidator);
    }

    private Task LoadAssessmentsAsync()
    {
        return LoadAsync(
            assessmentReader,
            _assessmentMapper.Map,
            assessmentValidator);
    }


    private Task LoadStudentInfoAsync()
    {
        return LoadAsync(
            studentInfoReader,
            _studentInfoMapper.Map,
            studentInfoValidator);
    }

    private Task LoadRegistrationsAsync()
    {
        return LoadAsync(
            registrationReader,
            _registrationMapper.Map,
            registrationValidator);
    }

    private Task LoadStudentAssessmentsAsync()
    {
        return LoadAsync(
            studentAssessmentReader,
            _studentAssessmentMapper.Map,
            studentAssessmentValidator);
    }

    private Task LoadVleAsync()
    {
        return LoadAsync(
            vleReader,
            _vleMapper.Map,
            vleValidator);
    }

    private Task LoadStudentVleAsync()
    {
        return LoadAsync(
            studentVleReader,
            _studentVleMapper.Map,
            studentVleValidator);
    }
}