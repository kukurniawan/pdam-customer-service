using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Pdam.Common.Shared.State;

namespace Pdam.Customer.Service.DataContext
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        public byte RowStatus { get; set; }
        public string CompanyCode { get; set; }
        public string BranchCode { get; set; }
        public string Title { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerGroupCode { get; set; }
        public ActiveState Status { get; set; } 
        public Guid? RouterId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
        public IEnumerable<CustomerContact> CustomerContacts { get; set; }
        public IEnumerable<CustomerStatusLog> CustomerStatusLogs { get; set; }
        
        public Router Router { get; set; }
        public IEnumerable<CustomerAsset> CustomerAssets { get; set; }
    }
}