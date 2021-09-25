using System;
using MediatR;

namespace Pdam.Customer.Service.Features.Contacts
{
    public class Request
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneLine1 { get; set; }
        public string PhoneLine2 { get; set; }
        public string Fax { get; set; }
        public string MobilePhone1 { get; set; }
        public string MobilePhone2 { get; set; }
        public string Email { get; set; }
    }
}