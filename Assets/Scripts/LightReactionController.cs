using UnityEngine;
using System.Collections;

public class LightReactionController : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform lightsParent;     // Parent object with all the lamp objects

    [Header("Light Settings")]
    public float delayBetweenLights = 1f;
    public float lightActiveTime = 1.5f;

    private Light[] lights;
    private AudioSource[] audioSources;
    private LampTrigger[] lampTriggers;

    private System.Random rng;
    private int currentIndex = -1;

    private void Start()
    {
        rng = new System.Random(System.DateTime.Now.Millisecond);

        lights = lightsParent.GetComponentsInChildren<Light>(true);
        audioSources = lightsParent.GetComponentsInChildren<AudioSource>(true);
        lampTriggers = lightsParent.GetComponentsInChildren<LampTrigger>(true);

        StartCoroutine(LightLoop());
    }

    private IEnumerator LightLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenLights);

            // Pick new random lamp
            currentIndex = rng.Next(lights.Length);

            ActivateLight(currentIndex);
            lampTriggers[currentIndex].SetLit(true);

            yield return new WaitForSeconds(lightActiveTime);

            // Missed the reaction window?
            if (lampTriggers[currentIndex].isLit)
            {
                GameUIManager.Instance.MissedLamp();
            }

            DeactivateLight(currentIndex);
            lampTriggers[currentIndex].SetLit(false);
        }
    }

    private void ActivateLight(int index)
    {
        lights[index].enabled = true;

        if (audioSources[index] != null)
            audioSources[index].Play();
    }

    private void DeactivateLight(int index)
    {
        lights[index].enabled = false;
    }
}
