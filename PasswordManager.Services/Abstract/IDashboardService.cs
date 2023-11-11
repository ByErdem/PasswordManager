using PasswordManager.Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Abstract
{
    public interface IDashboardService
    {
        Task<ResponseDto<CountsDto>> GetCounts();
    }
}
