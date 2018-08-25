namespace PlanningPoker.Client.Exceptions
{
    internal class ConfigurationException : System.Exception
    {
        internal ConfigurationException() : base() { }
        internal ConfigurationException(string message) : base(message)
        {

        }
        internal ConfigurationException(string message, System.Exception exception) : base(message, exception)
        {

        }
    }
}