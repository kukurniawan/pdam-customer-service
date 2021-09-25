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

namespace Pdam.Customer.Service.Features.Contacts
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
            var entity = _mapper.Map<CustomerContact>(request);
            entity.RowStatus = 0;
            entity.CreatedBy = "DEV";
            entity.CreatedDate = DateTime.Now;
            await _context.CustomerContacts.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"contact({entity.Id})"));
            return response;
        }

        public async Task<Response> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var contact = await _context.CustomerContacts.FirstOrDefaultAsync(c =>  c.Id == request.Id, cancellationToken);
            if (contact == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Kontak Pelanggan dengan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (contact.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Kontak pelanggan tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _mapper.Map(request, contact);
            contact.RowStatus = 0;
            contact.ModifiedBy = "DEV";
            contact.ModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);
            var response = new Response();
            response.AddLink("Get", _httpContextAccessor.GetFullLink("query", $"contact({contact.Id})"));
            return response;
        }

        public async Task<Response> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var contact = await _context.CustomerContacts.FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);
            if (contact == null) 
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 404,
                    Error = new ErrorDetail
                    {
                        Description = $"Kontak pelanggan tidak terdaftar",
                        ErrorCode = "2404",
                        StatusCode = HttpStatusCode.NotFound
                    }
                };
            if (contact.RowStatus != 0)
                return new Response
                {
                    IsSuccessful = false,
                    StatusCode = 422,
                    Error = new ErrorDetail
                    {
                        Description = $"Kontak pelanggan tidak dapat digunakan",
                        ErrorCode = "2422",
                        StatusCode = HttpStatusCode.UnprocessableEntity
                    }
                };

            _context.CustomerContacts.Remove(contact);
            await _context.SaveChangesAsync(cancellationToken);
            return new Response()
            {
                IsSuccessful = true, StatusCode = 200
            };
        }
    }
}