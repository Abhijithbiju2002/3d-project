using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    [SerializeField] float Cam_distance = 5f;
    [SerializeField] float RotationSpeed = 1f;

    float y_rotation;
    float x_rotation;

    [SerializeField] float minVerticalAngle = -45f;
    [SerializeField] float maxVerticalAngle = 45f;

    [SerializeField] Vector2 FramingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float invertXVal;
    float invertYVal;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        x_rotation += Input.GetAxis("Mouse Y") * invertYVal * RotationSpeed;
        x_rotation = Mathf.Clamp(x_rotation, minVerticalAngle, maxVerticalAngle);

        y_rotation += Input.GetAxis("Mouse X") * invertXVal * RotationSpeed;

        var targetRotation = Quaternion.Euler(x_rotation, y_rotation, 0);
        var focusPosition = followTarget.position + new Vector3(FramingOffset.x, FramingOffset.y); //cam focus to player at start

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, Cam_distance);
        transform.rotation = targetRotation;

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, y_rotation, 0);

    //or
    // public Quaternion GetPlanarRotation()
    //{
    //     return Quaternion.Euler(0, y_rotation, 0);
    // }
}
