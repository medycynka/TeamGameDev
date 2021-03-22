using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MinimapManager : MonoBehaviour
{
    public Transform playerCameraPivotTransform;
    public Transform minimapTransform;

    private void LateUpdate()
    {
        Vector3 newPosition = playerCameraPivotTransform.position;
        newPosition.y = minimapTransform.position.y;
        minimapTransform.position = newPosition;
        minimapTransform.rotation = Quaternion.Euler(0f, playerCameraPivotTransform.eulerAngles.y, 0f);
    }
}
