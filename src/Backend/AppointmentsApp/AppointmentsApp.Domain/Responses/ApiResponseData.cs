namespace AppointmentsApp.Domain.Responses
{
    public class ApiResponseData<T> : ApiResponse
    {
        public T? Data { get; set; }
    }
}