using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using WorkoutTracker.Api.Data;
using WorkoutTracker.Api.Mapping;
using WorkoutTracker.Api.Services.ExerciseDefinitions;
using WorkoutTracker.Tests.Builders;

namespace WorkoutTracker.Tests.ApiTests.Services
{
    public abstract class ExerciseDefinitionsServiceTestsBase
    {
        private readonly SqliteConnection _connection;

        protected readonly ApplicationDbContext Context;
        protected readonly IMapper Mapper;
        protected readonly ExerciseDefinitionsService EdService; 

        protected ExerciseDefinitionsServiceTestsBase()
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

            EdService = new ExerciseDefinitionsService(Context, Mapper);
        }

        protected void SeedDatabaseWithDefaults(int count = 25)
        {
            Context.ExerciseDefinitions.RemoveRange(Context.ExerciseDefinitions);
            Context.SaveChanges();

            Context.ExerciseDefinitions.AddRange(new ExerciseDefinitionBuilder().BuildManyDomains(count));
            Context.SaveChanges();
        }
    }
}
