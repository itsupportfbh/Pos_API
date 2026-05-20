using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Entities.Master;
using UNITYPOS_API.Entities;

namespace UNITYPOS_API.Controllers

{
    
        [Route("[controller]/[action]")]
        [ApiController]
        [Authorize]
        public class EmployeeController : ControllerBase
        {
            private readonly IEmployeeMasterService _employeeMasterService;

            public EmployeeController(IEmployeeMasterService employeeService)
            {
                _employeeMasterService = employeeService;
            }

            [HttpGet]
            public string GetAllEmployee(int orgId, string branchId)
            {
                string result = null;

                result = JsonConvert.SerializeObject(
                    _employeeMasterService.GetAllEmployee(orgId, branchId));

                return Common.Utility.GetResult(result);
            }

            [HttpGet]
            public string GetEmployeebyId(int id)
            {
                string result = null;

                result = JsonConvert.SerializeObject(
                    _employeeMasterService.GetEmployeebyId(id));

                return Common.Utility.GetResult(result);
            }

        [HttpPost]
        public string Create(EmployeeMaster request)
        {
            string result = JsonConvert.SerializeObject(
                _employeeMasterService.Create(request));

            return Common.Utility.GetResult(result);
        }



        [HttpPut]
        public string Update(EmployeeMaster request)
        {

            string result = JsonConvert.SerializeObject(
                _employeeMasterService.Update(request));

            return Common.Utility.GetResult(result);
        }

        [HttpDelete]
            public string Delete(int id)
            {
                string result = null;

                result = JsonConvert.SerializeObject(
                    _employeeMasterService.DeleteById(id));

                return Common.Utility.GetResult(result);
            }

            [HttpPut]
            public string ActiveInActive(int Id, bool IsActive)
            {
                string result = null;

                result = JsonConvert.SerializeObject(
                    _employeeMasterService.ActiveInActive(Id, IsActive));

                return Common.Utility.GetResult(result);
            }
        }
    }

