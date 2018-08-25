namespace PlanningPoker.Client.Exceptions
{
    internal class NotFoundException : System.Exception
    {
        internal NotFoundException() : base() { }
        internal NotFoundException(string message) : base(message)
        {

        }
        internal NotFoundException(string message, System.Exception exception) : base(message, exception)
        {

        }
    }
}