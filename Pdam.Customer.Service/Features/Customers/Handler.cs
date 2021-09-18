using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;   
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pdam.Common.Shared.Fault;
using Pdam.Common.Shared.Http;
using Pdam.Customer.Service.DataContext;

namespace Pdam.Customer.Service.Features.Customers
{
    public class Handler : IRequestHandler<CreateRequest, Response>, IRequestHandler<UpdateRequest, Response>, IRequestHandler<DeleteRequest, Response>
    {
        private readonly CustomerContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Handler(CustomerContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public async Task<Response> Handle(CreateRequest request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => 
                c.CompanyCode == request.CompanyCode && c.CustomerCode == request.CustomerCode, cancellationToken);
            if (customer != null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Pelanggan dengan kode {request.CustomerCode} sudah terdaftar",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            var entity = _mapper.Map<DataContext.Customer>(request);
            entity.Id = Guid.NewGuid();
            entity.RowStatus = 0;
            entity.CreatedBy = "DEV";
            entity.CreatedDate = DateTime.Now;
            await _context.Customers.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"customer({entity.Id})"));
            return response;
        }

        public async Task<Response> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => 
                c.CompanyCode == request.CompanyCode && c.Id == request.Id, cancellationToken);
            if (customer == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Pelanggan dengan kode {request.CustomerCode} tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (customer.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Pelanggan dengan kode {request.CustomerCode} tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _mapper.Map(request, customer);
            customer.RowStatus = 0;
            customer.ModifiedBy = "DEV";
            customer.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"customer({customer.Id})"));
            return response;
        }

        public async Task<Response> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (customer == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Pelanggan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (customer.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Pelanggan dengan kode {customer.CustomerCode} tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response()
            {
                IsSuccessful = true, StatusCode = 200
            };
        }
    }
}