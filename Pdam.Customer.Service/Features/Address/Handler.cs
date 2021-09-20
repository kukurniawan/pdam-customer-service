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

namespace Pdam.Customer.Service.Features.Address
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
            var entity = _mapper.Map<CustomerAddress>(request);
            entity.RowStatus = 0;
            entity.CreatedBy = "DEV";
            entity.CreatedDate = DateTime.Now;
            await _context.CustomerAddress.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"address({entity.Id})"));
            return response;
        }

        public async Task<Response> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var address = await _context.CustomerAddress.FirstOrDefaultAsync(c =>  c.Id == request.Id, cancellationToken);
            if (address == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Alamat Pelanggan dengan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (address.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $" Alamat pelanggan tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _mapper.Map(request, address);
            address.RowStatus = 0;
            address.ModifiedBy = "DEV";
            address.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"address({address.Id})"));
            return response;
        }

        public async Task<Response> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var address = await _context.CustomerAddress.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (address == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Alamat pelanggan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (address.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Alamat pelanggan tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _context.CustomerAddress.Remove(address);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response()
            {
                IsSuccessful = true, StatusCode = 200
            };
        }
    }
}