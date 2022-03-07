using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileAim : MonoBehaviour
{
    private Animator _animator;
    private AttackRange _attackRange;
    private CauseDamage _causeDamage;
    private GameObject[] _gameObjects;
    public float ExplosionTime;
    private void Awake()
    {
        _animator = transform.GetComponent<Animator>();
        _attackRange = transform.GetComponent<AttackRange>();
        _causeDamage = transform.GetComponent<CauseDamage>();
    }

    private void Start()
    {
        Invoke("PlayAnimation",ExplosionTime);
    }

    private void PlayAnimation()
    {
        _animator.SetTrigger("explosion");
        AudioManager.Instance.PlaySound("TankExplosion");
    }
    

    private void ExplosionDamage()
    {
        _gameObjects = _attackRange.GetDamageableObject();
        _causeDamage.OnDamage(_gameObjects);
        Destroy(gameObject,0.2f);
    }
}
