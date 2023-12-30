using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.Core.Entities;
using ECommerce.Core.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketRepository.GetBasketASync(id);    

            return basket ?? new CustomerBasket(id);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
        {
            var mappedBasket = _mapper.Map<CustomerBasketDto,CustomerBasket>(basket);  
            
            var updatedOrCreatedBasket = await _basketRepository.UpdateBasketAsync(mappedBasket);

            if(updatedOrCreatedBasket is null) return BadRequest(new ApiResponse(400));


            return updatedOrCreatedBasket;
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await _basketRepository.DeleteBasketAsync(id);
        }








    }
}
