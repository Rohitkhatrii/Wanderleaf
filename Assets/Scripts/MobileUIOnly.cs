using UnityEngine;

public class MobileUIOnly : MonoBehaviour
{
    void Start()
    {
        // If the game is running on a Desktop PC, hide this object
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            gameObject.SetActive(false);
        }
    }
}