namespace WpfApp1.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Point
    {
        public int Id { get; set; }

        [Column("Point")]
        public int Point1 { get; set; }

        public int IdItem { get; set; }

        public int IdStudent { get; set; }

        public int IdTeacher { get; set; }

        public virtual Item Item { get; set; }

        public virtual Student Student { get; set; }

        public virtual Teacher Teacher { get; set; }

       
    }
}
