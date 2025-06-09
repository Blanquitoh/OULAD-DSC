using Microsoft.EntityFrameworkCore.Migrations;

namespace OuladEtlEda.Migrations;

/// <inheritdoc />
public partial class AddOrdinalColumns : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int?>(
            name: "AssessmentTypeOrdinal",
            table: "assessments",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int?>(
            name: "RegionOrdinal",
            table: "studentInfo",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int?>(
            name: "ImdBandOrdinal",
            table: "studentInfo",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<int?>(
            name: "ActivityTypeOrdinal",
            table: "vle",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AssessmentTypeOrdinal",
            table: "assessments");

        migrationBuilder.DropColumn(
            name: "RegionOrdinal",
            table: "studentInfo");

        migrationBuilder.DropColumn(
            name: "ImdBandOrdinal",
            table: "studentInfo");

        migrationBuilder.DropColumn(
            name: "ActivityTypeOrdinal",
            table: "vle");
    }
}

