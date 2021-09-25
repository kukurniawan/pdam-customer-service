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

namespace Pdam.Customer.Service.Controllers
{
    public class AssetController : ODataController
    {
        private readonly IMediator _mediator;
        private readonly CustomerContext _context;

        public AssetController(IMediator mediator, CustomerContext context)
        {
            _mediator = mediator;
            _context = context;
        }
        
        [EnableQuery]
        [HttpGet]
        public IActionResult Get() 
        {
            return Ok( _context.CustomerAssets);
        }
        
        [EnableQuery]
        [HttpGet]
        public SingleResult<CustomerAsset> Get(Guid key) 
        {
            return new(_context.CustomerAssets.Where(x=> x.Id == key));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Features.Assets.CreateRequest request)
        {
            var response = await _mediator.Send(request);
            return ActionResultMapper.ToActionResult(response);
        }
        
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Features.Assets.UpdateRequest request)
        {
            var response = await _mediator.Send(request);
            return ActionResultMapper.ToActionResult(response);
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid key)
        {
            var response = await _mediator.Send(new Features.Assets.DeleteRequest
            {
                Id = key
            });
            return ActionResultMapper.ToActionResult(response);
        }
    }
}