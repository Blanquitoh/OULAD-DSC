using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OuladEtlEda.Migrations
{
    /// <inheritdoc />
    public partial class CreateFullDomainView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE VIEW dbo.FullDomain AS
SELECT
    sa.CodeModule,
    sa.CodePresentation,
    sa.IdStudent,
    sa.IdAssessment AS EventId,
    a.AssessmentType,
    a.AssessmentTypeOrdinal,
    a.Date AS AssessmentDate,
    a.Weight,
    sa.DateSubmitted,
    sa.IsBanked,
    sa.Score,
    NULL AS IdSite,
    NULL AS ActivityType,
    NULL AS ActivityTypeOrdinal,
    NULL AS WeekFrom,
    NULL AS WeekTo,
    NULL AS SumClick,
    'ASSESS' AS DomainType,
    si.Gender,
    si.Region,
    si.RegionOrdinal,
    si.HighestEducation,
    si.ImdBand,
    si.ImdBandOrdinal,
    si.AgeBand,
    si.NumOfPrevAttempts,
    si.StudiedCredits,
    si.Disability,
    si.FinalResult
FROM studentAssessment sa
JOIN assessments a ON sa.IdAssessment = a.IdAssessment
JOIN studentInfo si ON sa.CodeModule = si.CodeModule
    AND sa.CodePresentation = si.CodePresentation
    AND sa.IdStudent = si.IdStudent
UNION ALL
SELECT
    sv.CodeModule,
    sv.CodePresentation,
    sv.IdStudent,
    NULL AS EventId,
    NULL AS AssessmentType,
    NULL AS AssessmentTypeOrdinal,
    NULL AS AssessmentDate,
    NULL AS Weight,
    NULL AS DateSubmitted,
    NULL AS IsBanked,
    NULL AS Score,
    sv.IdSite,
    v.ActivityType,
    v.ActivityTypeOrdinal,
    v.WeekFrom,
    v.WeekTo,
    sv.SumClick,
    'VLE' AS DomainType,
    si.Gender,
    si.Region,
    si.RegionOrdinal,
    si.HighestEducation,
    si.ImdBand,
    si.ImdBandOrdinal,
    si.AgeBand,
    si.NumOfPrevAttempts,
    si.StudiedCredits,
    si.Disability,
    si.FinalResult
FROM studentVle sv
JOIN vle v ON sv.IdSite = v.IdSite
JOIN studentInfo si ON sv.CodeModule = si.CodeModule
    AND sv.CodePresentation = si.CodePresentation
    AND sv.IdStudent = si.IdStudent";
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS dbo.FullDomain;");
        }
    }
}
