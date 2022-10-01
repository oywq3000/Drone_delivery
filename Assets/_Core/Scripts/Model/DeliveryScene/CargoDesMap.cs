
using System.Collections.Generic;
using _Core.Drove.Event;
using _Core.Logistics;
using QFramework;
using _Core.Scripts;
using _Core.Scripts.Controller;
using UnityEngine;


namespace Drove
{
    public interface ICargoDesMap : IModel
    {
        List<CargoController> CargoList { get; set; }
        List<DesController> DesList { get; set; }
    }

    public class CargoDesMap : AbstractModel, ICargoDesMap
    {
        public List<CargoController> CargoList { get; set; } =
            new List<CargoController>();

        private int cargoMaxCount = 60;

        public List<DesController> DesList { get; set; } = new List<DesController>();

        protected override void OnInit()
        {
            //if get Scene Exit Event then clear the ChannelList 
            DroneArchitecture.Interface.RegisterEvent<OnSceneExit>(e =>
            { 
                 CargoList.Clear();
                 DesList.Clear();
            });
        }
    }
}