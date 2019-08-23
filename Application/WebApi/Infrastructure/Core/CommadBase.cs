using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAPI.Infrastructure.Core;

namespace WebApi.Infrastructure.Core
{
    public abstract class CommadBase : ICommandBase
    {
        protected virtual void ValidateCore()
        {
        }
        protected virtual void OnExecutingCore()
        {
        }
        protected virtual void OnExecutedCore(IResult result)
        {
        }
        protected abstract IResult ExecuteCore();
        public IResult Execute()
        {
            try
            {
                ValidateCore();
                OnExecutingCore();
                var result = ExecuteCore();
                OnExecutedCore(result);
                return result;
            }
            catch (Exception ex)
            {
                return new Result
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public virtual void Dispose()
        {
        }

        protected Result Success(string message = null)
        {
            return new Result
            {
                IsSuccess = true,
                Message = message ?? "success",

            };
        }

    }

    public abstract class CommandBase<T> : ICommandBase<T>
    {
        protected virtual void ValidateCore()
        {
        }
        protected virtual void OnExecutingCore()
        {
        }
        protected virtual void OnExecutedCore(IResult result)
        {
        }
        protected abstract IResult<T> ExecuteCore();
        public IResult<T> Execute()
        {
            try
            {
                ValidateCore();
                OnExecutingCore();
                var result = ExecuteCore();
                OnExecutedCore(result);
                return result;
            }
            catch (Exception ex)
            {
                return new Result<T>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Error = new ResultError
                    {
                        Code = 500,
                        Message = ex.Message,
                        StackTrace = ex.StackTrace
                    }
                };
            }
        }

        public virtual void Dispose()
        {
        }

        protected IResult<T> Success(T data,string message = null)
        {
            return new Result<T>
            {
                Data = data,
                IsSuccess = true,
                Message = message ?? "success"
            };
        }
    }
}