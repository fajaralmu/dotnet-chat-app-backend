using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Dto
{
    public class WebResponse<T>
    {
        public DateTime Date {get; set;} = DateTime.Now;
        public int Success {get; set;} = 1;
        public string Message {get; set;}
        public T Result {get; set;}

        public static WebResponse<T>  SuccessResponse(T returnValue)
        {
            WebResponse<T> response = new WebResponse<T>();
            response.Result = returnValue;
            return response;
        }

        public static WebResponse<string> ErrorResponse(Exception exception)
        {
            WebResponse<string> response = new WebResponse<string>();
            response.Success = 0;
            response.Message = exception.GetType().Name;
            response.Result = exception.Message;
            return response;
        }
    }
}