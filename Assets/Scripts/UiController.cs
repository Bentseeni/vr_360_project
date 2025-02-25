using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiController : MonoBehaviour
{
    public GameObject Ui;
    public TMP_Text ButtonText;
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
        ChangeButtonText();
       
    }

    public void ButtonToggleMenu()
    {
        Ui.SetActive(!Ui.activeSelf);
        ChangeButtonText();

    }

    private void ChangeButtonText()
    {
        if (Ui.activeSelf == false)
        {
            ButtonText.text = "Näytä valikko";
        }
        else
        {
            ButtonText.text = "Piilota valikko";
        }
    }

        
}
