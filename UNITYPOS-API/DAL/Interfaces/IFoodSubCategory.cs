using UNITYPOS_API.Entities.Master;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IFoodSubCategory
    {
        public IEnumerable<Object> GetAllSubCategory(int orgid);
        public IEnumerable<Object> GetSubCategorybyId(int id);
        public string Create(FoodMenuSubCategory SubCategory);
        public string Update(FoodMenuSubCategory SubCategory);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);
    }
}
