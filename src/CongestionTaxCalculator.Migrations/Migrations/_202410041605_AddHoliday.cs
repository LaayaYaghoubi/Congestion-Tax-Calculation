using FluentMigrator;

namespace CongestionTaxCalculator.Migrations.Migrations;
[Migration(202410041605)]
public class _202410041605_AddHoliday : Migration
{
    public override void Up()
    {
        Create.Table("Holidays")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Date").AsDate().NotNullable()
            .WithColumn("TaxSettingId").AsInt64().NotNullable()
            .ForeignKey("FK_Holidays_TaxSettings", "TaxSettings", "Id");
    }

    public override void Down()
    {
       Delete.Table("Holidays");
    }
}