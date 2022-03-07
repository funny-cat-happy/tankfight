using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private DamageAble _damageAble;
    private Transform respair;
    private float BuildTime = 5.0f;
    private float FinishBuildTime = 0;
    private enum FactoryStatus
    {
        repair,
        normal
    }

    private FactoryStatus CurrentFactoryStatus;
    private Animator _animator;
    private void Awake()
    {
        _damageAble = transform.GetComponent<DamageAble>();
        _animator = transform.GetComponent<Animator>();
        respair = transform.GetChild(0);
        CurrentFactoryStatus = FactoryStatus.repair;
    }

    // Start is called before the first frame update
    void Start()
    {
        _damageAble.health = 2;
        _damageAble.OnDead += Dead;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentFactoryStatus == FactoryStatus.repair)
        {
            RepairFactory();
        }
    }

    private void RepairFactory()
    {
        FinishBuildTime += Time.deltaTime;
        if (FinishBuildTime>=BuildTime)
        {
            _damageAble.health = _damageAble.health + 3;
            respair.gameObject.SetActive(false);
            BuffManager.Instance.EnemyDamageChangeExecute(1.0f);
            CurrentFactoryStatus = FactoryStatus.normal;
        }
    }

    private void Dead()
    {
        _animator.SetTrigger("dead");
        BuffManager.Instance.EnemyDamageChange(1.0f);
        Destroy(gameObject,0.6f);
    }
}
