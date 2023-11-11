using PasswordManager.Entity.Concrete;
using PasswordManager.Entity.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface ICategoryService
    {
        Task<ResponseDto<CategoryDto>> Create(CategoryDto categoryDto);
        Task<ResponseDto<CategoryDto>> Update(CategoryDto categoryDto);
        Task<ResponseDto<int>> Delete(MCategory categoryDto);
        Task<ResponseDto<int>> HardDelete(MCategory categoryDto);
        Task<ResponseDto<List<CategoryDto>>> GetAll();
        Task<ResponseDto<List<CategoryDto>>> GetMainCategories();
        Task<ResponseDto<List<CategoryDto>>> GetSubCategories(int CategoryId);
        Task<string> FindParentCategoryName(int CATEGORYID);
        Task<ResponseDto<List<MCategory>>> GetAllByParentID(MCategory categoryDto);
    }
}
