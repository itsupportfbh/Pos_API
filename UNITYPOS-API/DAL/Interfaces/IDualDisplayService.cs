using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IDualDisplayService
    {
        IEnumerable<object> GetAll(int orgId);
        object? GetActiveProfile(int orgId, int branchId, int counterId);
        DualDisplayProfile? GetById(int id);
        string Create(DualDisplayProfile profile);
        string Update(DualDisplayProfile profile);
        string DeleteById(int id);
        string ActiveInActive(int id, bool isActive);
    }
}
