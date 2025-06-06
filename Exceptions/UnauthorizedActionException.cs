namespace WorkoutTracker.Api.Exceptions
{
    public class UnauthorizedActionException : Exception
    {
        public UnauthorizedActionException(string message) : base(message) { }
    }
}
