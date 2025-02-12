using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.UI;

public class ButtonHaptic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private XRUIInputModule inputModule => EventSystem.current.currentInputModule as XRUIInputModule;
    public float impulseStrength = 0.5f;
    public float impulseDuration = 1.0f;
    void Start()
    {
        XRBaseInteractable interactable = GetComponent<XRBaseInteractable>();
      
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        NearFarInteractor nearFarInteractor = inputModule.GetInteractor(eventData.pointerId) as NearFarInteractor;

        nearFarInteractor.SendHapticImpulse(impulseStrength, impulseDuration);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        NearFarInteractor nearFarInteractor = inputModule.GetInteractor(eventData.pointerId) as NearFarInteractor;

        if (!nearFarInteractor)
        {
            return;
        }
        nearFarInteractor.SendHapticImpulse(impulseStrength, impulseDuration);
    }


    public void PointerEnter(PointerEventData eventData)
    {
        NearFarInteractor nearFarInteractor = inputModule.GetInteractor(eventData.pointerId) as NearFarInteractor;
        nearFarInteractor.SendHapticImpulse(impulseStrength, impulseDuration);
        
    }
     public void triggerHaptic(NearFarInteractor interactor)
    {
        interactor.SendHapticImpulse(impulseStrength,impulseDuration);
    }
    

}
