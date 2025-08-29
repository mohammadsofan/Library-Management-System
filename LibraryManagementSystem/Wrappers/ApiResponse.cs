﻿namespace LibraryManagementSystem.Wrappers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }
        public static ApiResponse<T> Ok(T? data, string message)
        {
            return new ApiResponse<T>()
            {
                Success = true,
                Data = data,
                Message = message
            };
        }
        public static ApiResponse<T> Fail(string message, List<string>? errors)
        {
            return new ApiResponse<T>()
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }
        public static ApiResponse Ok(string message)
        {
            return new ApiResponse()
            {
                Success = true,
                Message = message
            };
        }
        public static ApiResponse Fail(string message, List<string>? errors)
        {
            return new ApiResponse()
            {
                Success = false,
                Message = message,
                Errors = errors
            };
        }
    }

}
