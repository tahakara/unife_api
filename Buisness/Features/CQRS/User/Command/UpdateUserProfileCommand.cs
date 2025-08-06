using Buisness.DTOs.UserDtos.UpdateUserProfileDtos;
using Buisness.Features.CQRS.Base.Auth;
using Buisness.Features.CQRS.Base.Generic.Request.Command;
using Buisness.Features.CQRS.Base.Generic.Response;
using Buisness.Helpers.BuisnessLogicHelpers.Auth.Base;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.AuthCarrierInterfaces;
using Buisness.Validators.FluentValidation.Carriers.CarrierInterfaces.ProfileCarrierInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buisness.Features.CQRS.User.Command
{
    public class UpdateUserProfileCommand : ICommand<BaseResponse<UpdateUserProfileResponseDto>>,
        IAccessTokenCarrier,
        IFirstNameCarrier,
        IMiddleNameCarrier,
        ILastNameCarrier,
        IEmailCarrier,
        IPhoneCarrier, 
        IProfileImageUrlCarrier

    {
        public string? AccessToken { get; set; } = null;
        public string? FirstName { get; set; } = null;
        public string? MiddleName { get; set; } = null;
        public string? LastName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? PhoneCountryCode { get; set; } = null;
        public string? PhoneNumber { get; set; } = null;
        public string? ProfileImageUrl { get; set; } = null;
    }

    public class UpdateUserProfileCommandHandler : AuthCommandHandlerBase<UpdateUserProfileCommand>, ICommandHandler<UpdateUserProfileCommand, BaseResponse<UpdateUserProfileResponseDto>>
    {
        public UpdateUserProfileCommandHandler(
            IAuthBuisnessLogicHelper authBusinessLogicHelper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<UpdateUserProfileCommand> logger)
            : base(authBusinessLogicHelper, httpContextAccessor, logger, "UpdateUserProfile")
        {
        }

        public async Task<BaseResponse<UpdateUserProfileResponseDto>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            return BaseResponse<UpdateUserProfileResponseDto>.Success(
                data: new UpdateUserProfileResponseDto
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneCountryCode = request.PhoneCountryCode,
                    PhoneNumber = request.PhoneNumber,
                    ProfileImageUrl = request.ProfileImageUrl
                },
                message: "User profile updated successfully",
                statusCode: 200);
        }
    }
}
