using UnityEngine;

public class Adventure_FPSCameraPitch : MonoBehaviour
{
    public float sensitivity = 2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    float pitch = 0f;

    public CinemachineCameraTouchController camTouch;

    void Update()
    {
        float mouseY = camTouch.GetAxisCustom("Mouse Y");

        pitch -= mouseY * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localEulerAngles = new Vector3(-90, 0, pitch);
    }
}
