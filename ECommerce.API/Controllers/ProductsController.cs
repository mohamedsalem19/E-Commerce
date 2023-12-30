using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.API.Helpers;
using ECommerce.Core;
using ECommerce.Core.Entities;
using ECommerce.Core.IRepositories;
using ECommerce.Core.Specifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IMapper mapper , IUnitOfWork unitOfWork)
        {
            
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        //[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetProducts([FromQuery]ProductSpecParams SpecParams)
        {
            var spec = new ProductWithBrandAndCategorySpecification(SpecParams);
           
            var products = await _unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
           
            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var countSpec = new ProductWithFilterationForCountSpecfication(SpecParams);

            var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(SpecParams.PageIndex, SpecParams.PageSize,count,data));    
        }


        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound )]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndCategorySpecification(id);
           
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);

            if (product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product,ProductToReturnDto>(product));
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> Getcategories()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return Ok(categories);
        }




    }
}
