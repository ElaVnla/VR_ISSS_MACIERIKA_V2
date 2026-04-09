using UnityEngine;
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

    [Tooltip("How many degrees the lever must move from its starting UP position before it counts as complete")]
    public float leverRequiredAngleChange = 25f;

    [Tooltip("Turn this ON if pulling the lever DOWN makes the hinge angle go smaller/negative. Turn OFF if pulling DOWN makes the angle go bigger/positive.")]
    public bool downMeansAngleDecreases = true;

    [Header("Scene Change On Win")]
    public SceneController sceneController;
    public string winSceneName;

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
            tempStartDock.selectEntered.AddListener(OnStartDockSelect);
        }
    }

    private void OnDisable()
    {
        if (tempStartDock != null)
        {
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

        // Measures how much the lever has rotated from its starting position
        signedLeverDelta = Mathf.DeltaAngle(startLeverAngle, currentLeverAngle);

        bool leverComplete;

        if (downMeansAngleDecreases)
        {
            leverComplete = signedLeverDelta <= -leverRequiredAngleChange;
        }
        else
        {
            leverComplete = signedLeverDelta >= leverRequiredAngleChange;
        }

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

    private void OnStartDockSelect(SelectEnterEventArgs args)
    {
        startDockPressed = true;
        Debug.Log("Start Dock registered by SELECT");
    }

    private void CheckTaskResult()
    {
        Debug.Log("Checking result...");
        Debug.Log("StartDock = " + startDockPressed + ", GrabLever = " + grabLeverPressed);
        Debug.Log("Lever delta = " + signedLeverDelta);

        if (startDockPressed && grabLeverPressed)
        {
            resultTriggered = true;
            Debug.Log("WIN");

            if (sceneController != null && !string.IsNullOrEmpty(winSceneName))
            {
                sceneController.ChangeScene(winSceneName);
            }
            else
            {
                Debug.LogWarning("SceneController or winSceneName is missing.");
            }
        }
        else
        {
            if (losePopup != null) losePopup.SetActive(true);
            Debug.Log("LOSE");
        }
    }
}