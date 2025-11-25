using UnityEngine;
using System.Collections;

public class LightReactionController : MonoBehaviour
{
    [Header("Light Settings")]
    public GameObject[] lightTargets;      // Assign your light objects here
    public float delayBetweenLights = 1f;  // Time before the next random light
    public float lightActiveTime = 1.5f;   // How long the light stays on

    [Header("Audio Settings")]
    public AudioClip dingSound;            // Assign your .wav file here
    private AudioSource audioSource;

    private System.Random rng;

    private void Start()
    {
        // True random seed
        rng = new System.Random(System.DateTime.Now.Millisecond);

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Start the loop
        StartCoroutine(LightLoop());
    }

    private IEnumerator LightLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(delayBetweenLights);

            if (lightTargets.Length == 0)
                yield break;

            int index = rng.Next(lightTargets.Length);
            GameObject chosenLight = lightTargets[index];

            ActivateLight(chosenLight);

            yield return new WaitForSeconds(lightActiveTime);

            DeactivateLight(chosenLight);
        }
    }

    private void ActivateLight(GameObject lightObj)
    {
        lightObj.SetActive(true);

        // Play ding sound
        if (dingSound != null)
        {
            audioSource.PlayOneShot(dingSound);
        }
    }

    private void DeactivateLight(GameObject lightObj)
    {
        lightObj.SetActive(false);
    }
}
