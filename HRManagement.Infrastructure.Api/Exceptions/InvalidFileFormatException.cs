namespace HRManagement.Infrastructure.Api.Exceptions;

public class InvalidFileFormatException :Exception
{
    public InvalidFileFormatException(string message)
     : base(message) { }
}