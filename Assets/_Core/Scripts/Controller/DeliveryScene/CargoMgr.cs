using UnityEngine;
namespace _Core.Scripts.Controller
{
    public class CargoMgr : MonoBehaviour
    {
        // private List<CargoController> idleDeses;
        // private List<CargoController> occupyedDeses;
        //
        // private void Awake()
        // {
        //     occupyedDeses = new List<CargoController>();
        //     idleDeses = GetComponentsInChildren<CargoController>().ToList();
        // }
        //
        // private void Start()
        // {
        //     //Initiate cargos Id
        //     AutoInitiateCargosId();
        // }
        //
        // void AutoInitiateCargosId()
        // {
        //     int index = 0;
        //     foreach (var VARIABLE in idleDeses)
        //     {
        //         index++;
        //         if (index<10)
        //         {
        //             VARIABLE.Id = "000"+index;
        //         }else if (index<100)
        //         {
        //             VARIABLE.Id = "00"+index;
        //         }
        //         else if(index<1000)
        //         {
        //             VARIABLE.Id = "0"+index;
        //         }
        //         else if(index<10000)
        //         {
        //             VARIABLE.Id = index.ToString();
        //         }
        //     }
        // }
        //
        // public Vector3 GetOriginPos(int index = 0)
        // {
        //     if (idleDeses.Count != 0)
        //     {
        //         var cargoController = idleDeses[index];
        //         occupyedDeses.Add(cargoController);
        //         return cargoController.GetOriginPos();
        //     }
        //
        //     return default;
        // }
    }
}