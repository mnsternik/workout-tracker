using WorkoutTracker.Api.Models;

namespace WorkoutTracker.Tests.Builders
{
    public class ApplicationUserBuilder
    {
        private string _id = Guid.NewGuid().ToString();
        private string _email = "user@workoutracker.com";
        private string _displayName = "TestUser";
        private ICollection<TrainingSession> _trainingSessions = [];

        public ApplicationUserBuilder WithId(string id) 
        {
            _id = id;
            return this;
        }

        public ApplicationUserBuilder WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public ApplicationUserBuilder WithDisplayName(string displayName)
        {
            _displayName = displayName;
            return this;
        }

        public ApplicationUserBuilder WithTrainingSessions(ICollection<TrainingSession> trainingSessions)
        {
            _trainingSessions = trainingSessions;
            return this; 
        }

        public ApplicationUser BuildDomain()
        {
            return new ApplicationUser
            {
                Id = _id,
                Email = _email,
                DisplayName = _displayName,
                TrainingSessions = _trainingSessions
            };
        }

        public List<ApplicationUser> BuildManyDomains(int count)
        {
            return Enumerable.Range(1, count)
                .Select(i => new ApplicationUserBuilder()
                    .WithEmail($"user{count}@wourkouttracker.com")
                    .WithDisplayName($"TestUser{count}")
                    .BuildDomain())
                .ToList();
        }
    }
}
