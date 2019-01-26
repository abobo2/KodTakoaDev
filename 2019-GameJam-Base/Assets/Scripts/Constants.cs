using System;
using UnityEngine;

public static class Constants
{
    public static Func<Vector2, Vector3> InputToMotionFunction => (v2) =>
    {
        Quaternion targetRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        return targetRotation * new Vector3(v2.x, 0, v2.y);
    };

    public static Func<Vector3, Vector3> CameraSpaceFunction => (v3) =>
    {
        Quaternion targetRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        return targetRotation * v3;
    };

	public static string InteractableTag = "Interactable";
	public static string PlayerTag = "Player";
}
