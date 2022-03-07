using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Radar : MonoBehaviour
{
    //unity组件声明
    private DamageAble _damageAble;
    private Animator _animator;
    [Serializable]
    public enum RadarStatus
    {
        repair,
        normal,
        born
    }
    public RadarStatus currentRadarStatus;
    //修理变量声明
    private Transform respair;
    private float BuildTime = 5.0f;
    private float FinishBuildTime = 0;
    private Transform fire;
    //灯光变量声明
    private Light2D _light2D;
    public bool isLightUp;
    public bool isLightDown;
    private float BlackSpeed;
    private Light2D _playerLight;
    private bool isFinishing;
    private void Awake()
    {
        _light2D = transform.GetComponent<Light2D>();
        _damageAble = transform.GetComponent<DamageAble>();
        _animator = transform.GetComponent<Animator>();
        respair = transform.GetChild(1);
        currentRadarStatus = RadarStatus.born;
        _playerLight = GameObject.Find("Player1").transform.GetChild(0).GetChild(2).GetComponent<Light2D>();
        fire = transform.GetChild(2);
    }

    // Start is called before the first frame update
    void Start()
    {
        BlackSpeed = 0.01f;
        _damageAble.health = 2;
        _damageAble.OnDead += onDead;
    }

    // Update is called once per frame
    void Update()
    {
        LightChange();
        RadarStatusChange();
    }
    //全图灯光效果
    private void LightChange()
    {
        if (isLightDown)
        {
            _light2D.intensity = _light2D.intensity - BlackSpeed;
            if (_light2D.intensity <= 0)
            {
                isLightDown = false;
                return;
            }
        }
        if (isLightUp)
        {
            _light2D.intensity = _light2D.intensity + BlackSpeed;
            if (_light2D.intensity>=1)
            {
                isLightUp = false;
                return;
            }
        }
    }
    //修理
    private void AutoRepairRadar()
    {
        FinishBuildTime += Time.deltaTime;
        if (FinishBuildTime>=BuildTime)
        {
            _damageAble.health = _damageAble.health + 3;
            respair.gameObject.SetActive(false);
            fire.gameObject.SetActive(false);
            currentRadarStatus = RadarStatus.normal;
            isLightUp = true;
            _playerLight.gameObject.SetActive(false);
            FinishBuildTime = 0;
        }
    }

    private void PlayerRepairRadar()
    {
        if (isFinishing)
        {
            FinishBuildTime += Time.deltaTime;
            if (FinishBuildTime>=BuildTime)
            {
                _damageAble.health = _damageAble.health + 5;
                respair.gameObject.SetActive(false);
                currentRadarStatus = RadarStatus.normal;
                isLightUp = true;
                _playerLight.gameObject.SetActive(false);
                FinishBuildTime = 0;
                _damageAble.isProtected = false;
            }
        }
    }
    //状态切换
    private void RadarStatusChange()
    {
        if (currentRadarStatus==RadarStatus.born)
        {
            AutoRepairRadar();
        }
        else if(currentRadarStatus==RadarStatus.repair)
        {
            PlayerRepairRadar();
        }
    }
    
    
    //判断玩家进入
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentRadarStatus==RadarStatus.repair)
        {
            if (other.CompareTag("Player"))
            {
                _damageAble.isProtected = true;
                respair.gameObject.SetActive(true);
                fire.gameObject.SetActive(false);
                isFinishing = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentRadarStatus==RadarStatus.repair)
        {
            if (other.CompareTag("Player"))
            {
                isFinishing = false;
                respair.gameObject.SetActive(false);
                fire.gameObject.SetActive(true);
            }
        }
    }
    //雷达摧毁
    private void onDead()
    {
        _damageAble.isProtected = true;
        currentRadarStatus = RadarStatus.repair;
        fire.gameObject.SetActive(true);
        isLightDown = true;
        _playerLight.gameObject.SetActive(true);
        AudioManager.Instance.PlaySound("TankExplosion",AudioManager.AudioKind.tip);
    }
}
