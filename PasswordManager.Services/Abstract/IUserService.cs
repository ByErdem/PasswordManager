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
    }
}
