using System.Collections.Generic;
using System;
namespace PlayList.Models
{
    public class ResponseData<T>
    {
        public bool Success
        {
            get
            {
                return Data.IsNotNullOrEmpty();
            }
        }
        public T Data { get; set; }
        public string Message { get; set; }
    }
    public class ResponseList<T> where T : class, new()
    {
        public bool Success
        {
            get
            {
                return Data.IsListNotNull();
            }
        }
        public List<T> Data { get; set; }
        public string Message { get; set; }
    }
}