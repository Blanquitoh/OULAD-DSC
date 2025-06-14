﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OuladEtlEda.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    ModulePresentationLength = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => new { x.CodeModule, x.CodePresentation });
                });

            migrationBuilder.CreateTable(
                name: "assessments",
                columns: table => new
                {
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    IdAssessment = table.Column<int>(type: "int", nullable: false),
                    AssessmentType = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    AssessmentTypeEnum = table.Column<int>(type: "int", nullable: false),
                    AssessmentTypeOrdinal = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<int>(type: "int", nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessments", x => new { x.CodeModule, x.CodePresentation, x.IdAssessment });
                    table.ForeignKey(
                        name: "FK_assessments_courses_CodeModule_CodePresentation",
                        columns: x => new { x.CodeModule, x.CodePresentation },
                        principalTable: "courses",
                        principalColumns: new[] { "CodeModule", "CodePresentation" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentInfo",
                columns: table => new
                {
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Region = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    RegionOrdinal = table.Column<int>(type: "int", nullable: true),
                    HighestEducation = table.Column<int>(type: "int", nullable: false),
                    ImdBand = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false),
                    ImdBandOrdinal = table.Column<int>(type: "int", nullable: true),
                    AgeBand = table.Column<int>(type: "int", nullable: false),
                    NumOfPrevAttempts = table.Column<int>(type: "int", nullable: false),
                    StudiedCredits = table.Column<int>(type: "int", nullable: false),
                    Disability = table.Column<int>(type: "int", nullable: false),
                    FinalResult = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_studentInfo", x => new { x.CodeModule, x.CodePresentation, x.IdStudent });
                    table.ForeignKey(
                        name: "FK_studentInfo_courses_CodeModule_CodePresentation",
                        columns: x => new { x.CodeModule, x.CodePresentation },
                        principalTable: "courses",
                        principalColumns: new[] { "CodeModule", "CodePresentation" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "vle",
                columns: table => new
                {
                    IdSite = table.Column<int>(type: "int", nullable: false),
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    ActivityType = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    ActivityTypeOrdinal = table.Column<int>(type: "int", nullable: true),
                    WeekFrom = table.Column<int>(type: "int", nullable: true),
                    WeekTo = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vle", x => new { x.IdSite, x.CodeModule, x.CodePresentation });
                    table.ForeignKey(
                        name: "FK_vle_courses_CodeModule_CodePresentation",
                        columns: x => new { x.CodeModule, x.CodePresentation },
                        principalTable: "courses",
                        principalColumns: new[] { "CodeModule", "CodePresentation" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentAssessment",
                columns: table => new
                {
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    IdAssessment = table.Column<int>(type: "int", nullable: false),
                    DateSubmitted = table.Column<int>(type: "int", nullable: true),
                    IsBanked = table.Column<bool>(type: "bit", nullable: false),
                    Score = table.Column<float>(type: "real", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_studentAssessment_assessments_CodeModule_CodePresentation_IdAssessment",
                        columns: x => new { x.CodeModule, x.CodePresentation, x.IdAssessment },
                        principalTable: "assessments",
                        principalColumns: new[] { "CodeModule", "CodePresentation", "IdAssessment" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentAssessment_studentInfo_CodeModule_CodePresentation_IdStudent",
                        columns: x => new { x.CodeModule, x.CodePresentation, x.IdStudent },
                        principalTable: "studentInfo",
                        principalColumns: new[] { "CodeModule", "CodePresentation", "IdStudent" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentRegistration",
                columns: table => new
                {
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    DateRegistration = table.Column<int>(type: "int", nullable: true),
                    DateUnregistration = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_studentRegistration_courses_CodeModule_CodePresentation",
                        columns: x => new { x.CodeModule, x.CodePresentation },
                        principalTable: "courses",
                        principalColumns: new[] { "CodeModule", "CodePresentation" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentRegistration_studentInfo_CodeModule_CodePresentation_IdStudent",
                        columns: x => new { x.CodeModule, x.CodePresentation, x.IdStudent },
                        principalTable: "studentInfo",
                        principalColumns: new[] { "CodeModule", "CodePresentation", "IdStudent" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "studentVle",
                columns: table => new
                {
                    IdSite = table.Column<int>(type: "int", nullable: false),
                    IdStudent = table.Column<int>(type: "int", nullable: false),
                    CodeModule = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    CodePresentation = table.Column<string>(type: "varchar(45)", unicode: false, maxLength: 45, nullable: false),
                    Date = table.Column<int>(type: "int", nullable: true),
                    SumClick = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_studentVle_studentInfo_CodeModule_CodePresentation_IdStudent",
                        columns: x => new { x.CodeModule, x.CodePresentation, x.IdStudent },
                        principalTable: "studentInfo",
                        principalColumns: new[] { "CodeModule", "CodePresentation", "IdStudent" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_studentVle_vle_IdSite_CodeModule_CodePresentation",
                        columns: x => new { x.IdSite, x.CodeModule, x.CodePresentation },
                        principalTable: "vle",
                        principalColumns: new[] { "IdSite", "CodeModule", "CodePresentation" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_studentAssessment_CodeModule_CodePresentation_IdAssessment",
                table: "studentAssessment",
                columns: new[] { "CodeModule", "CodePresentation", "IdAssessment" });

            migrationBuilder.CreateIndex(
                name: "IX_studentAssessment_CodeModule_CodePresentation_IdStudent",
                table: "studentAssessment",
                columns: new[] { "CodeModule", "CodePresentation", "IdStudent" });

            migrationBuilder.CreateIndex(
                name: "IX_studentRegistration_CodeModule_CodePresentation_IdStudent",
                table: "studentRegistration",
                columns: new[] { "CodeModule", "CodePresentation", "IdStudent" });

            migrationBuilder.CreateIndex(
                name: "IX_studentVle_CodeModule_CodePresentation_IdStudent",
                table: "studentVle",
                columns: new[] { "CodeModule", "CodePresentation", "IdStudent" });

            migrationBuilder.CreateIndex(
                name: "IX_studentVle_IdSite_CodeModule_CodePresentation",
                table: "studentVle",
                columns: new[] { "IdSite", "CodeModule", "CodePresentation" });

            migrationBuilder.CreateIndex(
                name: "IX_vle_CodeModule_CodePresentation",
                table: "vle",
                columns: new[] { "CodeModule", "CodePresentation" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "studentAssessment");

            migrationBuilder.DropTable(
                name: "studentRegistration");

            migrationBuilder.DropTable(
                name: "studentVle");

            migrationBuilder.DropTable(
                name: "assessments");

            migrationBuilder.DropTable(
                name: "studentInfo");

            migrationBuilder.DropTable(
                name: "vle");

            migrationBuilder.DropTable(
                name: "courses");
        }
    }
}
