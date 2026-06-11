using UNITYPOS_API.Entities;

namespace UNITYPOS_API.DAL.Interfaces
{
    public interface IBillingService
    {
        public string Create(Billing payment);
        public string Update(Billing billing);
    }
}
