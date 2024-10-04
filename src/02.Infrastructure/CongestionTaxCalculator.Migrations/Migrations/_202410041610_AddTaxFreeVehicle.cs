using FluentMigrator;

namespace CongestionTaxCalculator.Migrations.Migrations;
[Migration(202410041610)]
public class _202410041610_AddTaxFreeVehicle : Migration
{
    public override void Up()
    {
        Create.Table("TaxFreeVehicles")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("VehicleType").AsInt32().NotNullable()
            .WithColumn("TaxSettingId").AsInt64().NotNullable()
            .ForeignKey("FK_TaxFreeVehicles_TaxSettings", "TaxSettings", "Id");
    }

    public override void Down()
    {
        Delete.Table("TaxFreeVehicles");
    }
}