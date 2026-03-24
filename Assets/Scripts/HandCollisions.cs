using UnityEngine;
public class HandCollisions : MonoBehaviour
{
    // Set one or more wall layers in the Inspector
    public LayerMask wallMask;
    // Other scripts read this
    public bool isTouching = false;
    // Small helper so you never see bit math outside this method
    bool IsWallLayer(int layer)
    {
        int layerBit = 1 << layer;
        return (wallMask.value & layerBit) != 0;
    }
    void OnTriggerEnter(Collider other)
    {
        if (IsWallLayer(other.gameObject.layer))
            isTouching = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (IsWallLayer(other.gameObject.layer))
            isTouching = false;
    }
}