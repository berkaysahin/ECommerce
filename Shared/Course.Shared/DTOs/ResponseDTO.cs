using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Course.Shared.DTOs
{
    public class ResponseDTO<T>
    {
        public T Data { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; set; }

        [JsonIgnore]
        public int StatusCode { get; private set; }

        // STATIC FACTORY METHODS

        public static ResponseDTO<T> Success(T data, int statusCode)
        {
            return new ResponseDTO<T> { Data = data, StatusCode = statusCode, IsSuccessful = true };
        }

        public static ResponseDTO<T> Success(int statusCode)
        {
            return new ResponseDTO<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static ResponseDTO<T> Fail(List<string> errors, int statusCode)
        {
            return new ResponseDTO<T> { Errors = errors, StatusCode = statusCode, IsSuccessful = false };
        }

        public static ResponseDTO<T> Fail(string error, int statusCode)
        {
            return new ResponseDTO<T> { Errors = new List<string>() { error }, StatusCode = statusCode, IsSuccessful = false };
        }
    }
}
