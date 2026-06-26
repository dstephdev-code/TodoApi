namespace TodoApp.Api.Exceptions
{
    public class BadRequestException(string message) : Exception(message)
    {
    }
}
