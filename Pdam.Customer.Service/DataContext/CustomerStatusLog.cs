using System;
using System.ComponentModel.DataAnnotations;

namespace Pdam.Customer.Service.DataContext
{
    public class CustomerStatusLog
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public byte RowStatusOldValue { get; set; }
        public byte RowStatusNewValue { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    
        public Customer Customer { get; set; }
    }
}