using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Notizen.DbModel.Notizen;
using Notizen.Model;
using Notizen.Repository;
using Xunit;

namespace Notizen.Test.Service
{
    public sealed class NotizServiceTest
    {
        private readonly NotizRepository _notizRepository;
        public NotizServiceTest()
        {
            var services = new ServiceCollection();

            services.AddEntityFrameworkInMemoryDatabase();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase());
            services.AddMvc();

            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetService<ApplicationDbContext>();
            _notizRepository  = new NotizRepository(dbContext);
        }

        [Fact]
        public void VeraendereNotiz()
        {
            NotizModelErstellen nme = new NotizModelEditieren
            {
                Abgeschlossen = false,
                Beschreibung = "Meine Notizbeschreibung",
                Titel = "Meine Notiz",
                Wichtigkeit = 3
            };
            var id = _notizRepository.FuegeHinzu(nme);
            var notizmodel =_notizRepository.GetAsNotizModelEditieren(id);
            notizmodel.Abgeschlossen = true;
            _notizRepository.Aktualisiere(notizmodel);
            var neuernotizmodel = _notizRepository.GetAsNotizModelEditieren(id);
         
            Assert.Equal(neuernotizmodel.Abgeschlossen, true);

        }
    }
}

