using FluentMigrator;

namespace CongestionTaxCalculator.Migrations.Migrations;
[Migration(202410041600)]
public class _202410041600_AddTaxRate : Migration
{
    public override void Up()
    {
        Create.Table("TaxRates")
            .WithColumn("Id").AsInt64().PrimaryKey().Identity()
            .WithColumn("Amount").AsDecimal(18,2).NotNullable()
            .WithColumn("Start").AsTime().NotNullable()
            .WithColumn("End").AsTime().NotNullable()
            .WithColumn("TaxSettingId").AsInt64().NotNullable()
            .ForeignKey("FK_TaxRates_TaxSettings", "TaxSettings", "Id");
            
    }

    public override void Down()
    {
        Delete.Table("TaxRates");
    }
}