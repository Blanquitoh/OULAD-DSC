namespace OuladEtlEda.Domain;

public enum Gender
{
    Female = 0,
    Male = 1
}

public enum AgeBand
{
    Under35 = 0,
    From35To55 = 1,
    Over55 = 2
}

public enum Disability
{
    No = 0,
    Yes = 1
}

public enum FinalResult
{
    Withdrawn = 0,
    Fail = 1,
    Pass = 2,
    Distinction = 3
}

public enum EducationLevel
{
    NoFormalQual = 0,
    LowerThanAlevel = 1,
    ALevelOrEquivalent = 2,
    HEQualification = 3,
    PostGraduate = 4
}

public enum AssessmentType
{
    Tma = 0,
    Cma = 1,
    Exam = 2
}
