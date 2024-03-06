namespace marshal_deploy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Deployment
    {
        public int id { get; set; }

        public int? ShiftId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        public int? DailyPerformId { get; set; }

        public int? PrecinctPerformanceId { get; set; }

        public int? PrecinctId { get; set; }

        public int? ZoneId { get; set; }

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

        public virtual Zone Zone { get; set; }

        public virtual DailyPerform DailyPerform { get; set; }

        public virtual PrecinctPerformance PrecinctPerformance { get; set; }
    }
}
