using Notizen.DbModel;

namespace Notizen.Model
{
    public class NotizModelList : NotizModelBase
    {
        

        public NotizModelList(NotizDbModel u):base(u)
        {
            Id = u.Id;
        }

        public int Id { get; set; }

    }
}