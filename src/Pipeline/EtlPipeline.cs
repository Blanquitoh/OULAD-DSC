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

public class EtlPipeline
{
    private readonly AssessmentCsvMapper _assessmentMapper;
    private readonly CsvReader<AssessmentCsv> _assessmentReader;
    private readonly AssessmentValidator _assessmentValidator;
    private readonly OuladContext _context;

    private readonly CourseCsvMapper _courseMapper;
    private readonly CsvReader<CourseCsv> _courseReader;

    private readonly CourseValidator _courseValidator;
    private readonly BulkLoader _loader;
    private readonly CategoricalOrdinalMapper _mapper;
    private readonly StudentRegistrationCsvMapper _registrationMapper;
    private readonly CsvReader<StudentRegistrationCsv> _registrationReader;
    private readonly StudentRegistrationValidator _registrationValidator;
    private readonly StudentAssessmentCsvMapper _studentAssessmentMapper;
    private readonly CsvReader<StudentAssessmentCsv> _studentAssessmentReader;
    private readonly StudentAssessmentValidator _studentAssessmentValidator;
    private readonly StudentInfoCsvMapper _studentInfoMapper;
    private readonly CsvReader<StudentInfoCsv> _studentInfoReader;
    private readonly StudentInfoValidator _studentInfoValidator;
    private readonly StudentVleCsvMapper _studentVleMapper;
    private readonly CsvReader<StudentVleCsv> _studentVleReader;
    private readonly StudentVleValidator _studentVleValidator;
    private readonly VleCsvMapper _vleMapper;
    private readonly CsvReader<VleCsv> _vleReader;
    private readonly VleValidator _vleValidator;

    public EtlPipeline(
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
        _courseReader = courseReader;
        _assessmentReader = assessmentReader;
        _studentInfoReader = studentInfoReader;
        _registrationReader = registrationReader;
        _studentAssessmentReader = studentAssessmentReader;
        _vleReader = vleReader;
        _studentVleReader = studentVleReader;
        _mapper = mapper;
        _courseValidator = courseValidator;
        _assessmentValidator = assessmentValidator;
        _studentInfoValidator = studentInfoValidator;
        _registrationValidator = registrationValidator;
        _studentAssessmentValidator = studentAssessmentValidator;
        _vleValidator = vleValidator;
        _studentVleValidator = studentVleValidator;
        _loader = loader;
        _context = context;

        _courseMapper = new CourseCsvMapper();
        _assessmentMapper = new AssessmentCsvMapper(mapper);
        _studentInfoMapper = new StudentInfoCsvMapper(mapper);
        _registrationMapper = new StudentRegistrationCsvMapper();
        _studentAssessmentMapper = new StudentAssessmentCsvMapper(mapper, context);
        _vleMapper = new VleCsvMapper(mapper);
        _studentVleMapper = new StudentVleCsvMapper();
    }

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
            await validator.ValidateAsync(entity);
            entities.Add(entity);
            count++;
            if (count % logInterval == 0)
                Log.Information("Processed {Count} records for {Entity}", count, typeof(TEntity).Name);
        }

        await _loader.BulkInsertAsync(_context, entities);
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
        if (!_context.Database.IsRelational())
        {
            _context.StudentVles.RemoveRange(_context.StudentVles);
            _context.StudentAssessments.RemoveRange(_context.StudentAssessments);
            _context.StudentRegistrations.RemoveRange(_context.StudentRegistrations);
            _context.Vles.RemoveRange(_context.Vles);
            _context.StudentInfos.RemoveRange(_context.StudentInfos);
            _context.Assessments.RemoveRange(_context.Assessments);
            _context.Courses.RemoveRange(_context.Courses);
            await _context.SaveChangesAsync();
            return;
        }

        var tables = new[]
        {
            "studentVle",
            "studentAssessment",
            "studentRegistration",
            "vle",
            "studentInfo",
            "assessments",
            "courses"
        };

        foreach (var table in tables)
        {
            await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE [{table}]");
        }
    }

    private Task LoadCoursesAsync()
    {
        return LoadAsync(
            _courseReader,
            _courseMapper.Map,
            _courseValidator);
    }

    private Task LoadAssessmentsAsync()
    {
        return LoadAsync(
            _assessmentReader,
            _assessmentMapper.Map,
            _assessmentValidator);
    }


    private Task LoadStudentInfoAsync()
    {
        return LoadAsync(
            _studentInfoReader,
            _studentInfoMapper.Map,
            _studentInfoValidator);
    }

    private Task LoadRegistrationsAsync()
    {
        return LoadAsync(
            _registrationReader,
            _registrationMapper.Map,
            _registrationValidator);
    }

    private Task LoadStudentAssessmentsAsync()
    {
        return LoadAsync(
            _studentAssessmentReader,
            _studentAssessmentMapper.Map,
            _studentAssessmentValidator);
    }

    private Task LoadVleAsync()
    {
        return LoadAsync(
            _vleReader,
            _vleMapper.Map,
            _vleValidator);
    }

    private Task LoadStudentVleAsync()
    {
        return LoadAsync(
            _studentVleReader,
            _studentVleMapper.Map,
            _studentVleValidator);
    }
}