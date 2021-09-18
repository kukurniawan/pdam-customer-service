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
        }
    }
}