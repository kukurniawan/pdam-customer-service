using AutoMapper;

namespace Pdam.Customer.Service.Infrastructures
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Features.Customers.Request, DataContext.Customer>()
                .Include<Features.Customers.CreateRequest, DataContext.Customer>()
                .Include<Features.Customers.UpdateRequest, DataContext.Customer>();
            CreateMap<Features.Customers.CreateRequest, DataContext.Customer>();
            CreateMap<Features.Customers.UpdateRequest, DataContext.Customer>();    
            
            CreateMap<Features.Address.Request, DataContext.CustomerAddress>()
                .Include<Features.Address.CreateRequest, DataContext.CustomerAddress>()
                .Include<Features.Address.UpdateRequest, DataContext.CustomerAddress>();
            CreateMap<Features.Address.CreateRequest, DataContext.CustomerAddress>();
            CreateMap<Features.Address.UpdateRequest, DataContext.CustomerAddress>();  
            
            CreateMap<Features.Contacts.Request, DataContext.CustomerContact>()
                .Include<Features.Contacts.CreateRequest, DataContext.CustomerContact>()
                .Include<Features.Contacts.UpdateRequest, DataContext.CustomerContact>();
            CreateMap<Features.Contacts.CreateRequest, DataContext.CustomerContact>();
            CreateMap<Features.Contacts.UpdateRequest, DataContext.CustomerContact>();
            
            CreateMap<Features.Assets.Request, DataContext.CustomerAsset>()
                .Include<Features.Assets.CreateRequest, DataContext.CustomerAsset>()
                .Include<Features.Assets.UpdateRequest, DataContext.CustomerAsset>();
            CreateMap<Features.Assets.CreateRequest, DataContext.CustomerAsset>();
            CreateMap<Features.Assets.UpdateRequest, DataContext.CustomerAsset>();
        }
    }
}