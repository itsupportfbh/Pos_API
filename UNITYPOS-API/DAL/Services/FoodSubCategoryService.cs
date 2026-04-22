using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class FoodSubCategoryService : IFoodSubCategory
    {
        private readonly IUnitOfWork _uow;
        public FoodSubCategoryService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<Object> GetAllSubCategory(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<FoodMenuSubCategory>().Table()
                      where b.IsDeleted == false && b.OrgId == orgid
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          isactive = b.IsActive,
                      }).ToList();


            return result;
        }
        public IEnumerable<Object> GetSubCategorybyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<FoodMenuSubCategory>().Table()
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          isactive = b.IsActive,
                      }).ToList();


            return result;
        }


        public string Create(FoodMenuSubCategory foodmenu)
        {
            int check = _uow.GenericRepository<FoodMenuSubCategory>().Table()
                .Count(b => b.Name.Trim().ToLower() == foodmenu.Name.Trim().ToLower()
                         && b.OrgId == foodmenu.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new FoodMenuSubCategory
            {
                Code = foodmenu.Code,
                Name = foodmenu.Name,
                CategoryId = foodmenu.CategoryId,
                OrgId = foodmenu.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = foodmenu.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<FoodMenuSubCategory>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(FoodMenuSubCategory foodmenu)
        {
            int check = _uow.GenericRepository<FoodMenuSubCategory>().Table()
                .Count(b => (b.Name.Trim().ToLower() == foodmenu.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == foodmenu.Code.Trim().ToLower())
                         && b.Id != foodmenu.Id
                         && b.OrgId == foodmenu.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingSubCate = _uow.GenericRepository<FoodMenuSubCategory>().Table()
                .FirstOrDefault(x => x.Id == foodmenu.Id
                                  && x.OrgId == foodmenu.OrgId
                                  && x.IsDeleted == false);

            if (existingSubCate != null)
            {
                existingSubCate.Code = foodmenu.Code;
                existingSubCate.Name = foodmenu.Name;
                existingSubCate.CategoryId  = foodmenu.CategoryId;
                existingSubCate.OrgId = foodmenu.OrgId;
                existingSubCate.IsActive = true;
                existingSubCate.IsDeleted = false;
                existingSubCate.UpdatedBy = foodmenu.UpdatedBy;
                existingSubCate.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<FoodMenuSubCategory>().Update(existingSubCate);
                _uow.Save();

                return Convert.ToString(existingSubCate.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<FoodMenuSubCategory>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<FoodMenuSubCategory>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<FoodMenuSubCategory>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<FoodMenuSubCategory>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }
    }
}
