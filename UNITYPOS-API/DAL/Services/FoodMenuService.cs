using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class FoodMenuService : IFoodmenu
    {
        private readonly IUnitOfWork _uow;
        public FoodMenuService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<object> GetAllMenu(int orgid)
        {
            var result = (from b in _uow.GenericRepository<FoodMenu>().Table()

                          join c in _uow.GenericRepository<FoodMenuCategory>().Table()
                          on b.CategoryId equals c.Id

                          join sc in _uow.GenericRepository<FoodMenuSubCategory>().Table()
                          on b.SubCategoryId equals sc.Id

                          join o in _uow.GenericRepository<Organization>().Table()
                          on b.OrgId equals o.Id

                          where b.IsDeleted == false
                          && (orgid == 0 || b.OrgId == orgid)

                          select new
                          {
                              id = b.Id,

                              organizationname = o.Name,

                              name = b.Name,
                              code = b.Code,
                              price = b.Price,

                              categoryId = c.Id,
                              categoryname = c.Name,

                              subCategoryId = sc.Id,
                              subCategoryName = sc.Name,

                              isactive = b.IsActive,
                          }).ToList();

            return result;
        }
        public IEnumerable<Object> GetMenubyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<FoodMenu>().Table()
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          categoryId = b.CategoryId,
                          price = b.Price,
                          isactive = b.IsActive,
                      }).ToList();


            return result;
        }


        public string Create(FoodMenu foodmenu)
        {
            int check = _uow.GenericRepository<FoodMenu>().Table()
                .Count(b => b.Name.Trim().ToLower() == foodmenu.Name.Trim().ToLower()
                         && b.OrgId == foodmenu.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new FoodMenu
            {
                Code = foodmenu.Code,
                Name = foodmenu.Name,
                CategoryId = foodmenu.CategoryId,
                Price = foodmenu.Price,
                OrgId = foodmenu.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = foodmenu.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<FoodMenu>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(FoodMenu foodmenu)
        {
            int check = _uow.GenericRepository<FoodMenu>().Table()
                .Count(b => (b.Name.Trim().ToLower() == foodmenu.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == foodmenu.Code.Trim().ToLower())
                         && b.Id != foodmenu.Id
                         && b.OrgId == foodmenu.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingMenu = _uow.GenericRepository<FoodMenu>().Table()
                .FirstOrDefault(x => x.Id == foodmenu.Id
                                  && x.OrgId == foodmenu.OrgId
                                  && x.IsDeleted == false);

            if (existingMenu != null)
            {
                existingMenu.Code = foodmenu.Code;
                existingMenu.Name = foodmenu.Name;
                existingMenu.Price = foodmenu.Price;
                existingMenu.CategoryId = foodmenu.CategoryId;
                existingMenu.OrgId = foodmenu.OrgId;
                existingMenu.IsActive = true;
                existingMenu.IsDeleted = false;
                existingMenu.UpdatedBy = foodmenu.UpdatedBy;
                existingMenu.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<FoodMenu>().Update(existingMenu);
                _uow.Save();

                return Convert.ToString(existingMenu.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<FoodMenu>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<FoodMenu>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<FoodMenu>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<FoodMenu>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }
    }
}
