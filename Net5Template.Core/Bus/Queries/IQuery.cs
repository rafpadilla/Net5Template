using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net5Template.Core.Bus
{
    public interface IQuery<TResponse> : IRequest<TResponse>
    {
    }
}
