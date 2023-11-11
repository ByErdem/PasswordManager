using PasswordManager.Entity.Dtos;
using PasswordManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PasswordManager.Services.Abstract
{
    public interface IProductService
    {
        Task<ResponseDto<ProductDto>> Create(ProductDto productDto, HttpContext context);
        Task<ResponseDto<PRODUCT>> Update(ProductDto productDto);
        Task<ResponseDto<int>> Delete(ProductDto productDto);
        Task<ResponseDto<int>> HardDelete(ProductDto productDto);
        Task<ResponseDto<List<ProductDto>>> GetAll();
    }
}
