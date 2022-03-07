using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTankBullet: bullet
{
    public WindTank WindTank;
    public float damage;
    protected override void Awake()
    {
        base.Awake();
        WindTank = GameObject.Find("Player1").transform.GetChild(0).GetComponent<WindTank>();
        speed = 7.0f;
    }

    private void Start()
    {
        _causedamage.damage = damage;
    }
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            speed = 0;
            _animator.SetTrigger("destory");
            WindTank.DeviceCharge(WindTank.ChargeMethod.wall);
            AudioManager.Instance.PlaySound("BulletHitWall");
            Destroy(gameObject,0.2f);
        }else if(other.CompareTag("Enemy"))
        {
            WindTank.DeviceCharge(WindTank.ChargeMethod.attackenemytank);
            _causedamage.OnDamage(other.gameObject);
            speed = 0;
            _animator.SetTrigger("destory");
            Destroy(gameObject,0.2f);
        }else if (other.CompareTag("enemy_bullet"))
        {
            speed = 0;
            _animator.SetTrigger("destory");
            WindTank.DeviceCharge(WindTank.ChargeMethod.attackbullet);
            Destroy(gameObject,0.2f);
        }
    }
}
