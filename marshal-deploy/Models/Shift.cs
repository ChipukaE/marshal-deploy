namespace marshal_deploy.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Shift
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shift()
        {
            DailyPerforms = new HashSet<DailyPerform>();
            DailyTargets = new HashSet<DailyTarget>();
            MonthlyPerforms = new HashSet<MonthlyPerform>();
            MonthlyTargets = new HashSet<MonthlyTarget>();
            PrecinctPerformances = new HashSet<PrecinctPerformance>();
        }

        public int id { get; set; }

        [StringLength(50)]
        public string DeviceShiftCode { get; set; }

        public int? PrecinctId { get; set; }

        [StringLength(50)]
        public string UserId { get; set; }

        public int? TerminalID { get; set; }

        public DateTime? start_datetime { get; set; }

        public DateTime? end_datetime { get; set; }

        public long? Duration { get; set; }

        public bool? IsOpen { get; set; }

        public decimal? ticketissued_amount { get; set; }

        public decimal? Epayments { get; set; }

        public decimal? total_arrears { get; set; }

        public decimal? total_prepaid { get; set; }

        public int? ShiftDiffCodeId { get; set; }

        public decimal? ShiftDifference { get; set; }

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

        public decimal? TotalCollectedUSD { get; set; }

        public decimal? TotalCollectedZW { get; set; }

        public decimal? TotalCountedZW { get; set; }

        public decimal? TotalCountedUSD { get; set; }

        public decimal? DeclaredVarianceZW { get; set; }

        public decimal? DeclaredVarianceUSD { get; set; }

        public decimal? CashupVarianceZW { get; set; }

        public decimal? CashupVarianceUSD { get; set; }

        public decimal? CashupZWD { get; set; }

        public decimal? CashupUSD { get; set; }

        public decimal? Total { get; set; }

        [StringLength(150)]
        public string Cashier { get; set; }

        public DateTime? CashupAudd { get; set; }

        public DateTime? LastConnection { get; set; }

        public decimal? CollectedEnforcementZW { get; set; }

        public decimal? CollectedEnforcementUSD { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyPerform> DailyPerforms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyTarget> DailyTargets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonthlyPerform> MonthlyPerforms { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MonthlyTarget> MonthlyTargets { get; set; }

        public virtual Precinct Precinct { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PrecinctPerformance> PrecinctPerformances { get; set; }
    }
}
