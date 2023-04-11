using System.Text.Json.Serialization;

namespace ShopApp.Business.Models
{
    public class CustomResponseModel<T>
    {
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public static CustomResponseModel<T> Success(T data, int statusCode)
        {
            return new CustomResponseModel<T> { Data = data, StatusCode = statusCode };
        }
        public static CustomResponseModel<T> Success(int statusCode)
        {
            return new CustomResponseModel<T> { StatusCode = statusCode };
        }
        public static CustomResponseModel<T> Fail(int statuscode, List<string> errors)
        {
            return new CustomResponseModel<T> { StatusCode = statuscode, Errors = errors };
        }
        public static CustomResponseModel<T> Fail(int statuscode, string error)
        {
            return new CustomResponseModel<T> { StatusCode = statuscode, Errors = new List<string> { error } };
        }
    }
}
