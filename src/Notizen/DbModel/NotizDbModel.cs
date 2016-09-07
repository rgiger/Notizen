using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Notizen.DbModel;
using Notizen.Model;

namespace Notizen.DbModel
{
    public class NotizDbModel
    {


        public int Id { get; set; }
        public string Title { get; set; }
        public string Beschreibung { get; set; }
        public short Wichtigkeit { get; set; }
        public bool Abgeschlossen { get; set; }
        public DateTime Erstelldatum { get; set; }
        public DateTime ErledigtBis { get; set; }
    }
}
