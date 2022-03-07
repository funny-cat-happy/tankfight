using System;
using UnityEngine;

public class DeathBomb : MonoBehaviour
{
    private GameObject _bombFire;
    private void Awake()
    {
        _bombFire = Resources.Load<GameObject>(FilePath.BombPath+"BombFire");
    }

    private void BombDamage()
    {
        AudioManager.Instance.PlaySound("bombfire",AudioManager.AudioKind.tip);
        Instantiate(_bombFire,transform.position,Quaternion.Euler(0,0,0));
        Destroy(gameObject);
    }
}
