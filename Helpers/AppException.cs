namespace WebApi.Helpers;

using System.Globalization;

// classe de exceção personalizada para lançar exceções específicas da aplicação (por exemplo, para validação)
// que podem ser capturadas e tratadas dentro da aplicação
public class AppException : Exception
{
    public AppException() : base() {}

    public AppException(string message) : base(message) { }

    public AppException(string message, params object[] args) 
        : base(String.Format(CultureInfo.CurrentCulture, message, args))
    {
    }
}