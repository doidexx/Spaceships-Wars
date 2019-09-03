using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 0;
    [SerializeField] GameObject hitFX;

    public int GetDamage()
    {
        return damage;
    }

    public void Hit()
    {
        Destroy(gameObject);
        GameObject HFX = Instantiate(hitFX, transform.position, Quaternion.identity);
        Destroy(HFX, 0.5f);
    }
}
