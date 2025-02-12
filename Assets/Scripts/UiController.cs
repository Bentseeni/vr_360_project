using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    public GameObject Ui;
    public InputActionReference MenuAction;

    private void Awake()
    {
        MenuAction.action.Enable();
        MenuAction.action.performed += ToggleMenu;
        InputSystem.onDeviceChange += OnChangeDevice;
    }

    private void OnChangeDevice(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Disconnected:
                MenuAction.action.Disable();
                MenuAction.action.performed -= ToggleMenu;
                break;
            case InputDeviceChange.Reconnected:
                MenuAction.action.Enable();
                MenuAction.action.performed += ToggleMenu;
                break;
        }
    }


    private void ToggleMenu(InputAction.CallbackContext context)
    {
        Ui.SetActive(!Ui.activeSelf);
       
    }
}
