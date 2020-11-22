using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Bus
{
    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : ICommand<Unit>
    {
    }
    public interface ICommandHandler<in TCommand, T> : IRequestHandler<TCommand, T> where TCommand : ICommand<T>
    {
    }
}
