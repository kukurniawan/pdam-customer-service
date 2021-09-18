using System;
using MediatR;

namespace Pdam.Customer.Service.Features.Customers
{
    public class DeleteRequest : IRequest<Response>
    {
        public Guid Id { get; set; }
    }
}