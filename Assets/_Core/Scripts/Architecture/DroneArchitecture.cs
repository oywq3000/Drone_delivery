using _Core.Drove.Script.System;

using QFramework;
using Drove;


namespace _Core.Scripts
{
    public class DroneArchitecture: Architecture<DroneArchitecture>
    {
        protected override void Init()
        {

            //Register model
            RegisterModel<IDroneCountModel>(new DroneCountModel());
            RegisterModel(new PressedType());
            RegisterModel<ICargoDesMap>(new CargoDesMap());
            RegisterModel<IDroneRestPos>(new DroneRestPos());
            RegisterModel<ICargoDesMap>(new CargoDesMap());
            RegisterModel<IAerialDroneCount>(new AerialDroneCount());
            RegisterModel<ISceneCameraModel>(new SceneCameraModel());
            //register event
            RegisterSystem<INavigationSystem>(new NavigationSystem());
            RegisterSystem<IInputSystem>(new InputSystem());
            RegisterSystem<ISceneSystem>(new SceneSystem());
            //register Utility
        }
    }
}