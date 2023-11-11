using AutoMapper;
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

        public CategoryManager(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<ResponseDto<CategoryDto>> Create(CategoryDto categoryDto)
        {
            var rsp = new ResponseDto<CategoryDto>();
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var category = await entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYNAME == categoryDto.CATEGORYNAME);
            if (category == null)
            {
                var newCategory = _mapper.Map<CATEGORY>(categoryDto);
                if (categoryDto.PARENTCATEGORYID == -1)
                {
                    newCategory.PARENTCATEGORYID = null;
                }
                else
                {
                    newCategory.PARENTCATEGORYID = categoryDto.PARENTCATEGORYID;
                }

                newCategory.CREATORUSERID = 1;
                newCategory.CREATEDDATE = DateTime.Now;
                newCategory.ISDELETED = false;

                entity.CATEGORY.Add(newCategory);
                await entity.SaveChangesAsync();

                var newCategoryDto = _mapper.Map<CategoryDto>(newCategory);

                if (newCategoryDto.PARENTCATEGORYID != -1 && newCategoryDto.PARENTCATEGORYID != 0)
                {
                    newCategoryDto.PARENTCATEGORYNAME = await FindParentCategoryName((int)newCategory.PARENTCATEGORYID);
                }

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

                    if (categoryDto.PARENTCATEGORYID > 0)
                    {
                        category.PARENTCATEGORYID = categoryDto.PARENTCATEGORYID;
                        newCategoryDto.PARENTCATEGORYID = categoryDto.PARENTCATEGORYID;
                        newCategoryDto.PARENTCATEGORYNAME = await FindParentCategoryName(categoryDto.PARENTCATEGORYID);
                    }

                    await entity.SaveChangesAsync();

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
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var category = await entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == categoryDto.CATEGORYID);
            if (category != null)
            {
                category.CATEGORYNAME = categoryDto.CATEGORYNAME;

                if (categoryDto.PARENTCATEGORYID != -1 && categoryDto.PARENTCATEGORYID != 0)
                {
                    category.PARENTCATEGORYID = categoryDto.PARENTCATEGORYID;
                    categoryDto.PARENTCATEGORYNAME = await FindParentCategoryName(categoryDto.PARENTCATEGORYID);
                }
                else
                {
                    category.PARENTCATEGORYID = null;
                    categoryDto.PARENTCATEGORYNAME = "";
                }

                await entity.SaveChangesAsync();

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
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var category = await entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == categoryDto.CATEGORYID);
            if (category != null)
            {
                category.ISDELETED = true;
                await entity.SaveChangesAsync();

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
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var category = await entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == categoryDto.CATEGORYID);
            if (category != null)
            {
                entity.CATEGORY.Remove(category);
                await entity.SaveChangesAsync();

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
            PasswordManagerEntities entity = new PasswordManagerEntities();
            var category = await entity.CATEGORY.FirstOrDefaultAsync(x => x.CATEGORYID == dto.CATEGORYID);
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

        public async Task<string> FindParentCategoryName(int CATEGORYID)
        {
            var categoryDto = new CategoryDto
            {
                CATEGORYID = CATEGORYID
            };

            var category = await Get(categoryDto);
            return category.Data.CATEGORYNAME;
        }

        public async Task<ResponseDto<List<CategoryDto>>> GetMainCategories()
        {
            var rsp = new ResponseDto<List<CategoryDto>>();
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var categories = await entity.CATEGORY.Where(x => x.ISDELETED == false && x.PARENTCATEGORYID == null).ToListAsync();

            if (categories.Count > 0)
            {
                rsp.Data = _mapper.Map<List<CategoryDto>>(categories);

                for (int i = 0; i < rsp.Data.Count; i++)
                {
                    if (rsp.Data[i].PARENTCATEGORYID > 0)
                    {
                        rsp.Data[i].PARENTCATEGORYNAME = await FindParentCategoryName(rsp.Data[i].PARENTCATEGORYID);
                    }
                }

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

        public async Task<ResponseDto<List<CategoryDto>>> GetSubCategories(int CategoryId)
        {
            var rsp = new ResponseDto<List<CategoryDto>>();
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var categories = await entity.CATEGORY.Where(x => x.ISDELETED == false && x.PARENTCATEGORYID != null && x.PARENTCATEGORYID == CategoryId).ToListAsync();

            if (categories.Count > 0)
            {
                rsp.Data = _mapper.Map<List<CategoryDto>>(categories);

                for (int i = 0; i < rsp.Data.Count; i++)
                {
                    if (rsp.Data[i].PARENTCATEGORYID > 0)
                    {
                        rsp.Data[i].PARENTCATEGORYNAME = await FindParentCategoryName(rsp.Data[i].PARENTCATEGORYID);
                    }
                }

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

        public async Task<ResponseDto<List<CategoryDto>>> GetAll()
        {
            var rsp = new ResponseDto<List<CategoryDto>>();
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var categories = await entity.CATEGORY.Where(x => x.ISDELETED == false).ToListAsync();

            if (categories.Count > 0)
            {
                rsp.Data = _mapper.Map<List<CategoryDto>>(categories);

                for (int i = 0; i < rsp.Data.Count; i++)
                {
                    if (rsp.Data[i].PARENTCATEGORYID > 0)
                    {
                        rsp.Data[i].PARENTCATEGORYNAME = await FindParentCategoryName(rsp.Data[i].PARENTCATEGORYID);
                    }
                }

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

        public async Task<ResponseDto<List<MCategory>>> GetAllByParentID(MCategory categoryDto)
        {
            var rsp = new ResponseDto<List<MCategory>>();
            PasswordManagerEntities entity = new PasswordManagerEntities();

            var categories = await entity.CATEGORY.Where(x => x.PARENTCATEGORYID == categoryDto.PARENTCATEGORYID).ToListAsync();

            if (categories.Count > 0)
            {
                rsp.Data = _mapper.Map<List<MCategory>>(categories);
                rsp.ResultStatus = ResultStatus.Success;
                rsp.SuccessMessage = $"Toplamda {categories.Count} adet kategori listelendi";
            }
            else
            {
                rsp.ResultStatus = ResultStatus.Error;
                rsp.ErrorMessage = "Sistemde tanımlı bir kategori yok.";
            }

            return rsp;
        }
    }
}
