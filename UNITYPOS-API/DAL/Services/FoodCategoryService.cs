using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class FoodCategoryService : IFoodCategory
    {
        private readonly IUnitOfWork _uow;
        public FoodCategoryService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<Object> GetAllCategory(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<FoodMenuCategory>().Table()
                      join o in _uow.GenericRepository<Organization>().Table()
                         on b.OrgId equals o.Id
                      where b.IsDeleted == false && (orgid == 0 || b.OrgId == orgid)
                      select new
                      {
                          id = b.Id,
                          organizationname = o.Name,
                          name = b.Name,
                          code = b.Code,
                          isactive = b.IsActive,
                      }).ToList();


            return result;
        }
        public IEnumerable<Object> GetCategorybyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<FoodMenuCategory>().Table()
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


        public string Create(FoodMenuCategory foodmenu)
        {
            int check = _uow.GenericRepository<FoodMenuCategory>().Table()
                .Count(b => b.Name.Trim().ToLower() == foodmenu.Name.Trim().ToLower()
                         && b.OrgId == foodmenu.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new FoodMenuCategory
            {
                Code = foodmenu.Code,
                Name = foodmenu.Name,
                OrgId = foodmenu.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = foodmenu.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<FoodMenuCategory>().Insert(entity);

            var codeTemplate = _uow.GenericRepository<CodeTemplate>().Table()
                             .FirstOrDefault(x => x.EntityNo == foodmenu.EntityNo
                                               && x.OrgId == foodmenu.OrgId
                                               && x.BranchId == foodmenu.BranchId
                                               );

            if (codeTemplate != null)
            {
                codeTemplate.CurrentValue = codeTemplate.CurrentValue + 1;
                _uow.GenericRepository<CodeTemplate>().Update(codeTemplate);
            }

            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(FoodMenuCategory foodmenu)
        {
            int check = _uow.GenericRepository<FoodMenuCategory>().Table()
                .Count(b => (b.Name.Trim().ToLower() == foodmenu.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == foodmenu.Code.Trim().ToLower())
                         && b.Id != foodmenu.Id
                         && b.OrgId == foodmenu.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingCategory = _uow.GenericRepository<FoodMenuCategory>().Table()
                .FirstOrDefault(x => x.Id == foodmenu.Id
                                  && x.OrgId == foodmenu.OrgId
                                  && x.IsDeleted == false);

            if (existingCategory != null)
            {
                existingCategory.Code = foodmenu.Code;
                existingCategory.Name = foodmenu.Name;
                existingCategory.OrgId = foodmenu.OrgId;
                existingCategory.IsActive = true;
                existingCategory.IsDeleted = false;
                existingCategory.UpdatedBy = foodmenu.UpdatedBy;
                existingCategory.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<FoodMenuCategory>().Update(existingCategory);
                _uow.Save();

                return Convert.ToString(existingCategory.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<FoodMenuCategory>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<FoodMenuCategory>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<FoodMenuCategory>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<FoodMenuCategory>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }
    }
}
