using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Core.Logistics.Manager
{
    
    public class DesMgr:MonoBehaviour
    {
        // private List<DesController> idleDeses;
        // private List<DesController> occupyedDeses;
        //
        // private void Awake()
        // {
        //     occupyedDeses = new List<DesController>();
        //     idleDeses = GetComponentsInChildren<DesController>().ToList();
        // }
        //
        // private void Start()
        // {
        //     AutoInitiateDesId();
        // }
        //
        // void AutoInitiateDesId()
        // {
        //     int index = 0;
        //     foreach (var VARIABLE in idleDeses)
        //     {
        //         index++;
        //         if (index < 10)
        //         {
        //             VARIABLE.Id = "000" + index;
        //         }
        //         else if (index < 100)
        //         {
        //             VARIABLE.Id = "00" + index;
        //         }
        //         else if (index < 1000)
        //         {
        //             VARIABLE.Id = "0" + index;
        //         }
        //         else if (index < 10000)
        //         {
        //             VARIABLE.Id = index.ToString();
        //         }
        //     }
        // }
        //
        // public Vector3 GetPos(int index = 0)
        // {
        //     if (idleDeses.Count != 0)
        //     {
        //         var destinationController = idleDeses[index];
        //         occupyedDeses.Add(destinationController);
        //         return destinationController.GetOriginPos();
        //     }
        //
        //     return default;
        // }
    }
}