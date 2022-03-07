using UnityEngine;

public class CauseDamage : MonoBehaviour
{
    public float damage;
    public void OnDamage(GameObject gameObject)
    {
        DamageAble damageAble = gameObject.GetComponent<DamageAble>();
        if (damageAble == null)
        {
            return;
        }
        else
        {
            damageAble.healthdecrease(this.damage);
        }
    }
    public void OnDamage(GameObject[] gameObjects)
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            OnDamage(gameObjects[i]);
        }
    }
}
