using AutoMapper;
using PasswordManager.Entity.Concrete;
using PasswordManager.Entity.Dtos;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IUserService
    {
        Task<ResponseDto<UserParameter>> SignIn(UserLoginDto userDto);
        Task<ResponseDto<MUser>> Register(UserRegisterDto user);
        Task<ResponseDto<UserParameter>> GetUserFromRedis();
        Task<ResponseDto<int>> GetUserId();
        Task<ResponseDto<UserInformationsDto>> GetUserInformations();
        Task<ResponseDto<int>> Logout();
    }
}
