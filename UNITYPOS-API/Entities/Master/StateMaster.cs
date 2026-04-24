namespace UNITYPOS_API.Entities.Master
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class StateMaster
    {
        [Column("id")]
        public short? Id { get; set; }

        [Column("name")]
        public string? Name { get; set; }

        [Column("country_id")]
        public byte? CountryId { get; set; }

        [Column("country_code")]
        public string? CountryCode { get; set; }

        [Column("country_name")]
        public string? CountryName { get; set; }

        [Column("iso2")]
        public string? Iso2 { get; set; }

        [Column("iso3166_2")]
        public string? Iso3166_2 { get; set; }

        [Column("fips_code")]
        public string? FipsCode { get; set; }

        [Column("type")]
        public string? Type { get; set; }

        [Column("level")]
        public int? Level { get; set; }

        [Column("parent_id")]
        public string? ParentId { get; set; }

        [Column("native")]
        public string? Native { get; set; }

        [Column("latitude")]
        public double? Latitude { get; set; }

        [Column("longitude")]
        public double? Longitude { get; set; }

        [Column("timezone")]
        public string? Timezone { get; set; }

        [Column("wikiDataId")]
        public decimal? WikiDataId { get; set; }

        [Column("population")]
        public int? Population { get; set; }
    }
}
