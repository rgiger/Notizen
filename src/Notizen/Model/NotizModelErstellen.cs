using System;
using System.ComponentModel.DataAnnotations;
using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelErstellen : NotizModelBase
    {
        public NotizModelErstellen()
        {
        }

        public NotizModelErstellen(NotizDbModel u) : base(u)
        {
            ErledigtBisDatum = u.ErledigtBis.Date;
            ErledigtBisZeit = u.ErledigtBis.TimeOfDay;
        }

        [DataType(DataType.Date)]
        public DateTime ErledigtBisDatum { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan ErledigtBisZeit { get; set; }

    }
}