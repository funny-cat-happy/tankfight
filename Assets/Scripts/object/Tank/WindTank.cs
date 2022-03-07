using UnityEngine;
using UnityEngine.UI;

public class WindTank : BaseTank
{
    //技能
    public float CurrentChargeValue = 0;
    private float WindStormStatusLastTime;
    private float WindStormStatusSurplusTime;
    private bool isWindStormStatusCountdown=false;
    private float ChargeCapacity;
    private float MoveCharge;
    private float BulletCharge;
    private float HitCharge;
    private float WallCharge;

    public enum ChargeMethod
    {
        move,
        attackbullet,
        attackenemytank,
        wall
    }
    private float WindStormStatusZeroAttack;
    private float WindStormStatusZeroSpeed;
    private float WindStormStatusOneAttack;
    private float WindStormStatusOneSpeed;
    private float WindStormStatusTwoAttack;
    private float WindStormStatusTwoSpeed;
    private float WindStormStatusThreeAttack;
    private float WindStormStatusThreeSpeed;
    public bool isStrengthenDevice=false;
    public int currentStage = 0;
    //UI
    private GameObject _TankPanel;
    private Slider _slider;
    private Text _status;
    private Transform _surplusPanel;
    private Slider _surpluSlider;
    //装备
    public int ArmOne=2;
    public int ArmTwo=0;

    private DataManager.TankArmData _armData;
    //数据
    private DataManager.WindStormTankData _windStormTankData;

    protected override void Awake()
    {
        //数据初始化
        _windStormTankData=DataManager.Instance.ReadJsonData<DataManager.WindStormTankData>("Data/PlayerTank/WindStormTank");
        WindStormStatusLastTime = _windStormTankData.WindStromLastTime;
        speed = _windStormTankData.speed;
        AttackGap = _windStormTankData.AttackGap;
        heath = _windStormTankData.health;
        BulletDamage = _windStormTankData.damage;
        MoveCharge = _windStormTankData.MoveCharge;
        BulletCharge = _windStormTankData.BulletCharge;
        HitCharge = _windStormTankData.HitCharge;
        WallCharge = _windStormTankData.WallCharge;
        ChargeCapacity = _windStormTankData.ChargeCapacity;
        WindStormStatusZeroAttack = _windStormTankData.damage;
        WindStormStatusZeroSpeed = _windStormTankData.speed;
        WindStormStatusOneAttack=_windStormTankData.WindStormStatusOneAttack;
        WindStormStatusOneSpeed=_windStormTankData.WindStormStatusOneSpeed;
        WindStormStatusTwoAttack=_windStormTankData.WindStormStatusTwoAttack;
        WindStormStatusTwoSpeed=_windStormTankData.WindStormStatusTwoSpeed;
        WindStormStatusThreeAttack=_windStormTankData.WindStormStatusThreeAttack;
        WindStormStatusThreeSpeed=_windStormTankData.WindStormStatusThreeSpeed;
        WindStormStatusSurplusTime = WindStormStatusLastTime;
        //组件初始化
        base.Awake();
        TankBullet = Resources.Load<GameObject>(FilePath.BulletPath+"WindStormTankBullet");
        //装备读取
        _armData = DataManager.Instance.ReadJsonData<DataManager.TankArmData>("Data/PlayerTank/TankArm");
        ArmRead(ArmOne);
        ArmRead(ArmTwo);
        //UI初始化
        _TankPanel = GameObject.Find("WindStormPanel");
        _slider = _TankPanel.transform.Find("ChargeSlider").GetComponent<Slider>();
        _status = _TankPanel.transform.Find("StatusPanel").GetChild(1).GetComponent<Text>();
        _surplusPanel = _TankPanel.transform.Find("SurplusPanel");
        _surpluSlider = _surplusPanel.transform.Find("SurplusSlider").GetComponent<Slider>();
        _slider.maxValue = ChargeCapacity;
    }

    protected override void Start()
    {
        _damageAble.ShieldLayerChange("increase");
        _damageAble.health = heath;

    }

    protected override void Update()
    {
        base.Update();
        StatusIncrease();
        WindStormStatusCountdown();
    }

    protected override void TankMove()
    {
        base.TankMove();
        if (_rigidbody2D.velocity!=Vector2.zero)
        {
            DeviceCharge(ChargeMethod.move);
        }
    }

    #region 技能
    public void DeviceCharge(ChargeMethod method)
    {
        switch (method)
        {
            case ChargeMethod.move:
                CurrentChargeValue += MoveCharge;
                break;
            case ChargeMethod.attackbullet:
                CurrentChargeValue += BulletCharge;
                break;
            case ChargeMethod.attackenemytank:
                CurrentChargeValue += HitCharge;
                break;
            case ChargeMethod.wall:
                CurrentChargeValue += WallCharge;
                break;
        }

        if (CurrentChargeValue<0)
        {
            CurrentChargeValue = 0;
        }else if (CurrentChargeValue>ChargeCapacity)
        {
            CurrentChargeValue=ChargeCapacity;
        }
        _slider.value = CurrentChargeValue;
    }

    private void StatusIncrease()
    {
        if (playerinput.Instance.skill.Down)
        {
            if (CurrentChargeValue<ChargeCapacity||isWindStormStatusCountdown)
            {
                return;
            }
            else
            {
                _damageAble.ShieldLayerChange("increase");
                switch (currentStage)
                {
                    case 0:
                        CurrentChargeValue = 0;
                        speed = WindStormStatusOneSpeed;
                        BulletDamage = WindStormStatusOneAttack;
                        _status.text = "风暴状态I";
                        currentStage = 1;
                        break;
                    case 1:
                        CurrentChargeValue = 0;
                        speed = WindStormStatusTwoSpeed;
                        BulletDamage = WindStormStatusTwoAttack;
                        _status.text = "风暴状态II";
                        currentStage = 2;
                        if (!isStrengthenDevice)
                        {
                            isWindStormStatusCountdown = true;
                            _surplusPanel.gameObject.SetActive(true);
                        }
                        break;
                    case 2:
                        CurrentChargeValue = 0;
                        speed = WindStormStatusThreeSpeed;
                        BulletDamage = WindStormStatusThreeAttack;
                        _status.text = "风暴状态III";
                        currentStage = 3;
                        isWindStormStatusCountdown = true;
                        _surplusPanel.gameObject.SetActive(true);
                        break;
                }
                AudioManager.Instance.PlaySound("facethefate",AudioManager.AudioKind.tip);
            }
        }
    }

    private void WindStormStatusCountdown()
    {
        if (isWindStormStatusCountdown)
        {
            if (WindStormStatusSurplusTime>0)
            {
                WindStormStatusSurplusTime -= Time.deltaTime;
                _surpluSlider.value = WindStormStatusSurplusTime * 10;
            }
            else
            {
                if (isStrengthenDevice)
                {
                    speed = WindStormStatusZeroSpeed;
                    BulletDamage = WindStormStatusZeroAttack;
                    _status.text = "风暴状态0";
                    currentStage = 0;
                }
                else
                {
                    
                    speed = WindStormStatusOneSpeed;
                    BulletDamage = WindStormStatusOneAttack;
                    _status.text = "风暴状态I";
                    currentStage = 1;
                }
                WindStormStatusSurplusTime = WindStormStatusLastTime;
                isWindStormStatusCountdown = false;
                _surplusPanel.gameObject.SetActive(false);
                AudioManager.Instance.PlaySound("ChargeUsed",AudioManager.AudioKind.tip);
            }
        }
    }
    #endregion

    #region 装备
    private void ArmRead(int ArmId)
    {
        switch (ArmId)
        {
            case 1:
                ChargeCapacity = _armData.TankArm[ArmId-1].Num1;
                break;
            case 2:
                isStrengthenDevice = true;
                break;
            case 3:
                WallCharge = _armData.TankArm[ArmId-1].Num1;
                break;
        }
    }
    #endregion

    
    
}
