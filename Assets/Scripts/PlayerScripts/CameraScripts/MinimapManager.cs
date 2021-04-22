using System;
using System.Collections;
using System.Collections.Generic;
using SzymonPeszek.PlayerScripts.Controller;
using UnityEngine;


public class MinimapManager : MonoBehaviour
{
    public InputHandler inputHandler;
    public Transform playerCameraPivotTransform;
    public Transform playerTargetTransform;
    public Transform minimapTransform;

    private void LateUpdate()
    {
        Vector3 currentPlayerPosition = playerTargetTransform.position;
        currentPlayerPosition.y = minimapTransform.position.y;
        minimapTransform.position = currentPlayerPosition;
    }
}
