using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class LampTrigger : MonoBehaviour
{
    public bool isLit = false;  // Set by LightReactionController
    
    [Header("Haptics")]
    public XRBaseInputInteractor leftControllerInteractor;
    public XRBaseInputInteractor rightControllerInteractor;
    public float hapticAmplitude = 0.7f;
    public float hapticDuration = 0.3f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (isLit)
        {
            // Correct lamp!
            GameUIManager.Instance.AddPoint();
            isLit = false; // prevent double scoring
        }
        else
        {
            // Wrong lamp
            GameUIManager.Instance.WrongLamp();
            
            // Trigger haptics på begge controllere
            SendHapticFeedback(leftControllerInteractor);
            SendHapticFeedback(rightControllerInteractor);
        }
    }
    
    private void SendHapticFeedback(XRBaseInputInteractor interactor)
    {
        if (interactor != null && interactor.xrController != null)
        {
            interactor.xrController.SendHapticImpulse(hapticAmplitude, hapticDuration);
        }
    }
    
    public void SetLit(bool state)
    {
        isLit = state;
    }
}