using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IFoodmenu
    {
        public IEnumerable<Object> GetAllMenu(int orgid);
        public IEnumerable<Object> GetMenubyId(int id);
        public string Create(FoodMenu Menu);
        public string Update(FoodMenu Menu);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
