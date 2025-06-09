using OuladEtlEda.DataImport.Readers;
using OuladEtlEda.Domain;
using OuladEtlEda.Domain.Validators;
using OuladEtlEda.DataAccess;
using OuladEtlEda.Infrastructure;

namespace OuladEtlEda.Pipeline;

public class EtlPipeline
{
    private readonly OuladContext _context;
    private readonly BulkLoader _loader;
    private readonly CategoricalOrdinalMapper _mapper;
    private readonly CsvCourseReader _courseReader;
    private readonly CsvAssessmentReader _assessmentReader;
    private readonly CsvStudentInfoReader _studentInfoReader;
    private readonly CsvStudentRegistrationReader _registrationReader;
    private readonly CsvStudentAssessmentReader _studentAssessmentReader;
    private readonly CsvVleReader _vleReader;
    private readonly CsvStudentVleReader _studentVleReader;

    private readonly CourseValidator _courseValidator;
    private readonly AssessmentValidator _assessmentValidator;
    private readonly StudentInfoValidator _studentInfoValidator;
    private readonly StudentRegistrationValidator _registrationValidator;
    private readonly StudentAssessmentValidator _studentAssessmentValidator;
    private readonly VleValidator _vleValidator;
    private readonly StudentVleValidator _studentVleValidator;

    public EtlPipeline(
        CsvCourseReader courseReader,
        CsvAssessmentReader assessmentReader,
        CsvStudentInfoReader studentInfoReader,
        CsvStudentRegistrationReader registrationReader,
        CsvStudentAssessmentReader studentAssessmentReader,
        CsvVleReader vleReader,
        CsvStudentVleReader studentVleReader,
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

    public async Task RunAsync()
    {
        try
        {
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

    private async Task LoadCoursesAsync()
    {
        var entities = new List<Course>();
        await foreach (var csv in _courseReader.ReadAsync())
        {
            var entity = new Course
            {
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                ModulePresentationLength = csv.ModulePresentationLength
            };
            await _courseValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }

    private async Task LoadAssessmentsAsync()
    {
        var entities = new List<Assessment>();
        await foreach (var csv in _assessmentReader.ReadAsync())
        {
            var entity = new Assessment
            {
                IdAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                AssessmentType = csv.AssessmentType,
                Date = csv.Date,
                Weight = csv.Weight
            };
            await _assessmentValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }

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

    private async Task LoadStudentInfoAsync()
    {
        var entities = new List<StudentInfo>();
        await foreach (var csv in _studentInfoReader.ReadAsync())
        {
            var entity = new StudentInfo
            {
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                IdStudent = csv.IdStudent,
                Gender = ParseGender(csv.Gender),
                Region = csv.Region,
                HighestEducation = ParseEducation(csv.HighestEducation),
                ImdBand = csv.ImdBand,
                AgeBand = ParseAgeBand(csv.AgeBand),
                NumOfPrevAttempts = csv.NumOfPrevAttempts,
                StudiedCredits = csv.StudiedCredits,
                Disability = ParseDisability(csv.Disability),
                FinalResult = ParseFinalResult(csv.FinalResult)
            };
            await _studentInfoValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }

    private async Task LoadRegistrationsAsync()
    {
        var entities = new List<StudentRegistration>();
        await foreach (var csv in _registrationReader.ReadAsync())
        {
            var entity = new StudentRegistration
            {
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                IdStudent = csv.IdStudent,
                DateRegistration = csv.DateRegistration,
                DateUnregistration = csv.DateUnregistration
            };
            await _registrationValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }

    private async Task LoadStudentAssessmentsAsync()
    {
        var entities = new List<StudentAssessment>();
        await foreach (var csv in _studentAssessmentReader.ReadAsync())
        {
            var entity = new StudentAssessment
            {
                IdAssessment = _mapper.GetOrAdd("assessment_id", csv.IdAssessment.ToString()),
                IdStudent = csv.IdStudent,
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                DateSubmitted = csv.DateSubmitted,
                IsBanked = csv.IsBanked,
                Score = csv.Score
            };
            await _studentAssessmentValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }

    private async Task LoadVleAsync()
    {
        var entities = new List<Vle>();
        await foreach (var csv in _vleReader.ReadAsync())
        {
            var entity = new Vle
            {
                IdSite = csv.IdSite,
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                ActivityType = csv.ActivityType,
                WeekFrom = csv.WeekFrom,
                WeekTo = csv.WeekTo
            };
            await _vleValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }

    private async Task LoadStudentVleAsync()
    {
        var entities = new List<StudentVle>();
        await foreach (var csv in _studentVleReader.ReadAsync())
        {
            var entity = new StudentVle
            {
                IdSite = csv.IdSite,
                IdStudent = csv.IdStudent,
                CodeModule = csv.CodeModule,
                CodePresentation = csv.CodePresentation,
                Date = csv.Date,
                SumClick = csv.SumClick
            };
            await _studentVleValidator.ValidateAsync(entity);
            entities.Add(entity);
        }
        await _loader.BulkInsertAsync(_context, entities);
    }
}
