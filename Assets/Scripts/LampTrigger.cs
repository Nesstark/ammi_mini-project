using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using System.Collections;

// This code was made in collaboration with ChatGPT

public class LampTrigger : MonoBehaviour
{
    [Header("References")]
    public Light pointLight;  // <-- Assign in Inspector!

    [Header("Haptics")]
    public float hapticAmplitude = 0.7f;
    public float hapticDuration = 0.3f;

    [Header("Cooldown")]
    public float triggerCooldown = 1.0f;
    private bool canTrigger = true;

    private InputDevice leftHand;
    private InputDevice rightHand;
    private AudioSource audioSource;

    private void Awake()
    {
        // FAILSAFE: hvis den ikke er sat i Inspector, så find den
        if (pointLight == null)
            pointLight = GetComponentInChildren<Light>(true);

        if (pointLight == null)
            Debug.LogError($"{name}: No Point Light assigned or found!");

        audioSource = GetComponent<AudioSource>();
    }

    public void ActivateLamp()
    {
        if (pointLight != null)
            pointLight.enabled = true;

        if (audioSource != null)
            audioSource.Play();
    }

    public void DeactivateLamp()
    {
        if (pointLight != null)
            pointLight.enabled = false;
    }

    public bool isLit => pointLight != null && pointLight.enabled;

    private void Start()
    {
        InitializeControllers();
    }

    private void InitializeControllers()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0) leftHand = devices[0];

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0) rightHand = devices[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !canTrigger)
            return;

        if (isLit)
        {
            GameUIManager.Instance.AddPoint();
            DeactivateLamp();
        }
        else
        {
            GameUIManager.Instance.WrongLamp();
            SendHaptics(leftHand);
            SendHaptics(rightHand);
        }

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }

    private void SendHaptics(InputDevice device)
    {
        if (!device.isValid) return;
        if (device.TryGetHapticCapabilities(out HapticCapabilities c) && c.supportsImpulse)
            device.SendHapticImpulse(0, hapticAmplitude, hapticDuration);
    }
}
