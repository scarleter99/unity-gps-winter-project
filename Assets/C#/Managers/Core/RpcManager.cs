using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RpcManager
{
    [ServerRpc]
    public void MoveGameObject(Transform transform, Vector3 dest, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, dest,
            NetworkManager.Singleton.ServerTime.FixedDeltaTime * speed);

        if (dest != transform.position)
        {
            Vector3 targetDirection = dest - transform.position;
            targetDirection.z = 0f;
            transform.forward = targetDirection;
        }
    }
}
