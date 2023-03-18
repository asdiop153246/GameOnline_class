using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class bombScript : NetworkBehaviour
{
    public Transform firePoint;
    public bombSpawner bombSpawner;
    public GameObject bombEffectPrefab;
    public float damage = 10f;
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;

        if (collision.gameObject.tag == "Player")
        {
            ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;            
            Debug.Log("Hit");           
            bombSpawner.DestroyServerRpc(networkObjectID);
            DealDamageServerRpc(networkObjectID, damage);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    void DealDamageServerRpc(ulong targetObjectId, float damage)
    {
        var targetObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[targetObjectId].gameObject;
        if (targetObject.CompareTag("Player"))
        {
            var health = targetObject.GetComponent<PlayerHealthSystem>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
    private void Update()
    {
        if (!IsOwner) return;
        //RaycastHit hit;
        //if (Physics.Raycast(firePoint.position, firePoint.forward, out hit))
        //{
        //    var hitNetworkObject = hit.transform.gameObject.GetComponent<NetworkObject>();
        //    if (hitNetworkObject != null)
        //    {
        //        DealDamageServerRpc(hitNetworkObject.NetworkObjectId, damage);
        //    }
        //}

        StartCoroutine(delaybeforeDestroy());

        IEnumerator delaybeforeDestroy()
        {
            yield return new WaitForSeconds(2);
            ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;
            bombSpawner.DestroyServerRpc(networkObjectID);
        }

    }
}