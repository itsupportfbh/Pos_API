using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IFoodCategory
    {
        public IEnumerable<Object> GetAllCategory(int orgid);
        public IEnumerable<Object> GetCategorybyId(int id);
        public string Create(FoodMenuCategory Category);
        public string Update(FoodMenuCategory Category);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
