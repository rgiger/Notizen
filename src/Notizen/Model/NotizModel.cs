using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Notizen.Model
{
    public class NotizModel
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
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
