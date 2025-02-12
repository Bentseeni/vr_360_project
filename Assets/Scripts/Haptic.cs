using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Haptic : MonoBehaviour
{
    public float hapticIntensity = 0.5f;
    public float hapticDuration = 0.1f;
    public GameObject controller;


    public void hover()
    {
        HapticImpulsePlayer player = controller.GetComponent<HapticImpulsePlayer>();
        if (player != null)
        {
            player.SendHapticImpulse(hapticIntensity,hapticDuration);
        }
    }
   
}
