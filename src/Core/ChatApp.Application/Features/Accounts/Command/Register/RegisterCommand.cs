using AutoMapper;
using ChatApp.Application.Persistence.Contracts;
using ChatApp.Application.Responses;
using ChatApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.Features.Accounts.Command.Register;
public class RegisterCommand:IRequest<BaseCommonResponse>
{
    public RegisterDto RegisterDto { get; set; }

    public RegisterCommand(RegisterDto registerDto)
    {
        RegisterDto = registerDto;
    }

    class Handler : IRequestHandler<RegisterCommand, BaseCommonResponse>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenServices _tokenServices;
        private readonly IMapper _mapper;

        public Handler(UserManager<AppUser> userManager,ITokenServices tokenServices,IMapper mapper)
        {
            _userManager = userManager;
            _tokenServices = tokenServices;
            _mapper = mapper;
        }
        public async Task<BaseCommonResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            BaseCommonResponse res = new();
            try
            {
                //Create Validator
                var validator = new RegisterValidator();
                var validatorResult = await validator.ValidateAsync(request.RegisterDto, cancellationToken);
                if (validatorResult.IsValid ==false) 
                {
                    res.IsSuccess = false;
                    res.Message = "While Validate Register Data";
                    res.Errors = validatorResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return res;
                }

                var user = _mapper.Map<AppUser>(request.RegisterDto);
                //var user = new AppUser()
                //{
                //    Email = request.RegisterDto.Email,
                //    UserName = request.RegisterDto.UserName,
                //    City = request.RegisterDto.City,
                //    Country = request.RegisterDto.Country,
                //    KnownAs = request.RegisterDto.KnownAs,
                //    DateOfBirth = request.RegisterDto.DateOfBirth,
                //    Gender = request.RegisterDto.Gender
                //};
                var response =  await _userManager.CreateAsync(user, request.RegisterDto.Password);
               if(response.Succeeded == false)
                {
                    foreach (var err in response.Errors)
                    {
                        res.Errors.Add($"{err.Code} - {err.Description}");
                    }
                    res.IsSuccess = false;
                    res.Message = "badRequest";
                    return res;
                }
                res.IsSuccess = true;
                res.Message = "register success";
                res.Data = new
                {
                    userName = user.UserName,
                    email = user.Email,
                    knownAs = user.KnownAs,
                    token =  _tokenServices.CreateToken(user),
                    photoUrl = user.Photos.FirstOrDefault(x=>x.IsMain)?.Url,
                    gender = user.Gender
                };
                return res;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;

                return res;
            }
        }
    }
}
