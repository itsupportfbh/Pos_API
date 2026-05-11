using System.Text.RegularExpressions;
using UNITYPOS_API.DAL.Interfaces;
using UNITYPOS_API.Data.ORM;
using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Services
{
    public class DiningTableService : IDiningTable
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _configuration;
        public DiningTableService(IUnitOfWork uow, IConfiguration configuration)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _configuration = configuration;
        }

        public IEnumerable<Object> GetAllDiningTable(int orgid)
        {
            IEnumerable<Object> result = null;

            result = (from d in _uow.GenericRepository<DiningTableMaster>().Table()                      
                      join o in _uow.GenericRepository<Organization>().Table()
                        on d.OrgId equals o.Id
                      join b in _uow.GenericRepository<Branch>().Table()
                        on d.BranchId equals b.Id
                      join f in _uow.GenericRepository<FloorMaster>().Table()
                       on d.FloorId equals f.Id
                      where d.IsDeleted == false && (orgid == 0 || b.OrgId == orgid)
                      select new
                      {
                          id = d.Id,
                          organizationname = o.Name,
                          name = d.Name,
                          code = d.Code,
                          branch = d.BranchId,
                          floor = d.FloorId,
                          branchname = b.Name,
                          floorname = f.Name,
                          seatingsize = d.SeatingSize,
                          available = d.IsAvailable,
                          reservable = d.IsReservable,
                          occupied = d.IsOccupied,
                          remarks = d.Remarks,
                          isactive = d.IsActive,
                      }).ToList();


            return result;
        }
        public IEnumerable<Object> GetDiningTablebyId(int id)
        {
            IEnumerable<Object> result = null;

            result = (from b in _uow.GenericRepository<DiningTableMaster>().Table()
                      join o in _uow.GenericRepository<Organization>().Table()
                        on b.OrgId equals o.Id                     
                      where b.IsDeleted == false && b.Id == id
                      select new
                      {
                          id = b.Id,
                          name = b.Name,
                          code = b.Code,
                          branchId = b.BranchId, 
                          floorId = b.FloorId,
                          seatingSize = b.SeatingSize,
                          displayorder = b.DisplayOrder,
                          remarks = b.Remarks,
                          isActive = b.IsActive,
                          image = string.IsNullOrEmpty(b.Image)
        ? null
        : _configuration["AppSettings:FileUploadPathView"] + b.Image
                      }).ToList();


            return result;
        }


        public string Create(DiningTableMaster Table)
        {
            int check = _uow.GenericRepository<DiningTableMaster>().Table()
                .Count(b => b.Name.Trim().ToLower() == Table.Name.Trim().ToLower()
                         && b.OrgId == Table.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var entity = new DiningTableMaster
            {
                Code = Table.Code,
                Name = Table.Name,
                BranchId = Table.BranchId,
                FloorId = Table.FloorId,
                SeatingSize = Table.SeatingSize,
                IsAvailable = Table.IsAvailable,
                IsReservable = Table.IsReservable,
                IsOccupied = Table.IsOccupied,
                Image = SaveBase64Image(Table.Image),
                Remarks =   Table.Remarks,
                OrgId   = Table.OrgId,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = Table.CreatedBy,
                CreatedDate = DateTime.Now
            };

            _uow.GenericRepository<DiningTableMaster>().Insert(entity);
            _uow.Save();

            return Convert.ToString(entity.Id);
        }

        public string Update(DiningTableMaster Table)
        {
            int check = _uow.GenericRepository<DiningTableMaster>().Table()
                .Count(b => (b.Name.Trim().ToLower() == Table.Name.Trim().ToLower()
                          || b.Code.Trim().ToLower() == Table.Code.Trim().ToLower())
                         && b.Id != Table.Id
                         && b.OrgId == Table.OrgId
                         && b.IsDeleted == false);

            if (check > 0)
            {
                return "AlreadyExists";
            }

            var existingTable = _uow.GenericRepository<DiningTableMaster>().Table()
                .FirstOrDefault(x => x.Id == Table.Id
                                  && x.OrgId == Table.OrgId
                                  && x.IsDeleted == false);

            if (existingTable != null)
            {
                existingTable.Code = Table.Code;
                existingTable.Name = Table.Name;
                existingTable.BranchId = Table.BranchId;
                existingTable.FloorId = Table.FloorId;
                existingTable.SeatingSize = Table.SeatingSize;
                existingTable.IsOccupied = Table.IsOccupied;
                existingTable.Image = SaveBase64Image(Table.Image);
                existingTable.Remarks = Table.Remarks;
                existingTable.OrgId = Table.OrgId;
                existingTable.IsActive = true;
                existingTable.IsDeleted = false;
                existingTable.UpdatedBy = Table.UpdatedBy;
                existingTable.UpdatedDate = DateTime.Now;

                _uow.GenericRepository<DiningTableMaster>().Update(existingTable);
                _uow.Save();

                return Convert.ToString(existingTable.Id);
            }

            return "0";
        }
        public string DeleteById(int id)
        {
            var result = _uow.GenericRepository<DiningTableMaster>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsDeleted = true;
                _uow.GenericRepository<DiningTableMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);


        }

        public string ActiveInActive(int id, bool isActive)
        {
            var result = _uow.GenericRepository<DiningTableMaster>().Table().Where(x => x.Id == id).FirstOrDefault();
            if (result != null)
            {

                result.IsActive = isActive;
                _uow.GenericRepository<DiningTableMaster>().Update(result);
                _uow.Save();
            }

            return Convert.ToString(result?.Id ?? 0);
        }

        private string SaveBase64Image(string base64Image)
        {
            if (string.IsNullOrEmpty(base64Image))
                return null;

            // Remove base64 header
            var matches = Regex.Match(base64Image, @"data:image/(?<type>.+?),(?<data>.+)");

            if (!matches.Success)
                return null;

            string extension = matches.Groups["type"].Value.Split(';')[0];

            string base64Data = matches.Groups["data"].Value;

            byte[] imageBytes = Convert.FromBase64String(base64Data);

            string fileName = $"table_{Guid.NewGuid()}.{extension}";

            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "FileUpload"
            );

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            string filePath = Path.Combine(folderPath, fileName);

            File.WriteAllBytes(filePath, imageBytes);

            return fileName;
        }
    }
}
