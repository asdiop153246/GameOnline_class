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
    [ServerRpc(RequireOwnership = false)]
    public void TakeDamageServerRpc(float damageAmount)
    {        
        Debug.Log("Damage taken = " + damageAmount);
        if (isDamaged == false)
        {
            StartCoroutine(DelayHp(damageAmount));
            Debug.Log("Your hp is =" + currentHealth);
        }        
        
        if (currentHealth <= 0f)
        {
            Debug.Log("You're dead");
            this.GetComponent<PlayerSpawnScript>().Respawn();
            currentHealth = maxHealth;
        }
    }
    public void TakeDamage(float damage)
    {
        Debug.Log("In TakeDamage");
        if (!IsOwner)
        {
            return;
        }

        TakeDamageServerRpc(damage);
    }

    IEnumerator DelayHp(float damageAmount)
    {
        isDamaged = true;
        currentHealth -= damageAmount;
        yield return new WaitForSeconds(1f);
        isDamaged = false;
    }


}
