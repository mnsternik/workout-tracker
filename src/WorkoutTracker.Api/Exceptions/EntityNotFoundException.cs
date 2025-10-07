namespace WorkoutTracker.Api.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public string EntityName { get; } = string.Empty;
        public object EntityId { get; } = string.Empty; 

        public EntityNotFoundException(string message) : base(message) { }

        public EntityNotFoundException(string entityName, object entityId) 
            : base($"Entity '{entityName}' with ID '{entityId}' not found.")
        {
            EntityName = entityName;
            EntityId = entityId;
        }
    }
}
