namespace Microservicio.ReservasF.Business.Exceptions;

public class UnauthorizedBusinessException : BusinessException
{
    public UnauthorizedBusinessException(string message)
        : base("UNAUTHORIZED", message, 401)
    {
    }
}