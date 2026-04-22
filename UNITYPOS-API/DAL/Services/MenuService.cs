using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _uow;
        public MenuService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }


        public IEnumerable<object> GetAllMenuAndSubMenu()
        {
            var result = (from m in _uow.GenericRepository<Menu>().Table()
                          where m.IsActive == true
                                && m.IsDeleted == false

                          orderby m.DisplayOrder ascending

                          select new
                          {
                              MenuId = m.Id,
                              MenuName = m.Name,
                              IsActive = m.IsActive,

                              SubMenus = (from sm in _uow.GenericRepository<SubMenu>().Table()
                                          where sm.MenuId == m.Id
                                                && sm.IsActive == true
                                                && sm.IsDeleted == false

                                          orderby sm.DisplayOrder ascending

                                          select new
                                          {
                                              SubMenuId = sm.Id,
                                              SubMenuName = sm.Name,
                                              sm.EntityNo,
                                              sm.MenuId,
                                              sm.Route,
                                              sm.Remarks,
                                              IsActive = sm.IsActive
                                          }).ToList()
                          }).ToList();

            return result;
        }



    }
}
