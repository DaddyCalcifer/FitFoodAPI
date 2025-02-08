using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFoodAPI.Migrations
{
    /// <inheritdoc />
    public partial class sportFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercise_TrainingPlans_TrainingPlanId",
                table: "Exercise");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseProgress_Exercise_ExerciseId",
                table: "ExerciseProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise");

            migrationBuilder.RenameTable(
                name: "Exercise",
                newName: "Exercises");

            migrationBuilder.RenameIndex(
                name: "IX_Exercise_TrainingPlanId",
                table: "Exercises",
                newName: "IX_Exercises_TrainingPlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseProgress_Exercises_ExerciseId",
                table: "ExerciseProgress",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_TrainingPlans_TrainingPlanId",
                table: "Exercises",
                column: "TrainingPlanId",
                principalTable: "TrainingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseProgress_Exercises_ExerciseId",
                table: "ExerciseProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TrainingPlans_TrainingPlanId",
                table: "Exercises");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Exercises",
                table: "Exercises");

            migrationBuilder.RenameTable(
                name: "Exercises",
                newName: "Exercise");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_TrainingPlanId",
                table: "Exercise",
                newName: "IX_Exercise_TrainingPlanId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Exercise",
                table: "Exercise",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercise_TrainingPlans_TrainingPlanId",
                table: "Exercise",
                column: "TrainingPlanId",
                principalTable: "TrainingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseProgress_Exercise_ExerciseId",
                table: "ExerciseProgress",
                column: "ExerciseId",
                principalTable: "Exercise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
