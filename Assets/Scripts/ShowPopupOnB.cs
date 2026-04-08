using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

public class ShowPopupOnB : MonoBehaviour
{
    public GameObject popupUI;

    private InputDevice rightController;

    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            rightController = devices[0];
        }

        popupUI.SetActive(false); // hide at start
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            InitializeController();
        }

        bool bButtonPressed = false;

        if (rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bButtonPressed) && bButtonPressed)
        {
            popupUI.SetActive(true);
        }
    }

    void InitializeController()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);

        if (devices.Count > 0)
        {
            rightController = devices[0];
        }
    }
}