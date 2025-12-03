using UnityEngine;
using System.Collections;

public class LightReactionController : MonoBehaviour
{
    public Transform lightsParent;

    public float delayBetweenLights = 1f;
    public float lightActiveTime = 1.5f;

    private LampTrigger[] lampTriggers;

    private System.Random rng;
    private int currentIndex = -1;

    private void Start()
    {
        rng = new System.Random(System.DateTime.Now.Millisecond);

        lampTriggers = lightsParent.GetComponentsInChildren<LampTrigger>(true);

        StartCoroutine(LightLoop());
    }

    private IEnumerator LightLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenLights);

            // Vælg lampe
            currentIndex = rng.Next(lampTriggers.Length);

            // TÆND
            lampTriggers[currentIndex].ActivateLamp();

            yield return new WaitForSeconds(lightActiveTime);

            // Missed?
            if (lampTriggers[currentIndex].isLit)
                GameUIManager.Instance.MissedLamp();

            // SLUK 
            lampTriggers[currentIndex].DeactivateLamp();
        }
    }
}
