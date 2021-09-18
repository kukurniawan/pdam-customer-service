using System;
using MediatR;

namespace Pdam.Customer.Service.Features.Customers
{
    public class UpdateRequest: Request, IRequest<Response>
    {
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}