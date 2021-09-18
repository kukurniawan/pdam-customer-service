using System;
using Pdam.Common.Shared.State;

namespace Pdam.Customer.Service.Features.Customers
{
    public class Request
    {
        public Guid Id { get; set; }
        public string CompanyCode { get; set; }
        public string BranchCode { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerGroupCode { get; set; }
        public ActiveState Status { get; set; } 
        public Guid? RouterId { get; set; }
    }
}