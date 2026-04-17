using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrganizationservice
    {
        public String Create(OrganizationDTO organizationDTO);
        public String Update(OrganizationDTO organizationDTO);
        public IEnumerable<Object> GetAllOrganization();
        public Organization GetById(int Id);
        public string DeleteById(int Id);
        public string ActiveInActive(int Id, bool IsActive);

    }
}
