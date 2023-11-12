using PasswordManager.Entity.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IMyPasswordsService
    {
        Task<ResponseDto<MyPasswordDto>> Create(MyPasswordDto myPasswordDto);
        Task<ResponseDto<MyPasswordDto>> Update(MyPasswordDto myPasswordDto);
        Task<ResponseDto<int>> Delete(MyPasswordDto myPasswordDto);
        Task<ResponseDto<List<MyPasswordDto>>> GetAll();
    }
}
