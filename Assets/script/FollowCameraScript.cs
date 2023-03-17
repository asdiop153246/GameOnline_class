using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class FollowCameraScript : NetworkBehaviour
{
    public Transform followPlayer;
    private Transform cameraTransform;

    public Vector3 playerOffset;
    public float MoveSpeed = 400f;
 
    void Start()
    {
        cameraTransform = transform;
    }

    // Update is called once per frame
    public void SetTarget(Transform netTransformTarget)
    {
        followPlayer = netTransformTarget;
    }
    private void LateUpdate()
    {
        if (!IsOwner) return;

        if (followPlayer != null)
        {
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playerOffset, MoveSpeed * Time.deltaTime);
        }
    }
}
