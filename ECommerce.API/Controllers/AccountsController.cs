using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.API.Extensions;
using ECommerce.Core.Entities.Identity;
using ECommerce.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService token,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = token;
            _mapper = mapper;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) return Unauthorized(new ApiResponse(401));

            var reault = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if(!reault.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token =await _tokenService.CreateTokenAsync(user,_userManager)
            }) ;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            if(CheckEmailAsync(model.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse() { Errors = new string[]{ "this email is already exist" } });

            var user = new AppUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {

                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email= User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user,_userManager)
            });
        }


        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserWithAddressByEmailAsync(User);

            var address = _mapper.Map<Address, AddressDto>(user.address);
           
            return Ok(address);
         
        }


        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updateAddress)
        {
            var address = _mapper.Map<AddressDto,Address>(updateAddress);   

            var user = await _userManager.FindUserWithAddressByEmailAsync(User);

            address.Id = user.address.Id;

            user.address = address; 

            var result = await _userManager.UpdateAsync(user);

            if(!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(updateAddress);

        }


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null ;
        }

    }
}
