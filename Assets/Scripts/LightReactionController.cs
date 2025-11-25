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

    private System.Random rng;

    private void Start()
    {
        rng = new System.Random(System.DateTime.Now.Millisecond);

        // Detect all Light components
        lights = lightsParent.GetComponentsInChildren<Light>(true);

        // Detect matching AudioSources (one on each lamp)
        audioSources = lightsParent.GetComponentsInChildren<AudioSource>(true);

        if (lights.Length == 0)
        {
            Debug.LogError("No Light components found under lightsParent!");
            return;
        }

        StartCoroutine(LightLoop());
    }

    private IEnumerator LightLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenLights);

            int index = rng.Next(lights.Length);

            ActivateLight(index);
            yield return new WaitForSeconds(lightActiveTime);
            DeactivateLight(index);
        }
    }

    private void ActivateLight(int index)
    {
        lights[index].enabled = true;

        // Play spatial ding from that lamp
        if (audioSources[index] != null)
            audioSources[index].Play();
    }

    private void DeactivateLight(int index)
    {
        lights[index].enabled = false;
    }
}
