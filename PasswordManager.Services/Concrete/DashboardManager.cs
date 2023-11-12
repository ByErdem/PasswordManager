using PasswordManager.Data;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services.Concrete
{
    public class DashboardManager : IDashboardService
    {
        private readonly IDbContextEntity _entity;

        public DashboardManager(IDbContextEntity entity)
        {
            _entity = entity;
        }

        public async Task<ResponseDto<CountsDto>> GetCounts()
        {
            var rsp = new ResponseDto<CountsDto>();
            var users = await _entity.USER.ToListAsync();
            var categories = await _entity.CATEGORY.Where(x => x.ISDELETED == false).ToListAsync();
            var passwords = await _entity.MYPASSWORDS.ToListAsync();

            rsp.Data = new CountsDto();
            rsp.Data.UserCount = users.Count;
            rsp.Data.CategoryCount = categories.Count;
            rsp.Data.PasswordsCount = passwords.Count;

            return rsp;
        }
    }
}
