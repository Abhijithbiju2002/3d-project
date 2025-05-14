using UnityEngine;

public class CameraFollow1 : MonoBehaviour
{
    [SerializeField] Transform followTarget;
    // [SerializeField] float Cam_distance = 5f;
    [SerializeField] float RotationSpeed = 1f;

    float y_rotation;
    float x_rotation;

    [SerializeField] float minVerticalAngle = -45f;
    [SerializeField] float maxVerticalAngle = 45f;

    [SerializeField] Vector2 FramingOffset;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    [SerializeField] float sprintCamDistance = 7f;
    [SerializeField] float walkCamDistance = 5f;
    [SerializeField] float zoomLerpSpeed = 5f;

    [SerializeField] float climbVerticalAngle = 15f;
    [SerializeField] float climbCamDistance = 6f;

    float currentCamDistance;


    float invertXVal;
    float invertYVal;

    [SerializeField] ParkourController controllerParkour;
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        currentCamDistance = walkCamDistance;

    }

    void Update()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) &&
              (Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f || Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f);//sprint
        bool isClimbing = Input.GetKey(KeyCode.LeftAlt) && controllerParkour.IsClimbing();//for climbing

        float targetDistance = walkCamDistance;
        if (isSprinting)
        {
            targetDistance = sprintCamDistance;//if sprint switch to sprint cam distance
        }
        else if (controllerParkour != null && controllerParkour.IsClimbing())
        {
            targetDistance = climbCamDistance;//if climbing switch to climb cam distance
            x_rotation = Mathf.Lerp(x_rotation, climbVerticalAngle, Time.deltaTime * zoomLerpSpeed);
        }
        // float targetDistance = isSprinting ? sprintCamDistance : walkCamDistance;//if sprinting 
        currentCamDistance = Mathf.Lerp(currentCamDistance, targetDistance, Time.deltaTime * zoomLerpSpeed);//Smooth camera distance change

        invertXVal = (invertX) ? -1 : 1;
        invertYVal = (invertY) ? -1 : 1;

        x_rotation += Input.GetAxis("Mouse Y") * invertYVal * RotationSpeed;
        x_rotation = Mathf.Clamp(x_rotation, minVerticalAngle, maxVerticalAngle);

        y_rotation += Input.GetAxis("Mouse X") * invertXVal * RotationSpeed;

        var targetRotation = Quaternion.Euler(x_rotation, y_rotation, 0);
        var focusPosition = followTarget.position + new Vector3(FramingOffset.x, FramingOffset.y); //cam focus to player at start

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, currentCamDistance);
        transform.rotation = targetRotation;

    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, y_rotation, 0);

    //or
    // public Quaternion GetPlanarRotation()
    //{
    //     return Quaternion.Euler(0, y_rotation, 0);
    // }
}
