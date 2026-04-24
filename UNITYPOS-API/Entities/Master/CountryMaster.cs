namespace UNITYPOS_API.Entities.Master
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class CountryMaster
    {
        [Column("id")]
        public byte? Id { get; set; }                  // tinyint NULL

        [Column("name")]
        public string? Name { get; set; }              // nvarchar(50) NULL

        [Column("iso3")]
        public string? Iso3 { get; set; }              // nvarchar(50) NULL

        [Column("iso2")]
        public string? Iso2 { get; set; }              // nvarchar(50) NULL

        [Column("numeric_code")]
        public short? NumericCode { get; set; }        // smallint NULL

        [Column("phonecode")]
        public short? Phonecode { get; set; }          // smallint NULL

        [Column("capital")]
        public string? Capital { get; set; }           // nvarchar(50) NULL

        [Column("currency")]
        public string? Currency { get; set; }          // nvarchar(50) NULL

        [Column("currency_name")]
        public string? CurrencyName { get; set; }      // nvarchar(50) NULL

        [Column("currency_symbol")]
        public string? CurrencySymbol { get; set; }    // nvarchar(50) NULL

        [Column("tld")]
        public string? Tld { get; set; }               // nvarchar(50) NULL

        [Column("native")]
        public string? Native { get; set; }            // nvarchar(100) NULL

        [Column("population")]
        public int? Population { get; set; }           // int NULL

        [Column("gdp")]
        public int? Gdp { get; set; }                  // int NULL

        [Column("region")]
        public string? Region { get; set; }            // nvarchar(50) NULL

        [Column("region_id")]
        public byte? RegionId { get; set; }            // tinyint NULL

        [Column("subregion")]
        public string? Subregion { get; set; }         // nvarchar(50) NULL

        [Column("subregion_id")]
        public byte? SubregionId { get; set; }         // tinyint NULL

        [Column("nationality")]
        public string? Nationality { get; set; }       // nvarchar(50) NULL

        [Column("area_sq_km")]
        public int? AreaSqKm { get; set; }             // int NULL

        [Column("latitude")]
        public double? Latitude { get; set; }          // float NULL

        [Column("longitude")]
        public double? Longitude { get; set; }         // float NULL

        [Column("emoji")]
        public string? Emoji { get; set; }             // nvarchar(50) NULL

        [Column("wikiDataId")]
        public decimal? WikiDataId { get; set; }       // money NULL
    }
}
