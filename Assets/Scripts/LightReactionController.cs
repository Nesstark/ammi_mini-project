using UnityEngine;
using System.Collections;

public class LightReactionController : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform lightsParent;     // The parent object that holds all cylinders/lights

    [Header("Light Settings")]
    public float delayBetweenLights = 1f;
    public float lightActiveTime = 1.5f;

    [Header("Audio Settings")]
    public AudioClip dingSound;
    private AudioSource audioSource;

    private Light[] lights;            // Array of detected lights
    private System.Random rng;

    private void Start()
    {
        // True random
        rng = new System.Random(System.DateTime.Now.Millisecond);

        // Setup audio
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Auto-detect lights in children
        lights = lightsParent.GetComponentsInChildren<Light>(true);

        if (lights.Length == 0)
        {
            Debug.LogError("No Light components found under the assigned parent!");
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
            Light chosenLight = lights[index];

            ActivateLight(chosenLight);
            yield return new WaitForSeconds(lightActiveTime);
            DeactivateLight(chosenLight);
        }
    }

    private void ActivateLight(Light light)
    {
        light.enabled = true;

        if (dingSound != null)
            audioSource.PlayOneShot(dingSound);
    }

    private void DeactivateLight(Light light)
    {
        light.enabled = false;
    }
}
