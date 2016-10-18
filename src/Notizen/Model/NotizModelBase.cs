using System;
using System.ComponentModel.DataAnnotations;
using Notizen.DbModel;

namespace Notizen.Model
{
    public abstract class NotizModelBase
    {
        public NotizModelBase()
        {
        }

        public NotizModelBase(NotizDbModel u)
        {
            Abgeschlossen = u.AbgeschlossenZeitpunkt.HasValue;
            AbgeschlossenZeitpunkt = u.AbgeschlossenZeitpunkt;
            Beschreibung = u.Beschreibung;
            Titel = u.Titel;
            Wichtigkeit = u.Wichtigkeit;
        }


        [DataType(DataType.DateTime)]
        public DateTime? AbgeschlossenZeitpunkt { get; set; }


        [Required]
        public string Titel { get; set; }

        public string Beschreibung { get; set; }

        [Required]
        [Range(1, 5)]
        public short Wichtigkeit { get; set; }

        public bool Abgeschlossen { get; set; }

    }
}