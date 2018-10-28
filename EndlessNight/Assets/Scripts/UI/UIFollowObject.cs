using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowObject : MonoBehaviour {

    public Transform Target;  // Object that this label should follow
    public Vector3 Offset = Vector3.zero;    // Units in world space to offset;

    [Header("Camera")]
    public bool UseMainCamera = true;   // Use the camera tagged MainCamera
    public Camera CameraToUse;   // Only use this if useMainCamera is false
    private Camera m_camera;

    void Start()
    {
        if (UseMainCamera)
            m_camera = Camera.main;
        else
            m_camera = CameraToUse;

    }


    void Update()
    {
        Vector3 screenPosition = m_camera.WorldToScreenPoint(Target.position);
        this.transform.position = screenPosition + Offset;
    }
}