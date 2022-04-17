namespace WpfApp1.DAL
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TimeTeacher
    {
        public int Id { get; set; }

        public DateTime TimeInput { get; set; }

        public DateTime TimeOutput { get; set; }

        public int TimeWork { get; set; }

        public int IdUser { get; set; }

        public virtual User User { get; set; }
    }
}
