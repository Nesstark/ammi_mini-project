using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;
using System.Collections;

public class LampTrigger : MonoBehaviour
{
    [Header("Haptics")]
    public float hapticAmplitude = 0.7f;
    public float hapticDuration = 0.3f;

    [Header("Cooldown")]
    public float triggerCooldown = 1.0f;
    private bool canTrigger = true;

    private InputDevice leftHand;
    private InputDevice rightHand;

    private Light pointLight;

    private void Start()
    {
        InitializeControllers();

        pointLight = GetComponentInChildren<Light>();
        if (pointLight == null)
            Debug.LogWarning($"LampTrigger {name} har ingen Point Light i children!");
    }

    private void InitializeControllers()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
            leftHand = devices[0];

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightHand = devices[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || !canTrigger)
            return;

        bool lampIsLit = pointLight != null && pointLight.enabled;

        if (lampIsLit)
        {
            GameUIManager.Instance.AddPoint();
            pointLight.enabled = false; // sluk lampen
        }
        else
        {
            GameUIManager.Instance.WrongLamp();
            SendHaptics(leftHand);
            SendHaptics(rightHand);
        }

        StartCoroutine(TriggerCooldown());
    }

    private IEnumerator TriggerCooldown()
    {
        canTrigger = false;
        yield return new WaitForSeconds(triggerCooldown);
        canTrigger = true;
    }

    private void SendHaptics(InputDevice device)
    {
        if (!device.isValid)
            return;

        if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
        {
            if (capabilities.supportsImpulse)
                device.SendHapticImpulse(0, hapticAmplitude, hapticDuration);
        }
    }

    // Property til LightReactionController
    public bool isLit
    {
        get { return pointLight != null && pointLight.enabled; }
    }

    // Metode til at tænde/slukke lampen
    public void SetLit(bool state)
    {
        if (pointLight != null)
            pointLight.enabled = state;
    }
}
