
using _Core.Drove.Event;
using QFramework;
namespace _Core.Drove.Script.System
{
    public interface IInputSystem:ISystem
    {
        void InterruptDroneOperation(bool isInterrupt);
    }
    public class InputSystem: AbstractSystem,IInputSystem
    {
        
        protected override void OnInit()
        {
           
        }
        public void InterruptDroneOperation(bool isInterrupt)
        {
            this.SendEvent(new DroneOperationListener(){IsInterrupt = isInterrupt});
        }
    }
}