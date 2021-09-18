using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Pdam.Common.Shared.Infrastructure;
using Pdam.Customer.Service.DataContext;
using Pdam.Customer.Service.Features.Customers;

namespace Pdam.Customer.Service.Controllers
{
    public class CustomerController : ODataController
    {
        private readonly IMediator _mediator;
        private readonly CustomerContext _context;

        public CustomerController(IMediator mediator, CustomerContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        
        [EnableQuery]
        [HttpGet]
        public IActionResult Get([FromRoute]string companyCode) 
        {
            return Ok( _context.Customers.Where(x=>x.CompanyCode == companyCode));
        }
        
        [EnableQuery]
        [HttpGet]
        public SingleResult<DataContext.Customer> Get(Guid key) 
        {
            return new(_context.Customers.Where(x=> x.Id == key));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateRequest request)
        {
            var response = await _mediator.Send(request);
            return ActionResultMapper.ToActionResult(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateRequest request)
        {
            var response = await _mediator.Send(request);
            return ActionResultMapper.ToActionResult(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid key)
        {
            var response = await _mediator.Send(new DeleteRequest
            {
                Id = key
            });
            return ActionResultMapper.ToActionResult(response);
        }
      
    }
}