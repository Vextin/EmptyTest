# Unity-Relative-Rotation-Clamp-Example

This example project shows how to clamp the player's view relative to a specific object, for example a vehicle. 

The example scene allows the player to walk up to 2 cubes, clamping the player's view within 35 degrees of forward relative to the cube. 

The magic happens in [CameraController.cs](https://github.com/Vextin/Unity-Relative-Rotation-Clamp-Example/blob/main/Assets/CameraController.cs). Instead of using Mathf.Clamp to clamp the euler angle representation of the camera's rotation, we use `Quaternion.RotateTowards()` with the optional `float maxAngleDelta`. This prevents gimbal lock and errors from negative angles.
