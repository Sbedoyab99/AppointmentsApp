namespace AppointmentsApp.Domain.Exceptions
{
    public class GenericException
    {
        public int Code { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = null!;
    }
}
