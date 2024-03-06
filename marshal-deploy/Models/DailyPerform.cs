namespace marshal_deploy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DailyPerform")]
    public partial class DailyPerform
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DailyPerform()
        {
            Deployments = new HashSet<Deployment>();
        }

        public int id { get; set; }

        public int? ShiftId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        public int? DailyTargetId { get; set; }

        public decimal? Target{ get; set; }

        public decimal? Total { get; set; }

        public decimal? Performance { get; set; }

        public int? Rating { get; set; }

        public int? ClusterId { get; set; }

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

        public virtual Cluster Cluster { get; set; }

        public virtual DailyTarget DailyTarget { get; set; }

        public virtual Shift Shift { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Deployment> Deployments { get; set; }
    }
}
