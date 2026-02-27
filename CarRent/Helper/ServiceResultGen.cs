namespace CarRent.Helper;
public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; set; }

    public ServiceResult(T data, bool isSuccess, string message) 
        : base(isSuccess, message)
    {
        Data = data;
    }

    public ServiceResult(bool isSuccess, string message) 
        : base(isSuccess, message)
    {
        Data = default;
    }
    public static ServiceResult<T> Success(T data, string message = "Operacja zakończona sukcesem")
    {
        return new ServiceResult<T>(data, true, message);
    }

    public new static ServiceResult<T> Failure(string message = "Operacja zakończona niepowodzeniem")
    {
        return new ServiceResult<T>(false, message);
    }
}