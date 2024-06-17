using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class minorChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_SensorConfigurations_SensorSettingsId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_UserParent_Newborns_NewbornId",
                table: "UserParent");

            migrationBuilder.DropForeignKey(
                name: "FK_UserParent_Parents_ParentId",
                table: "UserParent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserParent",
                table: "UserParent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorConfigurations",
                table: "SensorConfigurations");

            migrationBuilder.RenameTable(
                name: "UserParent",
                newName: "UserParents");

            migrationBuilder.RenameTable(
                name: "SensorConfigurations",
                newName: "SensorSettings");

            migrationBuilder.RenameIndex(
                name: "IX_UserParent_NewbornId",
                table: "UserParents",
                newName: "IX_UserParents_NewbornId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserParents",
                table: "UserParents",
                columns: new[] { "ParentId", "NewbornId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorSettings",
                table: "SensorSettings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_SensorSettings_SensorSettingsId",
                table: "Sensors",
                column: "SensorSettingsId",
                principalTable: "SensorSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserParents_Newborns_NewbornId",
                table: "UserParents",
                column: "NewbornId",
                principalTable: "Newborns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserParents_Parents_ParentId",
                table: "UserParents",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_SensorSettings_SensorSettingsId",
                table: "Sensors");

            migrationBuilder.DropForeignKey(
                name: "FK_UserParents_Newborns_NewbornId",
                table: "UserParents");

            migrationBuilder.DropForeignKey(
                name: "FK_UserParents_Parents_ParentId",
                table: "UserParents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserParents",
                table: "UserParents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SensorSettings",
                table: "SensorSettings");

            migrationBuilder.RenameTable(
                name: "UserParents",
                newName: "UserParent");

            migrationBuilder.RenameTable(
                name: "SensorSettings",
                newName: "SensorConfigurations");

            migrationBuilder.RenameIndex(
                name: "IX_UserParents_NewbornId",
                table: "UserParent",
                newName: "IX_UserParent_NewbornId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserParent",
                table: "UserParent",
                columns: new[] { "ParentId", "NewbornId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SensorConfigurations",
                table: "SensorConfigurations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_SensorConfigurations_SensorSettingsId",
                table: "Sensors",
                column: "SensorSettingsId",
                principalTable: "SensorConfigurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserParent_Newborns_NewbornId",
                table: "UserParent",
                column: "NewbornId",
                principalTable: "Newborns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserParent_Parents_ParentId",
                table: "UserParent",
                column: "ParentId",
                principalTable: "Parents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
