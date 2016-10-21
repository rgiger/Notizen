using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelEditieren : NotizModelErstellen
    {
        public NotizModelEditieren()
        {
        }

        public NotizModelEditieren(NotizDbModel u) : base(u)
        {
            Id = u.Id;
        }
        
        public int Id { get; set; }
        
    }
}