using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class EntityMasterService : IEntityMasterService
    {
        private readonly IUnitOfWork _uow;

        public EntityMasterService(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public IEnumerable<EntityRoleRights> GetEntityMasterForRoleRights(int orgId, int roleId)
        {
            var result = (
                from menu in _uow.GenericRepository<Menu>().Table()
                join subMenu in _uow.GenericRepository<SubMenu>().Table()
                    on menu.Id equals subMenu.MenuId
                join entity in _uow.GenericRepository<EntityMaster>().Table()
                    on subMenu.EntityNo equals entity.EntityNo
                join rolePermission in _uow.GenericRepository<RolePermission>().Table()
                        .Where(x => x.OrgId == orgId
                                 && x.RoleId == roleId
                                 && x.IsDeleted == false)
                    on entity.EntityNo equals rolePermission.EntityNo into permissionGroup
                from permission in permissionGroup.DefaultIfEmpty()
                where menu.IsActive == true
                   && subMenu.IsActive == true
                   && entity.IsActive == true
                   && menu.IsDeleted == false
                   && subMenu.IsDeleted == false
                   && entity.IsDeleted == false
                orderby menu.Id ascending
                select new EntityRoleRights
                {
                    OrgId = orgId,
                    RoleId = roleId,
                    MenuId = menu.Id,
                    MenuName = menu.Name,
                    SubMenuId = subMenu.Id,
                    SubMenuName = subMenu.Name,
                    EntityNo = entity.EntityNo,
                    EntityName = entity.Name,
                    Create = permission != null && permission.Create,
                    Edit = permission != null && permission.Edit,
                    Delete = permission != null && permission.Delete,
                    View = permission != null && permission.View,
                    Download = permission != null && permission.Download,
                    Print = permission != null && permission.Print,
                    ActiveInActive = permission !=null && permission.ActiveInActive
                })
                .ToList();

            return result;
        }

        public string SaveRolePermission(List<RolePermission> rolePermissions)
        {
            if (rolePermissions == null || rolePermissions.Count == 0)
            {
                return string.Empty;
            }

            foreach (var permission in rolePermissions)
            {
                var existingPermission = _uow.GenericRepository<RolePermission>()
                    .Table()
                    .FirstOrDefault(x => x.OrgId == permission.OrgId
                                      && x.RoleId == permission.RoleId
                                      && x.EntityNo == permission.EntityNo);

                if (existingPermission != null)
                {
                    existingPermission.OrgId = permission.OrgId;
                    existingPermission.RoleId = permission.RoleId;
                    existingPermission.EntityNo = permission.EntityNo;
                    existingPermission.Create = permission.Create;
                    existingPermission.Edit = permission.Edit;
                    existingPermission.Delete = permission.Delete;
                    existingPermission.View = permission.View;
                    existingPermission.Download = permission.Download;
                    existingPermission.Print = permission.Print;
                    existingPermission.ActiveInActive = permission.ActiveInActive;
                    existingPermission.IsActive = true;
                    existingPermission.IsDeleted = false;
                    existingPermission.UpdatedBy = permission.UpdatedBy ?? permission.CreatedBy;
                    existingPermission.UpdatedDate = DateTime.Now;

                    _uow.GenericRepository<RolePermission>().Update(existingPermission);
                }
                else
                {
                    var entity = new RolePermission
                    {
                        OrgId = permission.OrgId,
                        RoleId = permission.RoleId,
                        EntityNo = permission.EntityNo,
                        Create = permission.Create,
                        Edit = permission.Edit,
                        Delete = permission.Delete,
                        View = permission.View,
                        Download = permission.Download,
                        Print = permission.Print,
                        ActiveInActive = permission.ActiveInActive,
                        IsActive = true,
                        IsDeleted = false,
                        CreatedBy = permission.CreatedBy,
                        CreatedDate = DateTime.Now
                    };

                    _uow.GenericRepository<RolePermission>().Insert(entity);
                }
            }

            _uow.Save();

            return "Success";
        }
    }
}
