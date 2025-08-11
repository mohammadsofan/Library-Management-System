using LibraryManagementSystem.Enums;

namespace LibraryManagementSystem.Wrappers
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }
        public ServiceResultStatus Status { get; set; }
        public static ServiceResult<T> Ok(T? data,string message)
        {
            return new ServiceResult<T>()
            {
                Success = true,
                Data = data,
                Message = message,
                Status = ServiceResultStatus.Ok
            };
        }
        public static ServiceResult<T> Fail(string message,List<string>? errors, ServiceResultStatus status = ServiceResultStatus.Error)
        {
            return new ServiceResult<T>()
            {
                Success = false,
                Message = message,
                Errors = errors,
                Status = status
            };
        }
    }
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public ServiceResultStatus Status { get; set; }
        public static ServiceResult Ok(string message,ServiceResultStatus status = ServiceResultStatus.Ok)
        {
            return new ServiceResult()
            {
                Success = true,
                Message = message,
                Status = status
            };
        }
        public static ServiceResult Fail(string message, List<string>? errors,ServiceResultStatus status = ServiceResultStatus.Error)
        {
            return new ServiceResult()
            {
                Success = false,
                Message = message,
                Errors = errors,
                Status = status
            };
        }
    }
}
