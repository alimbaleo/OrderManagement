using OrderManagement.Application.Contract.AppUsers;
using OrderManagement.Application.Contract.Shared;
using OrderManagement.Application.Helpers;
using OrderManagement.EntityFramework.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static OrderManagement.Domain.Exceptions.ErrorCodes;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Application
{
    public class AppUsersAppService : IAppUsersAppService
    {
        private readonly IAppUserRepository _appUserRepository;
        private readonly ICurrentUserInfo _currentUserInfo;
        private readonly IMapper _mapper;
        private readonly ILogger<AppUsersAppService> _logger;
        public AppUsersAppService(IAppUserRepository appUserRepository, ICurrentUserInfo currentUserInfo, IMapper mapper, ILogger<AppUsersAppService> logger)
        {
            _appUserRepository = appUserRepository;
            _currentUserInfo = currentUserInfo;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<ResponseDto<List<AppUserDto>>> GetAppUsers()
        {
            try
            {
                var isAdmin = _currentUserInfo.IsCurrentUserAdmin();
                if (!isAdmin)
                {
                    return new ResponseDto<List<AppUserDto>>("You are not authorized to complete this action", UNAUTHORIZED_OPERATION);
                }
                var response = await _appUserRepository.GetList();
                return new ResponseDto<List<AppUserDto>>(_mapper.Map<List<AppUser>, List<AppUserDto>>(response));
            }

            catch (BusinessException ex)
            {
                return new ResponseDto<List<AppUserDto>>(ex.Message, ex.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured getting users");
                return new ResponseDto<List<AppUserDto>>("An error occurred. Please refresh and try again", INTERNAL_SERVER_ERROR);
            }
        }
    }
}
