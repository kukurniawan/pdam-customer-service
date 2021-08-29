using System;
using System.ComponentModel.DataAnnotations;

namespace Pdam.Customer.Service.DataContext
{
    public class CustomerAddress
    {
        [Key]
        public Guid Id { get; set; }
        public byte RowStatus { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Village { get; set; }
        public string RW { get; set; }
        public string RT { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
        public Customer Customer { get; set; }
    }
}