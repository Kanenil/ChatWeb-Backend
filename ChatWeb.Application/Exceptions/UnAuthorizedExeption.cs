namespace ChatWeb.Application.Exceptions;

public class UnAuthorizedExeption : ApplicationException
{
    public UnAuthorizedExeption(string message) : base(message)
    {

    }
}
