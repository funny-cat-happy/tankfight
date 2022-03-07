using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MissileAirport : MonoBehaviour
{
    public float AirportSpeed=0.1f;
    public int AirportLoad = 2;
    private bool isAttack = true;
    private float AttackPosition;
    private GameObject missile;
    public Vector2[] AllMissilePosition;
    public Vector2 LaunchPosition;
    private void Awake()
    {
        AttackPosition = 11 / AirportLoad;
        missile = Resources.Load<GameObject>("Prefabs/Missile");
        AirportSpeed = 0.1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlaySound("MissileAirport",AudioManager.AudioKind.tip);
    }
    
    private void FixedUpdate()
    {
        AirportMove();
        MissileLaunch();
    }

    // private void FixedUpdate()
    // {
    //     throw new NotImplementedException();
    // }

    private void AirportMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, -12.5f), AirportSpeed);
        if (transform.position.y<=-12.5f)
        {
            Destroy(gameObject);
        }
    }
    
    private void MissileLaunch() //y:-7.5---2.5
    {
        if (isAttack)
        {
            if (transform.position.y<=LaunchPosition.y)
            {
                AudioManager.Instance.PlaySound("MissileFly",AudioManager.AudioKind.tip);
                foreach (Vector2 eve in AllMissilePosition)
                {
                    GameObject gameObject = Instantiate(missile);
                    gameObject.GetComponent<Missile>().TargetPosition = eve;
                    gameObject.transform.position = transform.position;
                    float seita = Mathf.Atan(Mathf.Abs(transform.position.x-eve.x) / Mathf.Abs(transform.position.y-eve.y))*180/Mathf.PI;
                    if (transform.position.y>eve.y&&transform.position.x>eve.x)
                    {
                        seita = 180 - seita;
                    }else if (transform.position.x<eve.x&&transform.position.y>eve.y)
                    {
                        seita = 180 + seita;
                    }else if (transform.position.x<eve.x&&transform.position.y<eve.y)
                    {
                        seita = 360 - seita;
                    }
                    gameObject.transform.rotation=Quaternion.Euler(0,0,seita);
                }

                isAttack = false;
            }
        }
    }
    
}
