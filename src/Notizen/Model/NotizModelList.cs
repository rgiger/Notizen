using System;
using System.ComponentModel.DataAnnotations;
using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelListe : NotizModelBase
    {

        public NotizModelListe(NotizDbModel u):base(u)
        {
            Id = u.Id;
            ErledigtBis = u.ErledigtBis;
          //  AbgeschlossenDatum = u.AbgeschlossenDatum;
            Erstelldatum = u.Erstelldatum;
        }

        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ErledigtBis { get; set; }
      //  [DataType(DataType.DateTime)]
       // public DateTime AbgeschlossenDatum { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Erstelldatum { get; set; }
    }
}