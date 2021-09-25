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

namespace Pdam.Customer.Service.Features.Assets
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
            var entity = _mapper.Map<CustomerAsset>(request);
            entity.RowStatus = 0;
            entity.CreatedBy = "DEV";
            entity.CreatedDate = DateTime.Now;
            await _context.CustomerAssets.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"asset({entity.Id})"));
            return response;
        }

        public async Task<Response> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var asset = await _context.CustomerAssets.FirstOrDefaultAsync(c =>  c.Id == request.Id, cancellationToken);
            if (asset == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Asset Pelanggan dengan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (asset.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Asset pelanggan tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _mapper.Map(request, asset);
            asset.RowStatus = 0;
            asset.ModifiedBy = "DEV";
            asset.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"asset({asset.Id})"));
            return response;
        }

        public async Task<Response> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var asset = await _context.CustomerAssets.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (asset == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Asset pelanggan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (asset.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Asset pelanggan tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _context.CustomerAssets.Remove(asset);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response()
            {
                IsSuccessful = true, StatusCode = 200
            };
        }
    }
}