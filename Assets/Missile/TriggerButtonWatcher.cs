using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.XR;

[System.Serializable]
public class TriggerButtonEvent : UnityEvent<bool> { }

public class TriggerButtonWatcher : MonoBehaviour
{
    public TriggerButtonEvent triggerButtonPress;

    private bool lastButtonState = false;
    private bool appState = true;

    private List<InputDevice> devicesWithTriggerButton;

    private void InputDevices_deviceConnected(InputDevice device)
    {
        bool discardedValue;
        if (device.TryGetFeatureValue(CommonUsages.menuButton, out discardedValue))
        {
            devicesWithTriggerButton.Add(device);
        }
    }

    private void InputDevices_deviceDisconnected(InputDevice device)
    {
        devicesWithTriggerButton.Remove(device);
    }

    // Start is called before the first frame update
    void Start()
    {
        devicesWithTriggerButton = new List<InputDevice>();

        RegisterDevices();
    }

    private void OnEnable(){
        RegisterDevices();
    }

    private void onDisable(){
        InputDevices.deviceConnected -= InputDevices_deviceConnected;
        InputDevices.deviceDisconnected -= InputDevices_deviceDisconnected;
        devicesWithTriggerButton.Clear();
    }

    private void RegisterDevices()
    {
        List<InputDevice> allDevices = new List<InputDevice>();
        InputDevices.GetDevices(allDevices);
        foreach (InputDevice device in allDevices)
        {
            InputDevices_deviceConnected(device);
        }
        InputDevices.deviceConnected += InputDevices_deviceConnected;
        InputDevices.deviceDisconnected += InputDevices_deviceDisconnected;
    }

    // Update is called once per frame
    void Update()
    {
        bool tempState = false;
        foreach (var device in devicesWithTriggerButton)
        {
            bool primaryButtonState = false;
            tempState = device.TryGetFeatureValue(CommonUsages.menuButton, out primaryButtonState) && primaryButtonState || tempState;
        }

        if (tempState != lastButtonState)
        {
            if(tempState){
                appState = !appState;
                triggerButtonPress.Invoke(appState);
            }      
            lastButtonState = tempState;
        }
    }
}