using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelListe : NotizModelBase
    {

        public NotizModelListe(NotizDbModel u):base(u)
        {
            Id = u.Id;
        }

        public int Id { get; set; }

    }
}