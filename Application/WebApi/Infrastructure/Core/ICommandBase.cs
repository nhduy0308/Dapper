using System;
using WebAPI.Infrastructure.Core;

namespace Data.Infrastructure
{
    public interface ICommandBase : IDisposable
    {
        IResult Execute();
    }

    public interface ICommandBase<T> : IDisposable
    {
        IResult<T> Execute();
    }
}
