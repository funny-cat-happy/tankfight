using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    private List<GameObject> _damageAbles = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DamageAble damageAble = other.transform.GetComponent<DamageAble>();
            if (damageAble!=null)
            {
                _damageAbles.Add(damageAble.gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DamageAble damageAble = other.transform.GetComponent<DamageAble>();
            if (damageAble!=null)
            {
                if (!_damageAbles.Contains(damageAble.gameObject))
                {
                    _damageAbles.Add(damageAble.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(transform.tag))
        {
            DamageAble damageAble = other.transform.GetComponent<DamageAble>();
            if (damageAble!=null)
            {
                _damageAbles.Remove(damageAble.gameObject);
            }
        }
    }

    public GameObject[] GetDamageableObject()
    {
        return _damageAbles.ToArray();
    }
}