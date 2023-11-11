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

        public async Task<ResponseDto<CountsDto>> GetCounts()
        {
            var rsp = new ResponseDto<CountsDto>();
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var users = await entity.USER.ToListAsync();
            var categories = await entity.CATEGORY.Where(x => x.ISDELETED == false).ToListAsync();
            var products = await entity.PRODUCT.Where(x => x.ISDELETED == false).ToListAsync();

            rsp.Data = new CountsDto();
            rsp.Data.UserCount = users.Count;
            rsp.Data.CategoryCount = categories.Count;
            rsp.Data.ProductCount = products.Count;

            return rsp;
        }
    }
}
