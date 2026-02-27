namespace CarRent.Helper;
public class ServiceResult
{
    public bool IsSuccess {get; set;}
    public string Message {get; set;}

    public ServiceResult(bool isSuccess, string message)
    {
        IsSuccess = isSuccess;
        Message = message;
    }
    public static ServiceResult Success(string message = "Operacja zakończona sukcesem")
    {
        return new ServiceResult(true, message);
    }

    public static ServiceResult Failure(string message = "Operacja zakończona niepowodzeniem")
    {
        return new ServiceResult(false, message);
    }
}