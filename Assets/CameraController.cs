using System;
using UnityEngine;


//I stole this almost directly from https://gist.github.com/KarlRamstedt/407d50725c7b6abeaf43aee802fdd88e 
public class CameraController : MonoBehaviour
{
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    [Tooltip("Limits horizontal camera rotation. Useful for vehicles that do not allow free camera rotation.")]
    [Range(0f, 90f)][SerializeField] float xRotationLimit = 35f;

    [SerializeField] Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X"; //Strings in direct code generate garbage, storing and re-using them creates no garbage
    const string yAxis = "Mouse Y";

    [SerializeField] PlayerController player;

    void Update()
    {
        if(player.InVehicle) 
        { 
            DoVehicleCameraControls();
        }
        else
        {
            DoNormalCameraControls();
        }
    }

    void DoVehicleCameraControls()
    {
        Vector2 mouseDelta = Input.GetAxis(xAxis) * Vector2.right + Input.GetAxis(yAxis) * Vector2.up;

        //add mouse movement to both axes of rotation
        rotation.x += mouseDelta.x * sensitivity;
        //Note that we DON'T keep our rotation between 0-360 here. Doing so would cause weird snapping if we move the mouse too fast. Should be fine, anyway. 

        rotation.y += mouseDelta.y * sensitivity;

        //clamp up-down to ~90 degrees so we dont flip the camera
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        //Create a quaternion to represent the rotation on each axis
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        //Quaternion.AngleAxis gives us a gimbal-lock-free clamping method in the form of AngleAxis
        var xQuat = Quaternion.RotateTowards(player.AttachedVehicle.rotation, Quaternion.AngleAxis(rotation.x, Vector3.up), xRotationLimit);

        //To keep our stored rotation the appropriate value, steal the euler y component of the xquat.
        rotation.x = xQuat.eulerAngles.y;

        transform.localRotation = yQuat; 
        player.transform.localRotation = xQuat;
    }

    void DoNormalCameraControls()
    {
        Vector2 mouseDelta = Input.GetAxis(xAxis) * Vector2.right + Input.GetAxis(yAxis) * Vector2.up;

        rotation.x += mouseDelta.x * sensitivity;
        //Keep our rotation in the range 0-360.
        //THIS was the source of a lot of our problems. if we rotated the camera to the left before walking into the car, we would have an xRotation of ~-350
        //That's less than -35, so even though we're looking almost completely forward, we snap to the left side of the view.
        if (rotation.x < 0) rotation.x += 360f;
        if (rotation.x > 360f) rotation.x -= 360f;
        rotation.y += mouseDelta.y * sensitivity;

        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);

        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = yQuat; //Quaternions seem to rotate more consistently than EulerAngles. Sensitivity seemed to change slightly at certain degrees using Euler. transform.localEulerAngles = new Vector3(-rotation.y, rotation.x, 0);
        player.transform.localRotation = xQuat;
    }
}
