using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HireMatch.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MakeResumePerCandidateUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_resumes_CandidateProfileId",
                table: "resumes");

            migrationBuilder.CreateIndex(
                name: "IX_resumes_CandidateProfileId",
                table: "resumes",
                column: "CandidateProfileId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_resumes_CandidateProfileId",
                table: "resumes");

            migrationBuilder.CreateIndex(
                name: "IX_resumes_CandidateProfileId",
                table: "resumes",
                column: "CandidateProfileId");
        }
    }
}
