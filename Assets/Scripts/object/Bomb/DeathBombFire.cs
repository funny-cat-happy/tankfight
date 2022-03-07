using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBombFire : MonoBehaviour
{
    private AttackRange _attackRange;
    private CauseDamage _causeDamage;
    public float damagevalue=2.0f;

    private void Awake()
    {
        _attackRange = transform.GetComponent<AttackRange>();
        _causeDamage = transform.GetComponent<CauseDamage>();
    }

    private void Start()
    {
        _causeDamage.damage=damagevalue;
    }

    private void CauseDamageToPlayer()
    {
        _causeDamage.OnDamage(_attackRange.GetDamageableObject());
        Destroy(gameObject);
    }
}
