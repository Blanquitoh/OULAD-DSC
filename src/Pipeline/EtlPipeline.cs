using OuladEtlEda.DataImport.Readers;
using OuladEtlEda.Domain;
using OuladEtlEda.Domain.Validators;
using OuladEtlEda.DataAccess;
using OuladEtlEda.DataImport;
using OuladEtlEda.DataImport.Models;
using OuladEtlEda.Infrastructure;
using Serilog;

namespace OuladEtlEda.Pipeline;

public class EtlPipeline
{
    private readonly OuladContext _context;
    private readonly BulkLoader _loader;
    private readonly CategoricalOrdinalMapper _mapper;
    private readonly CsvReader<CourseCsv> _courseReader;
    private readonly CsvReader<AssessmentCsv> _assessmentReader;
    private readonly CsvReader<StudentInfoCsv> _studentInfoReader;
    private readonly CsvReader<StudentRegistrationCsv> _registrationReader;
    private readonly CsvReader<StudentAssessmentCsv> _studentAssessmentReader;
    private readonly CsvReader<VleCsv> _vleReader;
    private readonly CsvReader<StudentVleCsv> _studentVleReader;

    private readonly CourseValidator _courseValidator;
    private readonly AssessmentValidator _assessmentValidator;
    private readonly StudentInfoValidator _studentInfoValidator;
    private readonly StudentRegistrationValidator _registrationValidator;
    private readonly StudentAssessmentValidator _studentAssessmentValidator;
    private readonly VleValidator _vleValidator;
    private readonly StudentVleValidator _studentVleValidator;

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
            {
                Log.Information("Processed {Count} records for {Entity}", count, typeof(TEntity).Name);
            }
        }

        await _loader.BulkInsertAsync(_context, entities);
        Log.Information("Inserted {Count} records for {Entity}", count, typeof(TEntity).Name);
    }

    public async Task RunAsync()
    {
        try
        {
            var tasks = new[]
            {
                LoadCoursesAsync(),
                LoadAssessmentsAsync(),
                LoadStudentInfoAsync(),
                LoadRegistrationsAsync(),
                LoadStudentAssessmentsAsync(),
                LoadVleAsync(),
                LoadStudentVleAsync()
            };

            await Task.WhenAll(tasks);
        }
        catch (DomainException ex)
        {
            throw new EtlException("Domain validation failed", ex);
        }
    }

    private Task LoadCoursesAsync() =>
        LoadAsync(
            _courseReader,
            csv => new Course
            {
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                ModulePresentationLength = csv.ModulePresentationLength
            },
            _courseValidator);

    private Task LoadAssessmentsAsync() =>
        LoadAsync(
            _assessmentReader,
            csv => new Assessment
            {
                IdAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                AssessmentType = csv.AssessmentType,
                AssessmentTypeOrdinal = csv.AssessmentType == null
                    ? null
                    : _mapper.GetOrAdd("assessment_type", csv.AssessmentType),
                Date = csv.Date,
                Weight = csv.Weight
            },
            _assessmentValidator);

    private static Gender ParseGender(string value) => value.Trim().ToUpper() switch
    {
        "M" => Gender.Male,
        "F" => Gender.Female,
        _ => Enum.TryParse<Gender>(value, true, out var g) ? g : Gender.Female
    };

    private static AgeBand ParseAgeBand(string value) => value.Trim() switch
    {
        "0-35" => AgeBand.Under35,
        "35-55" => AgeBand.From35To55,
        _ => AgeBand.Over55
    };

    private static Disability ParseDisability(string value) => value.Trim().ToUpper() switch
    {
        "N" => Disability.No,
        "Y" => Disability.Yes,
        _ => Disability.No
    };

    private static FinalResult ParseFinalResult(string value) => Enum.TryParse<FinalResult>(value.Replace(" ", ""), true, out var r) ? r : FinalResult.Fail;

    private static EducationLevel ParseEducation(string value)
    {
        var v = value.ToLower();
        if (v.StartsWith("no formal")) return EducationLevel.NoFormalQual;
        if (v.StartsWith("lower")) return EducationLevel.LowerThanAlevel;
        if (v.StartsWith("a level")) return EducationLevel.ALevelOrEquivalent;
        if (v.StartsWith("he")) return EducationLevel.HEQualification;
        if (v.StartsWith("post")) return EducationLevel.PostGraduate;
        return EducationLevel.NoFormalQual;
    }

    private Task LoadStudentInfoAsync() =>
        LoadAsync(
            _studentInfoReader,
            csv => new StudentInfo
            {
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                IdStudent = csv.IdStudent,
                Gender = ParseGender(csv.Gender),
                Region = csv.Region,
                RegionOrdinal = csv.Region == null
                    ? null
                    : _mapper.GetOrAdd("region", csv.Region),
                HighestEducation = ParseEducation(csv.HighestEducation),
                ImdBand = csv.ImdBand,
                ImdBandOrdinal = csv.ImdBand == null
                    ? null
                    : _mapper.GetOrAdd("imd_band", csv.ImdBand),
                AgeBand = ParseAgeBand(csv.AgeBand),
                NumOfPrevAttempts = csv.NumOfPrevAttempts,
                StudiedCredits = csv.StudiedCredits,
                Disability = ParseDisability(csv.Disability),
                FinalResult = ParseFinalResult(csv.FinalResult)
            },
            _studentInfoValidator);

    private Task LoadRegistrationsAsync() =>
        LoadAsync(
            _registrationReader,
            csv => new StudentRegistration
            {
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                IdStudent = csv.IdStudent,
                DateRegistration = csv.DateRegistration,
                DateUnregistration = csv.DateUnregistration
            },
            _registrationValidator);

    private Task LoadStudentAssessmentsAsync() =>
        LoadAsync(
            _studentAssessmentReader,
            csv => new StudentAssessment
            {
                IdAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
                IdStudent = csv.IdStudent,
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                DateSubmitted = csv.DateSubmitted,
                IsBanked = csv.IsBanked,
                Score = csv.Score
            },
            _studentAssessmentValidator);

    private Task LoadVleAsync() =>
        LoadAsync(
            _vleReader,
            csv => new Vle
            {
                IdSite = csv.IdSite,
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                ActivityType = csv.ActivityType,
                ActivityTypeOrdinal = csv.ActivityType == null
                    ? null
                    : _mapper.GetOrAdd("activity_type", csv.ActivityType),
                WeekFrom = csv.WeekFrom,
                WeekTo = csv.WeekTo
            },
            _vleValidator);

    private Task LoadStudentVleAsync() =>
        LoadAsync(
            _studentVleReader,
            csv => new StudentVle
            {
                IdSite = csv.IdSite,
                IdStudent = csv.IdStudent,
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                Date = csv.Date,
                SumClick = csv.SumClick
            },
            _studentVleValidator);
}
