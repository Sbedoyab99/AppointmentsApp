using Microsoft.AspNetCore.Http;

namespace AppointmentsApp.Domain.Responses
{
    public class ActionResponse<T>(bool success, string? message, T? data, int statusCode = StatusCodes.Status200OK)
    {
        public const int StatusOk = StatusCodes.Status200OK;
        public const int StatusCreated = StatusCodes.Status201Created;
        public const int StatusBadRequest = StatusCodes.Status400BadRequest;
        public const int StatusUnauthorized = StatusCodes.Status401Unauthorized;
        public const int StatusForbidden = StatusCodes.Status403Forbidden;
        public const int StatusNotFound = StatusCodes.Status404NotFound;
        public const int StatusConflict = StatusCodes.Status409Conflict;
        public const int StatusInternalServerError = StatusCodes.Status500InternalServerError;

        public int StatusCode { get; set; } = statusCode;
        public bool WasSuccess { get; set; } = success;
        public string? Message { get; set; } = message;
        public T? Result { get; set; } = data;

        public static ActionResponse<T> Ok(T data, string? message = null)
            => new(true, message, data, StatusOk);

        public static ActionResponse<T> Created(T? data = default, string? message = null)
            => new(true, message, data, StatusCreated);

        public static ActionResponse<T> BadRequest(string message)
            => new(false, message, default(T), StatusBadRequest);

        public static ActionResponse<T> Unauthorized(string message)
            => new(false, message, default(T), StatusUnauthorized);

        public static ActionResponse<T> Forbidden(string message)
            => new(false, message, default(T), StatusForbidden);

        public static ActionResponse<T> NotFound(string message)
            => new(false, message, default(T), StatusNotFound);

        public static ActionResponse<T> Conflict(string message)
            => new(false, message, default(T), StatusConflict);

        public static ActionResponse<T> InternalServerError(string message)
            => new(false, message, default(T), StatusInternalServerError);

        public static ActionResponse<T> Code(int statusCode, bool success, T? data = default, string? message = null)
            => new(success, message, data, statusCode);
    }
}
