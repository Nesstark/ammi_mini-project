using UnityEngine;

public class LampTrigger : MonoBehaviour
{
    public bool isLit = false;  // Set by LightReactionController

    public void SetLit(bool state)
    {
        isLit = state;
    }

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
        }
    }
}
