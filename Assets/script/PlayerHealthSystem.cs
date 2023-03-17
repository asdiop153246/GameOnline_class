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
    TMP_Text p1Text;
    TMP_Text p2Text;
    MainPlayerScript mainPlayer;

    void Start()
    {
        if (!IsOwner) return;
        currentHealth = maxHealth;
        p1Text = GameObject.Find("P1ScoreText").GetComponent<TMP_Text>();
        p2Text = GameObject.Find("P2ScoreText").GetComponent<TMP_Text>();
        mainPlayer = GetComponent<MainPlayerScript>();
    }
    [ClientRpc]
    public void TakeDamageClientRpc(float damageAmount)
    {
        Debug.Log("Damage taken = " + damageAmount);
        if (isDamaged == false)
        {
            StartCoroutine(DelayHp(damageAmount));
            Debug.Log(p1Text.text + " Hp = " + currentHealth);
            Debug.Log(p2Text.text + " Hp = " + currentHealth);
        }        
        
        if (currentHealth <= 0f)
        {
            GetComponent<PlayerSpawnScript>().Respawn();
            currentHealth = maxHealth;
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
