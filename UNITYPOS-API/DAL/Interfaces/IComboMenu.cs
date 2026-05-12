using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IComboMenu
    {
        public IEnumerable<object> GetAllComboMenu(int orgid);
        public object GetComboMenubyId(int id);
        public string Create(ComboMenu comboMenu);
        public string Update(ComboMenu comboMenu);
        public string DeleteById(int id);
        public string ActiveInActive(int id, bool isActive);


    }
}
