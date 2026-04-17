using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IOrganizationservice
    {

        public List<Organization> GetAllOrganization();
        public Organization GetOrganizationById(int Id);
        public Organization AddUpdateOrganization(OrganizationDTO organizationDTO);
        public Organization DeleteOrganizationById(int id);
    }
}
