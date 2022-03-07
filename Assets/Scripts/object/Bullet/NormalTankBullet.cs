using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTankBullet : bullet
{
    private static readonly int _destoryId = Animator.StringToHash("destory");

    protected override void Awake()
    {
        base.Awake();
        speed = 7.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "wall":
                speed = 0;
                _animator.SetTrigger(_destoryId);
                AudioManager.Instance.PlaySound("BulletHitWall");
                Destroy(gameObject,0.2f);
                break;
            case "Player":
            case "Radar":
            case "EnergyTower":
                _causedamage.OnDamage(other.gameObject);
                speed = 0;
                _animator.SetTrigger(_destoryId);
                Destroy(gameObject,0.2f);
                break;
            case "friend_bullet":
                speed = 0;
                _animator.SetTrigger(_destoryId);
                Destroy(gameObject,0.2f);
                break;
        }
    }
}
