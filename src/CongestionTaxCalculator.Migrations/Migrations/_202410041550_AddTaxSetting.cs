using FluentMigrator;
namespace CongestionTaxCalculator.Migrations.Migrations;
[Migration(202410031550)]
public class _202410031550_AddTaxSetting :Migration
{
    public override void Up()
    {
        Create.Table("TaxSettings")
            .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey()
            .WithColumn("CityName").AsString().NotNullable()
            .WithColumn("MaxTaxPerDay").AsDecimal(18,2).NotNullable()
            .WithColumn("SingleChargeIntervalMinutes").AsInt32().NotNullable()
            .WithColumn("IsHolidayTaxFree").AsBoolean().NotNullable()
            .WithColumn("IsDayBeforeHolidayTaxFree").AsBoolean().NotNullable()
            .WithColumn("IsDayAfterHolidayTaxFree").AsBoolean().NotNullable()
            .WithColumn("IsWeekendTaxFree").AsBoolean().NotNullable();
    }

    public override void Down()
    {
        Delete.Table("TaxSettings");
    }
}