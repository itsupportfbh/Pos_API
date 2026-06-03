using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class ComboMenuService:IComboMenu
    {


        private readonly IUnitOfWork _uow;
        public ComboMenuService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        //public IEnumerable<object> GetAllComboMenu(int orgid)
        //{
        //    var result = (from c in _uow.GenericRepository<ComboMenu>().Table()
        //                  join cat in _uow.GenericRepository<FoodMenuCategory>().Table()
        //                      on c.CategoryId equals cat.Id
        //                  join sub in _uow.GenericRepository<FoodMenuSubCategory>().Table()
        //                      on c.SubCategoryId equals sub.Id into subJoin
        //                  from sub in subJoin.DefaultIfEmpty()
        //                  where c.IsDeleted == false
        //                        && (orgid == 0 || c.OrgId == orgid)
        //                  select new
        //                  {
        //                      c.Id,
        //                      c.Code,
        //                      c.Name,
        //                      c.CategoryId,
        //                      CategoryName = cat.Name,
        //                      c.SubCategoryId,
        //                      SubCategoryName = sub != null ? sub.Name : "",
        //                      c.Price,
        //                      c.OrgId,
        //                      c.IsActive,

        //                      ComboMenuItems =
        //                          (from item in _uow.GenericRepository<ComboMenuItem>().Table()
        //                           join fm in _uow.GenericRepository<FoodMenu>().Table()
        //                               on item.FoodMenuId equals fm.Id
        //                           where item.ComboMenuId == c.Id
        //                                 && item.IsDeleted == false
        //                           select new
        //                           {
        //                               item.Id,
        //                               item.ComboMenuId,
        //                               item.FoodMenuId,
        //                               FoodMenuName = fm.Name,
        //                               item.Qty,
        //                               item.Price,
        //                               item.IsActive
        //                           }).ToList(),

        //                      ItemsCount = _uow.GenericRepository<ComboMenuItem>().Table()
        //                          .Count(x => x.ComboMenuId == c.Id && x.IsDeleted == false)
        //                  }).ToList();

        //    return result;
        //}



        public IEnumerable<object> GetAllComboMenu(int orgid)
        {
            var comboMenus = _uow.GenericRepository<ComboMenu>().Table()
                .Where(c => c.IsDeleted == false && (orgid == 0 || c.OrgId == orgid))
                .ToList();

            var categories = _uow.GenericRepository<FoodMenuCategory>().Table().ToList();
            var subCategories = _uow.GenericRepository<FoodMenuSubCategory>().Table().ToList();
            var items = _uow.GenericRepository<ComboMenuItem>().Table()
                .Where(x => x.IsDeleted == false)
                .ToList();
            var foodMenus = _uow.GenericRepository<FoodMenu>().Table().ToList();

            var result = comboMenus.Select(c => new
            {
                c.Id,
                c.Code,
                c.Name,
              //  c.CategoryId,
              //  CategoryName = categories.FirstOrDefault(x => x.Id == c.CategoryId)?.Name ?? "",
               // c.SubCategoryId,
               // SubCategoryName = subCategories.FirstOrDefault(x => x.Id == c.SubCategoryId)?.Name ?? "",
                c.Price,
                c.OrgId,
                c.IsActive,

                ComboMenuItems = items
                    .Where(i => i.ComboMenuId == c.Id)
                    .Select(i => new
                    {
                        i.Id,
                        i.ComboMenuId,
                        i.FoodMenuId,
                        FoodMenuName = foodMenus.FirstOrDefault(f => f.Id == i.FoodMenuId)?.Name ?? "",
                        i.Qty,
                        i.IsActive
                    })
                    .ToList(),

                ItemsCount = items.Count(i => i.ComboMenuId == c.Id)
            }).ToList();

            return result;
        }
        public object GetComboMenubyId(int id)
        {
            var combo = _uow.GenericRepository<ComboMenu>()
                .Table()
                .Where(x => x.Id == id && x.IsDeleted == false)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.CategoryId,
                    x.SubCategoryId,
                    x.Price,
                    x.OrgId,
                    x.BranchId,
                    x.IsActive
                })
                .FirstOrDefault();

            if (combo == null)
                return null;

            var items = (from i in _uow.GenericRepository<ComboMenuItem>().Table()
                         join f in _uow.GenericRepository<FoodMenu>().Table()
                             on i.FoodMenuId equals f.Id
                         where i.ComboMenuId == id
                               && i.IsDeleted == false
                         select new
                         {
                             i.Id,
                             i.ComboMenuId,
                             i.FoodMenuId,
                             FoodMenuName = f.Name,
                             i.Qty,
                             i.Price,
                             i.OrgId,
                             i.IsActive
                         }).ToList();

            return new
            {
                combo.Id,
                combo.Code,
                combo.Name,
                combo.CategoryId,
                combo.SubCategoryId,
                combo.Price,
                combo.OrgId,
                combo.IsActive,
                ComboMenuItems = items
            };
        }

        public string Create(ComboMenu comboMenu)
        {
            int check = _uow.GenericRepository<ComboMenu>().Table()
                .Count(x => x.Name.Trim().ToLower() == comboMenu.Name.Trim().ToLower()
                         && x.OrgId == comboMenu.OrgId
                         && x.IsDeleted == false);

            if (check > 0)
                return "AlreadyExists";

            var entity = new ComboMenu
            {
                Code = comboMenu.Code,
                Name = comboMenu.Name,
                CategoryId = comboMenu.CategoryId,
                SubCategoryId = comboMenu.SubCategoryId, // this is correct
                Price = comboMenu.Price,
                OrgId = comboMenu.OrgId,
                BranchId=comboMenu.BranchId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = comboMenu.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<ComboMenu>().Insert(entity);
            _uow.Save();

            foreach (var item in comboMenu.ComboMenuItems ?? new List<ComboMenuItem>())
            {
                var itemEntity = new ComboMenuItem
                {
                    ComboMenuId = entity.Id,
                    FoodMenuId = item.FoodMenuId,
                    Qty = item.Qty,
                    Price = item.Price,
                    OrgId = comboMenu.OrgId,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = comboMenu.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                _uow.GenericRepository<ComboMenuItem>().Insert(itemEntity);
            }

            _uow.Save();

            return entity.Id.ToString();
        }

        public string Update(ComboMenu comboMenu)
        {
            int check = _uow.GenericRepository<ComboMenu>().Table()
                .Count(x => x.Name.Trim().ToLower() == comboMenu.Name.Trim().ToLower()
                         && x.Id != comboMenu.Id
                         && x.OrgId == comboMenu.OrgId
                         && x.IsDeleted == false);

            if (check > 0)
                return "AlreadyExists";

            var existingCombo = _uow.GenericRepository<ComboMenu>().Table()
                .FirstOrDefault(x => x.Id == comboMenu.Id
                                  && x.OrgId == comboMenu.OrgId
                                  && x.IsDeleted == false);

            if (existingCombo == null)
                return "";

            existingCombo.Code = comboMenu.Code;
            existingCombo.Name = comboMenu.Name;
            existingCombo.CategoryId = comboMenu.CategoryId;
            existingCombo.SubCategoryId = comboMenu.SubCategoryId;
            existingCombo.Price = comboMenu.Price;
            existingCombo.OrgId = comboMenu.OrgId;
            existingCombo.IsActive = true;
            existingCombo.IsDeleted = false;
            existingCombo.UpdatedBy = comboMenu.UpdatedBy;
            existingCombo.UpdatedDate = DateTime.Now;

            _uow.GenericRepository<ComboMenu>().Update(existingCombo);
            _uow.Save();

            var oldItems = _uow.GenericRepository<ComboMenuItem>().Table()
                .Where(x => x.ComboMenuId == comboMenu.Id && x.IsDeleted == false)
                .ToList();

            foreach (var oldItem in oldItems)
            {
                oldItem.IsDeleted = true;
                oldItem.IsActive = false;
                oldItem.UpdatedBy = comboMenu.UpdatedBy;
                oldItem.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<ComboMenuItem>().Update(oldItem);
            }

            _uow.Save();

            if (comboMenu.ComboMenuItems != null && comboMenu.ComboMenuItems.Any())
            {
                foreach (var item in comboMenu.ComboMenuItems)
                {
                    var itemEntity = new ComboMenuItem
                    {
                        ComboMenuId = existingCombo.Id,
                        FoodMenuId = item.FoodMenuId,
                        Qty = item.Qty,
                        Price = item.Price,
                        OrgId = comboMenu.OrgId,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = comboMenu.UpdatedBy ?? comboMenu.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<ComboMenuItem>().Insert(itemEntity);
                }

                _uow.Save();
            }

            return Convert.ToString(existingCombo.Id);
        }

        public string DeleteById(int id)
        {
            var combo = _uow.GenericRepository<ComboMenu>().Table()
                .FirstOrDefault(x => x.Id == id);

            if (combo != null)
            {
                combo.IsDeleted = true;
                combo.IsActive = false;
                combo.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<ComboMenu>().Update(combo);

                var items = _uow.GenericRepository<ComboMenuItem>().Table()
                    .Where(x => x.ComboMenuId == id)
                    .ToList();

                foreach (var item in items)
                {
                    item.IsDeleted = true;
                    item.IsActive = false;
                    item.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<ComboMenuItem>().Update(item);
                }

                _uow.Save();
            }

            return Convert.ToString(combo?.Id ?? 0);
        }

        public string ActiveInActive(int id, bool isActive)
        {
            var combo = _uow.GenericRepository<ComboMenu>().Table()
                .FirstOrDefault(x => x.Id == id);

            if (combo != null)
            {
                combo.IsActive = isActive;
                combo.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<ComboMenu>().Update(combo);
                _uow.Save();
            }

            return Convert.ToString(combo?.Id ?? 0);
        }


    }
}
