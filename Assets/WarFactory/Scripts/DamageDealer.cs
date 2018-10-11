using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer:MonoBehaviour{

    public int maxAmmunition = 100;
    public int currentAmmunition = 100;
    public GameObject bullet;
    public float shootForce = 10;

    public float attack = 1;
    public float attackSpeed = 1;
    public float attackRange = 10;

    public float ammoRatio()
    {
        return currentAmmunition / maxAmmunition;
    }
    public int ammoNeed()
    {
        return maxAmmunition - currentAmmunition;
    }

    public void AttackTarget(DamageReceiver target)
    {
        
        if (target != null)
        {
            //Debug.Log(this.tag + " ATTACKING " + target.gameObject.tag);
            float dmg = attack;
            Vector3 direction = (target.transform.position - transform.position).normalized;
            GameObject clone = Instantiate(bullet, transform.position, Quaternion.identity);

            clone.GetComponent<Rigidbody>().AddForce(direction*shootForce);
            Destroy(clone, 3);
            target.ReceiveDamage(this, dmg);
            currentAmmunition -= 1;
        }
    }
}
