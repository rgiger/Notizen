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
            if (u.ErledigtBis != null)
            {
                ErledigtBisDatum = u.ErledigtBis.Value.Date;
                ErledigtBisZeit = u.ErledigtBis.Value.TimeOfDay;
            }
        }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? ErledigtBisDatum { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan? ErledigtBisZeit { get; set; }

    }
}