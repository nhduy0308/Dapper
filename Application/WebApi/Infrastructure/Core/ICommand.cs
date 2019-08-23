using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Infrastructure.Core
{
    public interface IResult
    {
        IResultError Error { get; set; }
        string Message { get; set; }
        bool IsSuccess { get; set; }
    }
    public interface IResultError
    {
        string Message { get; set; }
        int Code { get; set; }
    }
    public interface IResult<T> : IResult
    {
        T Data { get; set; }
    }
    public class Result : IResult
    {
        public bool IsSuccess { get; set; }
        public IResultError Error { get; set; }
        public string Message { get; set; }
    }
    public class ResultError : IResultError
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public int Code { get; set; }
    }
    public class Result<T> : Result, IResult<T>
    {
        public T Data { get; set; }
    }


}
