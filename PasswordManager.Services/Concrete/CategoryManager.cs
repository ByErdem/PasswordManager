﻿using AutoMapper;
using PasswordManager.Data;
using PasswordManager.Entity.Concrete;
using PasswordManager.Entity.Dtos;
using PasswordManager.Services.Abstract;
using PasswordManager.Shared.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly IDbContextEntity _entity;

        public CategoryManager(IMapper mapper, IDbContextEntity entity)
        {
            _mapper = mapper;
            _entity = entity;
        }

        public async Task<ResponseDto<CategoryDto>> Create(CategoryDto categoryDto)
        {
            var rsp = new ResponseDto<CategoryDto>();

            var category = await _entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYNAME == categoryDto.CATEGORYNAME);
            if (category == null)
            {
                var newCategory = _mapper.Map<CATEGORY>(categoryDto);

                newCategory.CREATORUSERID = -1;
                newCategory.CREATEDDATE = DateTime.Now;
                newCategory.ISDELETED = false;

                _entity.CATEGORY.Add(newCategory);
                await _entity.SaveChangesAsync();

                var newCategoryDto = _mapper.Map<CategoryDto>(newCategory);

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Kategori başarıyla eklendi.";
                rsp.Data = newCategoryDto;
            }
            else
            {
                if (category.ISDELETED.Value)
                {
                    var newCategoryDto = _mapper.Map<CategoryDto>(category);

                    category.ISDELETED = false;

                    await _entity.SaveChangesAsync();

                    rsp.ResultStatus = ResultStatus.Success;
                    rsp.SuccessMessage = "Kategori başarıyla eklendi.";
                    rsp.Data = newCategoryDto;
                }
                else
                {
                    rsp.ErrorMessage = $"{categoryDto.CATEGORYNAME} isimli kategori sistemde tanımlıdır.";
                    rsp.ResultStatus = ResultStatus.Error;
                    rsp.Data = _mapper.Map<CategoryDto>(category);
                }
            }

            return rsp;
        }

        public async Task<ResponseDto<CategoryDto>> Update(CategoryDto categoryDto)
        {
            var rsp = new ResponseDto<CategoryDto>();

            var category = await _entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == categoryDto.CATEGORYID);
            if (category != null)
            {
                category.CATEGORYNAME = categoryDto.CATEGORYNAME;

                await _entity.SaveChangesAsync();

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Değişiklikler başarıyla kaydedildi.";
                rsp.Data = categoryDto;
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kategori bulunamadı.";
            }

            return rsp;
        }

        public async Task<ResponseDto<int>> Delete(MCategory categoryDto)
        {
            var rsp = new ResponseDto<int>();

            var category = await _entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == categoryDto.CATEGORYID);
            if (category != null)
            {
                category.ISDELETED = true;
                await _entity.SaveChangesAsync();

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = $"{category.CATEGORYNAME} isimli kategori başarıyla silindi.";
                rsp.Data = 1;
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kategori bulunamadı.";
            }

            return rsp;
        }

        public async Task<ResponseDto<int>> HardDelete(MCategory categoryDto)
        {
            var rsp = new ResponseDto<int>();

            var category = await _entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == categoryDto.CATEGORYID);
            if (category != null)
            {
                _entity.CATEGORY.Remove(category);
                await _entity.SaveChangesAsync();

                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = $"{categoryDto.CATEGORYNAME} isimli kategori sistemden başarıyla silindi.";
                rsp.Data = 1;
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kategori bulunamadı.";
            }

            return rsp;
        }

        public async Task<ResponseDto<CategoryDto>> Get(CategoryDto dto)
        {
            var rsp = new ResponseDto<CategoryDto>();
            var category = await _entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == dto.CATEGORYID);
            if (category != null)
            {
                rsp.Data = _mapper.Map<CategoryDto>(category);
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = "Kategori kaydı alındı";
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Böyle bir kayıt bulunamadı";
            }

            return rsp;
        }

        public async Task<ResponseDto<List<CategoryDto>>> GetAll()
        {
            var rsp = new ResponseDto<List<CategoryDto>>();
            var categories = await _entity.CATEGORY.Where(x => x.ISDELETED == false).ToListAsync();

            if (categories.Count > 0)
            {
                rsp.Data = _mapper.Map<List<CategoryDto>>(categories);
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = $"Toplamda {categories.Count} adet kategori listelendi";
            }
            else
            {
                rsp.Data = new List<CategoryDto>();
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Sistemde tanımlı bir kategori yok.";
            }

            return rsp;
        }
    }
}
