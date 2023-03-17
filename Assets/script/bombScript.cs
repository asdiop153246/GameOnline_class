using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class bombScript : NetworkBehaviour
{
    public bombSpawner bombSpawner;
    public GameObject bombEffectPrefab;
    public float damage = 10f;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        PlayerHealthSystem playerHealth = collision.gameObject.GetComponent<PlayerHealthSystem>();
        if (collision.gameObject.tag == "Player")
        {
            ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;
            Debug.Log("Hit");
            bombSpawner.DestroyServerRpc(networkObjectID);            
            playerHealth.TakeDamageClientRpc(damage);                   
        }

    }
    
    private void Update()
    {
        if (!IsOwner) return;
        StartCoroutine(delaybeforeDestroy());
    }

    IEnumerator delaybeforeDestroy()
    {
        yield return new WaitForSeconds(2);
        ulong networkObjectID = GetComponent<NetworkObject>().NetworkObjectId;
        bombSpawner.DestroyServerRpc(networkObjectID);
    }

}