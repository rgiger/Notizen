using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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
            ErledigtBis = u.ErledigtBis;
            Abgeschlossen = u.Abgeschlossen;
            Beschreibung = u.Beschreibung;
            Erstelldatum = u.Erstelldatum;
            Id = u.Id;
            Titel = u.Titel;
            Wichtigkeit = u.Wichtigkeit;
        }

        public int Id { get; set; }
        [Required]
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        [Range(1, 5)]
        public short Wichtigkeit { get; set; }
        public bool Abgeschlossen { get; set; }
        [DataType(DataType.Date)]
        public DateTime Erstelldatum { get; set; }
        [DataType(DataType.Date)]
        public DateTime ErledigtBis { get; set; }

    }
}
