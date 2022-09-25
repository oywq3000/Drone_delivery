using System;
using System.Collections.Generic;
using _Core.Drove.Script.System;
using _Core.Scripts;
using QFramework;
using UnityEngine;

public class InputManager : MonoBehaviour, IController
{
    /// <summary>
    /// Used with InputSystem
    /// </summary>
    private IInputSystem inputSystem;
    void Start()
    {
        inputSystem = this.GetSystem<IInputSystem>();
        inputSystem.RegisterGetKeyDown(ClientKeys.Mouse0,(() =>
        {
            Debug.Log("000000000");
        }));
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.anyKeyDown)
        {
            foreach (ClientKeys VARIABLE in Enum.GetValues(typeof(ClientKeys)))
            {
                if (Input.GetKeyDown((KeyCode)VARIABLE))
                {
                    if (inputSystem.KeyQueryDic.TryGetValue(VARIABLE,out List<Action> actions))
                    {
                        foreach (var action in actions)
                        {
                            action.Invoke();
                        }
                    }
                }
            }
        }

        if (Input.anyKey)
        {
            foreach (ClientKeys VARIABLE in Enum.GetValues(typeof(ClientKeys)))
            {
                if (Input.GetKey((KeyCode)VARIABLE))
                {
                    if (inputSystem.KeyQueryDicDown.TryGetValue(VARIABLE,out List<Action> actions))
                    {
                        foreach (var action in actions)
                        {
                            action.Invoke();
                        }
                    }
                }
            }
        }
        
    }

    public IArchitecture GetArchitecture()
    {
        return DroneArchitecture.Interface;
    }
}