using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vizsgaController.Persistence;

namespace ControllerTesting
{
    public class DbContextFactory
    {
        public static NewsDbContext Create()
        {
            // 1️⃣ SQLite in-memory kapcsolat
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open(); // FONTOS: nyitva kell maradnia

            // DbContextOptions, ugyan az mint postgres adatbázisnál
            var options = new DbContextOptionsBuilder<NewsDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            // DbContext létrehozása
            var context = new NewsDbContext(options);

            // Sémák létrehozása, hogy biztosan létezzen az adatbázis
            context.Database.EnsureCreated();

            // Seed adatok
            DbSeeder.Seed(context);

            return context;
        }

        public static NewsDbContext CreateEmpty()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<NewsDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new NewsDbContext(options);
            context.Database.EnsureCreated();

            // direkt NEM seedelünk, hogy ne legyen adat --> allroom tesztelése miatt
            return context;
        }
    }
}

