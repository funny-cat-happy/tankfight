using System;
using UnityEngine;

public class DamageAble:MonoBehaviour
{
    public float health;
    public Action OnDead;
    public Action OnHurt;
    public bool isProtected=false;
    public int ShieldLayer=0;
    //护盾变量声明
    private Transform _shield;
    private SpriteRenderer _shieldSpriteRenderer;

    private void Awake()
    {
        _shield = transform.Find("tank_shield");
        _shieldSpriteRenderer = _shield.GetComponent<SpriteRenderer>();
    }


    public void healthdecrease(float damage)
    {
        if(isProtected)
            return;
        if (ShieldLayer>0)
        {
            ShieldLayerChange("decrease");
            return;
        }
        health-=damage;
        if (health <= 0)
        {
            if (OnDead!=null)
            {
                OnDead();
            }
        }
        else
        {
            if (OnHurt!=null)
            {
                OnHurt();
            }
        }
    }
    
    public void ShieldLayerChange(string change)  //护盾处理
    {
        if (change=="increase")
        {
            ShieldLayer++;
        }
        else
        {
            ShieldLayer--;
            AudioManager.Instance.PlaySound("ShieldBroken",AudioManager.AudioKind.tip);
        }

        if (ShieldLayer>7)
        {
            ShieldLayer = 7;
        }
        if (ShieldLayer>0)
        {
            _shield.gameObject.SetActive(true);
        }
        else
        {
            _shield.gameObject.SetActive(false);
        }
        switch (ShieldLayer)
        {
            case 1:
                _shieldSpriteRenderer.color=new Color(1,0,0,98.0f/255);
                break;
            case 2:
                _shieldSpriteRenderer.color = new Color(1,128.0f/255,0.16f,98.0f/255);
                break;
            case 3:
                _shieldSpriteRenderer.color = new Color(1,1,0,98.0f/255);
                break;
            case 4:
                _shieldSpriteRenderer.color = new Color(1,128.0f/255,0.16f,98.0f/255);
                break;
            case 5:
                _shieldSpriteRenderer.color = new Color(0,1,1,98.0f/255);
                break;
            case 6:
                _shieldSpriteRenderer.color = new Color(0,0,1,98.0f/255);
                break;
            case 7:
                _shieldSpriteRenderer.color = new Color(128.0f/255,0,1,98.0f/255);
                break;
        }
    }
}
