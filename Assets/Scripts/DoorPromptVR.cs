using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DoorPromptVR : MonoBehaviour
{
    [Header("UI")]
    public GameObject promptText;
    public GameObject losePopup;

    [Header("Player")]
    public Transform player;
    public float activationDistance = 0.3f;

    [Header("Start Dock Button")]
    public XRSimpleInteractable tempStartDock;

    [Header("Lever")]
    public HingeJoint leverHinge;
    public float leverRequiredAngleChange = 25f;
    public bool downMeansAngleIncrease = true;

    [Header("Scene Change On Win")]
    public string winSceneName;

    [Header("Lose Haptics")]
    [Range(0f, 1f)] public float loseHapticAmplitude = 0.7f;
    public float loseHapticDuration = 0.2f;
    public bool vibrateLeftController = true;
    public bool vibrateRightController = true;

    [Header("Debug State")]
    [SerializeField] private bool startDockPressed = false;
    [SerializeField] private bool grabLeverPressed = false;
    [SerializeField] private float startLeverAngle = 0f;
    [SerializeField] private float currentLeverAngle = 0f;
    [SerializeField] private float signedLeverDelta = 0f;

    private bool playerNearby = false;
    private bool bWasPressedLastFrame = false;
    private bool resultTriggered = false;

    private void OnEnable()
    {
        if (tempStartDock != null)
        {
            tempStartDock.hoverEntered.AddListener(OnStartDockHover);
            tempStartDock.selectEntered.AddListener(OnStartDockSelect);
        }
    }

    private void OnDisable()
    {
        if (tempStartDock != null)
        {
            tempStartDock.hoverEntered.RemoveListener(OnStartDockHover);
            tempStartDock.selectEntered.RemoveListener(OnStartDockSelect);
        }
    }

    private void Start()
    {
        if (promptText != null) promptText.SetActive(false);
        if (losePopup != null) losePopup.SetActive(false);

        if (leverHinge != null)
        {
            startLeverAngle = leverHinge.angle;
            Debug.Log("Lever START angle recorded as: " + startLeverAngle);
        }
        else
        {
            Debug.LogWarning("Lever Hinge is not assigned.");
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        playerNearby = distance <= activationDistance;

        if (promptText != null)
            promptText.SetActive(playerNearby);

        UpdateLeverState();

        bool bPressedNow = IsBPressed();

        if (!resultTriggered && playerNearby && bPressedNow && !bWasPressedLastFrame)
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

    private void UpdateLeverState()
    {
        if (leverHinge == null || grabLeverPressed) return;

        currentLeverAngle = leverHinge.angle;
        signedLeverDelta = Mathf.DeltaAngle(startLeverAngle, currentLeverAngle);

        bool leverComplete = downMeansAngleIncrease
            ? signedLeverDelta >= leverRequiredAngleChange
            : signedLeverDelta <= -leverRequiredAngleChange;

        if (leverComplete)
        {
            grabLeverPressed = true;
            Debug.Log("Lever pulled DOWN and marked complete.");
        }

        Debug.Log("Lever Start: " + startLeverAngle +
                  " | Current: " + currentLeverAngle +
                  " | Delta: " + signedLeverDelta +
                  " | Complete: " + grabLeverPressed);
    }

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

    private void TriggerLoseHaptics()
    {
        if (vibrateLeftController)
            SendHapticToNode(XRNode.LeftHand, loseHapticAmplitude, loseHapticDuration);

        if (vibrateRightController)
            SendHapticToNode(XRNode.RightHand, loseHapticAmplitude, loseHapticDuration);
    }

    private void SendHapticToNode(XRNode node, float amplitude, float duration)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(node);

        if (!device.isValid)
        {
            Debug.LogWarning(node + " controller is not valid for haptics.");
            return;
        }

        if (device.TryGetHapticCapabilities(out HapticCapabilities capabilities))
        {
            if (capabilities.supportsImpulse)
            {
                device.SendHapticImpulse(0u, amplitude, duration);
                Debug.Log("Haptic sent to " + node);
            }
            else
            {
                Debug.LogWarning(node + " does not support haptic impulse.");
            }
        }
        else
        {
            Debug.LogWarning("Could not get haptic capabilities for " + node);
        }
    }

    private void CheckTaskResult()
    {
        Debug.Log("Checking result...");
        Debug.Log("StartDock = " + startDockPressed + ", GrabLever = " + grabLeverPressed);

        if (startDockPressed && grabLeverPressed)
        {
            resultTriggered = true;
            Debug.Log("WIN");

            if (losePopup != null) losePopup.SetActive(false);

            if (!string.IsNullOrEmpty(winSceneName))
            {
                Debug.Log("Loading scene: " + winSceneName);
                SceneManager.LoadScene(winSceneName);
            }
            else
            {
                Debug.LogWarning("winSceneName is empty.");
            }
        }
        else
        {
            if (losePopup != null) losePopup.SetActive(true);

            TriggerLoseHaptics();

            Debug.Log("LOSE");
        }
    }
}