using System;
using System.Collections.Generic;
using System.IO;
using Drove;
using QFramework;
using UnityEngine;

namespace _Core.Logistics.Model
{
    
    public class Cargo_DesInventory:AbstractModel
    {
        private CargoDesSet cargoDesSet;
        private struct CargoDesIndex
        {
            public string cargo;
            public String des;
        }

        private struct CargoDesSet
        {
            public CargoDesIndex[] CargoDesIndexs;
        }
        
        protected override void OnInit()
        {
            //Parse json information
            string jsonPath = Application.streamingAssetsPath + "/Cargo_DesInventory.json";
            string jsons = File.ReadAllText(jsonPath);
            cargoDesSet = JsonUtility.FromJson<CargoDesSet>(jsons);
        }

        public string GetDesIndex(string cargoIndex)
        {
            foreach (var VARIABLE in cargoDesSet.CargoDesIndexs)
            {
                if (cargoIndex == VARIABLE.cargo)
                {
                    return VARIABLE.des;
                }
            }
            //if return null then indicates this inventory is not the corresponding des of this cargo
            return null;
        }
    }
}