using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities;
using UNITYPOS_API.ViewModel;

namespace UNITYPOS_API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class OrganizationController : ControllerBase
    {

        private readonly IOrganizationservice _organizationservice;
        public OrganizationController(IOrganizationservice organizationservice)
        {
            _organizationservice = organizationservice;
        }

        [HttpGet]
        public List<Organization> GetAllOrganization()
        {
            return _organizationservice.GetAllOrganization();
        }

        [HttpGet]
        public Organization GetOrganizationById(int Id)
        {
            return _organizationservice.GetOrganizationById(Id);
        }

        [HttpPost]
        public Organization AddUpdateOrganization(OrganizationDTO organizationDTO)
        {
            return _organizationservice.AddUpdateOrganization(organizationDTO);
        }

        [HttpDelete]
        public Organization DeleteOrganizationById(int id)
        {
            return _organizationservice.DeleteOrganizationById(id);
        }
    }
}
