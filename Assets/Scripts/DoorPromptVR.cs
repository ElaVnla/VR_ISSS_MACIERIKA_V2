using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorPromptVR : MonoBehaviour
{
    [Header("UI")]
    public GameObject promptText;
    public GameObject winPopup;
    public GameObject losePopup;

    [Header("Player")]
    public Transform player;
    public float activationDistance = 0.3f;

    [Header("Buttons to Track")]
    public XRSimpleInteractable tempStartDock;
    public XRSimpleInteractable tempGrabLever;

    [Header("Debug State")]
    [SerializeField] private bool startDockPressed = false;
    [SerializeField] private bool grabLeverPressed = false;

    private bool playerNearby = false;
    private bool bWasPressedLastFrame = false;

    private void OnEnable()
    {
        if (tempStartDock != null)
        {
            tempStartDock.hoverEntered.AddListener(OnStartDockHover);
            tempStartDock.selectEntered.AddListener(OnStartDockSelect);
        }

        if (tempGrabLever != null)
        {
            tempGrabLever.hoverEntered.AddListener(OnGrabLeverHover);
            tempGrabLever.selectEntered.AddListener(OnGrabLeverSelect);
        }
    }

    private void OnDisable()
    {
        if (tempStartDock != null)
        {
            tempStartDock.hoverEntered.RemoveListener(OnStartDockHover);
            tempStartDock.selectEntered.RemoveListener(OnStartDockSelect);
        }

        if (tempGrabLever != null)
        {
            tempGrabLever.hoverEntered.RemoveListener(OnGrabLeverHover);
            tempGrabLever.selectEntered.RemoveListener(OnGrabLeverSelect);
        }
    }

    void Start()
    {
        if (promptText != null) promptText.SetActive(false);
        if (winPopup != null) winPopup.SetActive(false);
        if (losePopup != null) losePopup.SetActive(false);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        playerNearby = distance <= activationDistance;

        if (promptText != null)
            promptText.SetActive(playerNearby);

        bool bPressedNow = IsBPressed();

        if (playerNearby && bPressedNow && !bWasPressedLastFrame)
        {
            CheckTaskResult();
        }

        bWasPressedLastFrame = bPressedNow;
    }

    private bool IsBPressed()
    {
        InputDevice rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        if (rightHand.TryGetFeatureValue(CommonUsages.secondaryButton, out bool pressed))
            return pressed;

        return false;
    }

    // --- Temp Start Dock ---
    private void OnStartDockHover(HoverEnterEventArgs args)
    {
        startDockPressed = true;
        Debug.Log("Start Dock registered by HOVER");
    }

    private void OnStartDockSelect(SelectEnterEventArgs args)
    {
        startDockPressed = true;
        Debug.Log("Start Dock registered by SELECT");
    }

    // --- Temp Grab Lever ---
    private void OnGrabLeverHover(HoverEnterEventArgs args)
    {
        grabLeverPressed = true;
        Debug.Log("Grab Lever registered by HOVER");
    }

    private void OnGrabLeverSelect(SelectEnterEventArgs args)
    {
        grabLeverPressed = true;
        Debug.Log("Grab Lever registered by SELECT");
    }

    private void CheckTaskResult()
    {
        Debug.Log("Checking result...");
        Debug.Log("StartDock = " + startDockPressed + ", GrabLever = " + grabLeverPressed);

        if (startDockPressed && grabLeverPressed)
        {
            if (winPopup != null) winPopup.SetActive(true);
            if (losePopup != null) losePopup.SetActive(false);
            Debug.Log("WIN");
        }
        else
        {
            if (losePopup != null) losePopup.SetActive(true);
            if (winPopup != null) winPopup.SetActive(false);
            Debug.Log("LOSE");
        }
    }
}