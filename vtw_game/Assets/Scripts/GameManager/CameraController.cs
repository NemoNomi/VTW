using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    private Vector3 cameraTargetPosition;

    void Update()
    {
        CalculateCameraTargetPosition();
        UpdateCameraTargetPosition();
    }

    void CalculateCameraTargetPosition()
    {
        cameraTargetPosition = (player1.position + player2.position) / 2f;
    }

    void UpdateCameraTargetPosition()
    {
        transform.position = cameraTargetPosition;
    }
}
