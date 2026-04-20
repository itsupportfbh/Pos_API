namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IMenuService
    {
        public IEnumerable<object> GetAllMenuAndSubMenu();
    }
}
