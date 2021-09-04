using System;
using System.ComponentModel.DataAnnotations;
using Pdam.Common.Shared.State;

namespace Pdam.Customer.Service.DataContext
{
    public class CustomerAsset
    {
        [Key]
        public Guid Id { get; set; }
        public byte RowStatus { get; set; }
        public Guid CustomerId { get; set; }
        public ActiveState Status { get; set; }
        public string StatusDescription { get; set; }
        public string AssetTypeCode { get; set; }
        public string SerialNumber { get; set; }
        public string AssetDescription { get; set; }
        public DateTime? DateOfInstallation { get; set; }
        public string InstalledBy { get; set; }
        public string InstalledDocument { get; set; }
        public DateTime? DateOfWithdraw { get; set; }
        public string WithdrawBy { get; set; }
        public string WithdrawDocument { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Customer Customer { get; set; }
    }
}