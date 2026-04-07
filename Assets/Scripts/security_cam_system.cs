using UnityEngine;
using System.Collections.Generic;

public class security_cam_system : MonoBehaviour
{
    public List<GameObject> cameras;
    public int cameraSelected;
    public security_cam_system other_button;

    [Header("VR Direct Interaction")]
    public bool useNextCamera = true;
    public string interactorTag = "Untagged";

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(interactorTag))
            return;

        if (useNextCamera)
        {
            nextCam();
        }
        else
        {
            previousCam();
        }
    }

    public void nextCam()
    {
        if (cameras == null || cameras.Count == 0)
            return;

        cameras[cameraSelected].SetActive(false);

        cameraSelected++;

        if (cameraSelected >= cameras.Count)
        {
            cameraSelected = 0;
        }

        cameras[cameraSelected].SetActive(true);

        if (other_button != null)
        {
            other_button.cameraSelected = cameraSelected;
        }

        Debug.Log(cameraSelected);
    }

    public void previousCam()
    {
        if (cameras == null || cameras.Count == 0)
            return;

        cameras[cameraSelected].SetActive(false);

        cameraSelected--;

        if (cameraSelected < 0)
        {
            cameraSelected = cameras.Count - 1;
        }

        cameras[cameraSelected].SetActive(true);

        if (other_button != null)
        {
            other_button.cameraSelected = cameraSelected;
        }

        Debug.Log(cameraSelected);
    }
}