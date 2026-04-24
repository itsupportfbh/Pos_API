namespace UNITYPOS_API.Entities.Master
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class CityMaster
    {
        [Column("id")]
        public int? Id { get; set; }             // int NULL

        [Column("name")]
        public string? Name { get; set; }        // nvarchar(100) NULL

        [Column("state_id")]
        public short? StateId { get; set; }      // smallint NULL

        [Column("country_id")]
        public byte? CountryId { get; set; }     // tinyint NULL

        [Column("latitude")]
        public double? Latitude { get; set; }    // float NULL

        [Column("longitude")]
        public double? Longitude { get; set; }   // float NULL

        [Column("timezone")]
        public string? Timezone { get; set; }    // nvarchar(50) NULL
    }
}
