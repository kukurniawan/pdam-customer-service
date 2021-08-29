using System;
using System.ComponentModel.DataAnnotations;

namespace Pdam.Customer.Service.DataContext
{
    public class CustomerContact
    {
        [Key]
        public Guid Id { get; set; }
        public byte RowStatus { get; set; }
        public Guid CustomerId { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneLine1 { get; set; }
        public string PhoneLine2 { get; set; }
        public string Fax { get; set; }
        public string MobilePhone1 { get; set; }
        public string MobilePhone2 { get; set; }
        public string Email { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Customer Customer { get; set; }
    }
}