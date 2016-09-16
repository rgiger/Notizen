using System;
using System.ComponentModel.DataAnnotations;
using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelEditieren : NotizModelBase
    {
        public NotizModelEditieren()
        {
        }

        public NotizModelEditieren(NotizDbModel u) : base(u)
        {
            ErledigtBisDatum = u.ErledigtBis.Date;
            ErledigtBisZeit = u.ErledigtBis.TimeOfDay;
            Id = u.Id;
        }

        //public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime ErledigtBisDatum { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan ErledigtBisZeit { get; set; }
    }
}