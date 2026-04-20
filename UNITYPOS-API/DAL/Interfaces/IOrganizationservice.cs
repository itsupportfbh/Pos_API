using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrganizationservice
    {
        public IEnumerable<Object> GetAllOrganization();
        public Organization GetById(int Id);
        public String Create(OrganizationDTO organizationDTO);
        public String Update(OrganizationDTO organizationDTO);
        public string Delete(int Id);
        public string ActiveInActive(int Id, bool IsActive);

    }
}
