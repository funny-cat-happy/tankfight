using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBomb : MonoBehaviour
{
    private CauseDamage _causeDamage;
    private AttackRange _attackRange;
    private GameObject[] _gameObjects;
    private void Awake()
    {
        _causeDamage = transform.GetComponent<CauseDamage>();
        _attackRange = transform.GetComponent<AttackRange>();
    }

    private void BombDamage()
    {
        _gameObjects = _attackRange.GetDamageableObject();
        AudioManager.Instance.PlaySound("bigexplosion");
        _causeDamage.OnDamage(_gameObjects);
        Destroy(gameObject,0.1f);
    }
    
}
