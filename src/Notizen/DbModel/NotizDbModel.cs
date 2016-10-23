using System;

namespace Notizen.DbModel
{
    public class NotizDbModel
    {
        public int Id { get; set; }
        public string Titel { get; set; }
        public string Beschreibung { get; set; }
        public short Wichtigkeit { get; set; }
        public DateTime? AbgeschlossenZeitpunkt { get; set; }
        public DateTime Erstelldatum { get; set; }
        public DateTime? Termin { get; set; }
    }
}