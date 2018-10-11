using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver:MonoBehaviour  {

    public float health = 100;
    public float defence = 1;
    public float organisation = 100;

    public DamageDealer attacker;

    public void ReceiveDamage(DamageDealer at, float damage)
    {
        attacker = at;
        //TODO: Make neater
        //Debug.Log("Damage Change: " + (damage - Mathf.Lerp(defence, 0, 1 - (organisation / 100))));
        health -= damage - Mathf.Lerp(defence, 0, (1-organisation / 100));
        organisation -= damage*Mathf.Lerp(4, 1, organisation / 100);
        //Debug.Log("Organisation Change: " + damage * Mathf.Lerp(4, 1, organisation / 100));
        if (health <= 0 || organisation <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
