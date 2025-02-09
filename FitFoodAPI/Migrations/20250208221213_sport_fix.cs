using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitFoodAPI.Migrations
{
    /// <inheritdoc />
    public partial class sport_fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Set_ExerciseProgress_ExerciseProgressId",
                table: "Set");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingPlans_TrainigPlanId",
                table: "Trainings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Set",
                table: "Set");

            migrationBuilder.RenameTable(
                name: "Set",
                newName: "Sets");

            migrationBuilder.RenameColumn(
                name: "TrainigPlanId",
                table: "Trainings",
                newName: "TrainingPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Trainings_TrainigPlanId",
                table: "Trainings",
                newName: "IX_Trainings_TrainingPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Set_ExerciseProgressId",
                table: "Sets",
                newName: "IX_Sets_ExerciseProgressId");

            migrationBuilder.AddColumn<bool>(
                name: "isDeleted",
                table: "TrainingPlans",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isCompleted",
                table: "Sets",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sets",
                table: "Sets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sets_ExerciseProgress_ExerciseProgressId",
                table: "Sets",
                column: "ExerciseProgressId",
                principalTable: "ExerciseProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingPlans_TrainingPlanId",
                table: "Trainings",
                column: "TrainingPlanId",
                principalTable: "TrainingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sets_ExerciseProgress_ExerciseProgressId",
                table: "Sets");

            migrationBuilder.DropForeignKey(
                name: "FK_Trainings_TrainingPlans_TrainingPlanId",
                table: "Trainings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sets",
                table: "Sets");

            migrationBuilder.DropColumn(
                name: "isDeleted",
                table: "TrainingPlans");

            migrationBuilder.DropColumn(
                name: "isCompleted",
                table: "Sets");

            migrationBuilder.RenameTable(
                name: "Sets",
                newName: "Set");

            migrationBuilder.RenameColumn(
                name: "TrainingPlanId",
                table: "Trainings",
                newName: "TrainigPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Trainings_TrainingPlanId",
                table: "Trainings",
                newName: "IX_Trainings_TrainigPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Sets_ExerciseProgressId",
                table: "Set",
                newName: "IX_Set_ExerciseProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Set",
                table: "Set",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Set_ExerciseProgress_ExerciseProgressId",
                table: "Set",
                column: "ExerciseProgressId",
                principalTable: "ExerciseProgress",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trainings_TrainingPlans_TrainigPlanId",
                table: "Trainings",
                column: "TrainigPlanId",
                principalTable: "TrainingPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
