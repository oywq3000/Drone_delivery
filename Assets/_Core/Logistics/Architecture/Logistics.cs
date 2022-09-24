using _Core.Logistics.Model;
using QFramework;

namespace _Core.Logistics.Architecture
{
    public class Logistics:Architecture<Logistics>
    {
        protected override void Init()
        {
            RegisterModel(new Cargo_DesInventory());
        }
    }
}