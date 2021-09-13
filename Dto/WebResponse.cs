using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ChatAPI.Dto
{
    public class WebResponse<T>
    {
        public DateTime Date {get; set;} = DateTime.Now;
        public int Success {get; set;} = 1;
        public T Result {get; set;}

        public static WebResponse<T>  success(T returnValue)
        {
            WebResponse<T> response = new WebResponse<T>();
            response.Result = returnValue;
            return response;
        }
    }
}