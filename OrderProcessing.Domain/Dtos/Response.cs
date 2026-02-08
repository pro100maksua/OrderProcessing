namespace OrderProcessing.Domain.Dtos;

public class Response<T>
{
    public T Value { get; set; }

    public int? ErrorCode { get; set; }
    public string ErrorMessage { get; set; }

    public Response(T value)
    {
        Value = value;
    }

    public Response(int errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }
}