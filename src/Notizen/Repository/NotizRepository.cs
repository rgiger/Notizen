using System;
using System.Collections.Generic;
using System.Linq;
using Notizen.DbModel;
using Notizen.DbModel.Notizen;
using Notizen.Model;

namespace Notizen.Repository
{
    public class NotizRepository
    {
        private readonly ApplicationDbContext _context;

        public NotizRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int FuegeHinzu(NotizModelErstellen nm)
        {
            var zuErledigenbis = nm.ErledigtBisDatum;
            if (nm.ErledigtBisZeit.HasValue)
            {
                zuErledigenbis = zuErledigenbis?.Add(nm.ErledigtBisZeit.Value);
            }
            var neueNotiz = new NotizDbModel
            {
                Erstelldatum = DateTime.Now,
                Beschreibung = nm.Beschreibung,
                Wichtigkeit = nm.Wichtigkeit,
                Titel = nm.Titel,
                ErledigtBis = zuErledigenbis,
            };
            if (nm.Abgeschlossen)
            {
                nm.AbgeschlossenZeitpunkt = DateTime.Now;
            }
            _context.Notizen.Add(neueNotiz);
            _context.SaveChanges();
            return neueNotiz.Id;
        }

        public void Aktualisiere(NotizModelEditieren nm)
        {
            var zuErledigenbis = nm.ErledigtBisDatum;
            if (nm.ErledigtBisZeit.HasValue)
            {
                zuErledigenbis = zuErledigenbis?.Add(nm.ErledigtBisZeit.Value);
            }
            var x = _context.Notizen.First(c => c.Id == nm.Id);

            if (!x.AbgeschlossenZeitpunkt.HasValue)
            {
                if (nm.Abgeschlossen)
                {
                    x.AbgeschlossenZeitpunkt = DateTime.Now;
                }
            }
            else
            {
                if (!nm.Abgeschlossen)
                {
                    x.AbgeschlossenZeitpunkt = null;
                }
            }
            x.Beschreibung = nm.Beschreibung;
            x.Wichtigkeit = nm.Wichtigkeit;
            x.Titel = nm.Titel;
            x.ErledigtBis = zuErledigenbis;
            _context.SaveChanges();
        }

        public void Loesche(int id)
        {
            var x = _context.Notizen.First(c => c.Id == id);
            _context.Notizen.Remove(x);
            _context.SaveChanges();
        }

        private List<NotizModelListe> FilterListe(List<NotizModelListe> x, bool filterAbgeschlossen)
        {
            return filterAbgeschlossen ? x.Where(c => !c.Abgeschlossen).ToList() : x;
        }

        private List<NotizModelListe> SortiereListe(List<NotizModelListe> x, string sortierung)
        {
            if (sortierung == SortierungsTyp.ErledigtBisDatum.ToString())
                x = x.OrderBy(c => c.ErledigtBis).ToList();
            else if (sortierung == SortierungsTyp.Wichtigkeit.ToString())
                x = x.OrderByDescending(c => c.Wichtigkeit).ToList();
            else if (sortierung == SortierungsTyp.Erstelldatum.ToString())
                x = x.OrderBy(c => c.Erstelldatum).ToList();
            return x;
        }

        public List<NotizModelListe> GetListe(bool filterAbgeschlossen, string sortierung)
        {
           var x = _context.Notizen.ToList().Select(u => new NotizModelListe(u)).ToList();
            x = FilterListe(x, filterAbgeschlossen);
            x = SortiereListe(x, sortierung);
            return x;
        }

        public bool Existiert(int id)
        {
            return _context.Notizen.Any(c => c.Id == id);
        }

        public NotizModelEditieren GetAsNotizModelEditieren(int id)
        {
            return new NotizModelEditieren(_context.Notizen.First(c => c.Id == id));
        }
    }
}