namespace marshal_deploy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PrecinctMonthly")]
    public partial class PrecinctMonthly
    {

        public int id { get; set; }

        public int? PrecinctPerformanceId { get; set; }

        public int? PrecinctId { get; set; }

        public int? ClusterId { get; set; }

        public int? ZoneId { get; set; }

        public decimal? Collected { get; set; }

        public decimal? Performance { get; set; }

        public decimal? Variance { get; set; }

        public int? Rating { get; set; }

        public DateTime? Audd { get; set; }

        [StringLength(50)]
        public string Audu { get; set; }

        [StringLength(50)]
        public string Audp { get; set; }

        public DateTime? lu_Audd { get; set; }

        [StringLength(50)]
        public string lu_Audu { get; set; }

        [StringLength(50)]
        public string lu_Audp { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Precinct Precinct { get; set; }

        public virtual Cluster Cluster { get; set; }

        public virtual PrecinctPerformance PrecinctPerformance { get; set; }
       
    }
}
