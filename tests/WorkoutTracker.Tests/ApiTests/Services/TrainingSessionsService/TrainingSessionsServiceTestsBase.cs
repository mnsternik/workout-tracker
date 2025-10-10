using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Mapping;
using WorkoutTracker.Api.Services.TrainingSessions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public class TrainingSessionsServiceTestsBase
    {
        private readonly SqliteConnection _connection;

        protected readonly ApplicationDbContext Context;
        protected readonly IMapper Mapper;
        protected readonly TrainingSessionsService TsService;

        protected TrainingSessionsServiceTestsBase()
        {
            // SQLite in-memory setup
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            Context = new ApplicationDbContext(options);
            Context.Database.EnsureCreated();

            // AutoMapper configuration 
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            Mapper = config.CreateMapper();

            TsService = new TrainingSessionsService(Context, Mapper);
        }

        protected void SeedWithDefaultSessions(int count = 25)
        {
            Context.TrainingSessions.RemoveRange(Context.TrainingSessions);
            Context.TrainingSessions.AddRange(new TrainingSessionBuilder().BuildManyDomains(count));
            Context.SaveChanges(); 
        }
    }
}
