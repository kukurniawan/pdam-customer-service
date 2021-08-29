using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pdam.Customer.Service.DataContext
{
    public class Router
    {
        [Key]
        public Guid Id { get; set; }
        public byte RowStatus { get; set; }
        public string CompanyCode { get; set; }
        public string BranchCode { get; set; }
        public string GroupRoute { get; set; }
        public string ZoneRoute { get; set; }
        public string RouteCode { get; set; }
        public string RouteName { get; set; }
        public string CodeReader { get; set; }
        public int? DayInMonth { get; set; }
        public Guid? UserProfileId { get; set; }
        public int IndexReading { get; set; }
        public string Village { get; set; }
        public string District { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        public IEnumerable<Customer> Customers { get; set; }
    }
}