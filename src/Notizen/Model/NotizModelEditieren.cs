using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelEditieren : NotizModelBase
    {
        public NotizModelEditieren():base()
        {

        }
        public NotizModelEditieren(NotizDbModel u) :base(u)
        {
            Id = u.Id;
        }

        public int Id { get; set; }

    }
}