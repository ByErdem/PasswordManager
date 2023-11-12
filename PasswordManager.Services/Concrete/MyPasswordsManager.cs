using AutoMapper;
using PasswordManager.Data;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using PasswordManager.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services.Concrete
{
    public class MyPasswordsManager : IMyPasswordsService
    {
        private readonly IMapper _mapper;
        private readonly IDbContextEntity _entity;
        private readonly ICategoryService _categoryService;

        public MyPasswordsManager(IMapper mapper, IDbContextEntity entity, ICategoryService categoryService)
        {
            _mapper = mapper;
            _entity = entity;
            _categoryService = categoryService;
        }

        public async Task<ResponseDto<MyPasswordDto>> Create(MyPasswordDto myPasswordDto)
        {
            var rsp = new ResponseDto<MyPasswordDto>();
            var myPassword = await _entity.MYPASSWORDS.FirstOrDefaultAsync(x => x.NAME == myPasswordDto.NAME && x.CATEGORYID == myPasswordDto.CATEGORYID);
            if (myPassword == null)
            {
                var newMyPassword = _mapper.Map<MYPASSWORDS>(myPasswordDto);
                _entity.MYPASSWORDS.Add(newMyPassword);
                await _entity.SaveChangesAsync();

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Şifre başarıyla eklendi.";
                rsp.Data = myPasswordDto;
            }

            return rsp;
        }

        public async Task<ResponseDto<MyPasswordDto>> Update(MyPasswordDto myPasswordDto)
        {
            var rsp = new ResponseDto<MyPasswordDto>();
            var myPassword = await _entity.MYPASSWORDS.FirstOrDefaultAsync(x => x.ID == myPasswordDto.ID);
            if (myPassword != null)
            {
                myPassword.NAME = myPasswordDto.NAME;
                myPassword.USERNAME = myPasswordDto.USERNAME;
                myPassword.PASSWORD = myPasswordDto.PASSWORD;
                myPassword.CATEGORYID = myPasswordDto.CATEGORYID;
                myPassword.URL = myPasswordDto.URL;

                await _entity.SaveChangesAsync();

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Değişiklikler başarıyla kaydedildi.";
                rsp.Data = myPasswordDto;
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kayıt bulunamadı.";
            }

            return rsp;
        }

        public async Task<ResponseDto<int>> Delete(MyPasswordDto myPasswordDto)
        {
            var rsp = new ResponseDto<int>();
            var myPassword = await _entity.MYPASSWORDS.FirstOrDefaultAsync(x => x.ID == myPasswordDto.ID);
            if (myPassword != null)
            {
                _entity.MYPASSWORDS.Remove(myPassword);
                await _entity.SaveChangesAsync();

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = $"{myPassword.NAME} isimli şifre başarıyla silindi.";
                rsp.Data = 1;
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.SuccessMessage = "Böyle bir kayıt bulunamadı.";
            }

            return rsp;
        }

        public async Task<ResponseDto<List<MyPasswordDto>>> GetAll()
        {
            var rsp = new ResponseDto<List<MyPasswordDto>>();
            var myPasswords = await _entity.MYPASSWORDS.ToListAsync();

            if (myPasswords.Count > 0)
            {
                var listDto = _mapper.Map<List<MyPasswordDto>>(myPasswords);

                foreach (var item in listDto)
                {
                    var categoryDto = new CategoryDto();
                    categoryDto.CATEGORYID = item.CATEGORYID;
                    var categoryRspDto = await _categoryService.Get(categoryDto);
                    item.CATEGORYNAME = categoryRspDto.Data.CATEGORYNAME;
                }

                rsp.Data = listDto;
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = $"Toplamda {myPasswords.Count} adet kayıt listelendi";
            }
            else
            {
                rsp.Data = new List<MyPasswordDto>();
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Sistemde tanımlı bir şifre yok.";
            }

            return rsp;

        }


        public async Task<ResponseDto<MyPasswordDto>> Get(MyPasswordDto dto)
        {
            var rsp = new ResponseDto<MyPasswordDto>();
            var myPassword = await _entity.MYPASSWORDS.FirstOrDefaultAsync(x => x.ID == dto.ID);
            if (myPassword != null)
            {
                rsp.Data = _mapper.Map<MyPasswordDto>(myPassword);
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Şifre kaydı alındı";
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kayıt bulunamadı";
            }

            return rsp;
        }


    }
}
