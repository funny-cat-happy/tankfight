using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Vector2 TargetPosition;
    private float speed = 0.1f;
    void FixedUpdate()
    {
        MissileMove();
    }

    private void MissileMove()
    {
        transform.position = Vector2.MoveTowards(transform.position, TargetPosition, speed);
        if (transform.position.Equals(TargetPosition))
        {
            Destroy(gameObject);
        }
    }
}
