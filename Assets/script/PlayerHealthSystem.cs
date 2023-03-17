using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerHealthSystem : NetworkBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    private bool isDamaged = false;
    MainPlayerScript mainPlayer;  
    public float damage = 10f;

    void Start()
    {
        if (!IsOwner) return;
        currentHealth = maxHealth;
        mainPlayer = GetComponent<MainPlayerScript>();        
    }
    [ClientRpc]
    public void TakeDamageClientRpc(float damageAmount)
    {
        if (!IsOwner) return;
        Debug.Log("Damage taken = " + damageAmount);
        if (isDamaged == false)
        {
            StartCoroutine(DelayHp(damageAmount));
            Debug.Log("Your hp is =" + currentHealth);
        }        
        
        if (currentHealth <= 0f)
        {
            this.GetComponent<PlayerSpawnScript>().Respawn();
            currentHealth = maxHealth;
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsOwner) return;
        if (collision.gameObject.tag == "Bullet")
        {
            TakeDamageClientRpc(damage);
        }
    }

    IEnumerator DelayHp(float damageAmount)
    {
        isDamaged = true;
        currentHealth -= damageAmount;
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }


}
