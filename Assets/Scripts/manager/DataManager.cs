using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public NormalTankData currentNormalTankData;
    private static DataManager _instance=null;
    public static DataManager Instance 
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataManager();
            }
            return _instance;
        }
    }

    DataManager()
    {
        NormalTankDataInit();
    }
    


    public T ReadJsonData<T>(string path)
    {
        string JsonData=Resources.Load<TextAsset>(path).text;
        T DataObject = JsonUtility.FromJson<T>(JsonData);
        return DataObject;
    }

    private void NormalTankDataInit()
    {
        currentNormalTankData= ReadJsonData<NormalTankData>("Data/EnemyTank/NormalTank");
    }

    


    #region 玩家坦克数据类
    [Serializable]
    public class WindStormTankData
    {
        public int id;
        public string TankName;
        public string SkillName;
        public string SkillIntroduction;
        public string FriendSkillName;
        public string FriendSkillIntroduction;
        public float AttackGap;
        public float speed;
        public int damage;
        public float health;
        public int WindStormStatusOneAttack;
        public float WindStormStatusOneSpeed;
        public int WindStormStatusTwoAttack;
        public float WindStormStatusTwoSpeed;
        public int WindStormStatusThreeAttack;
        public float WindStormStatusThreeSpeed;
        public float ChargeCapacity;
        public float MoveCharge;
        public float BulletCharge;
        public float HitCharge;
        public float WallCharge;
        public float WindStromLastTime;
    }
    #endregion

    #region 坦克装备数据类
    [Serializable]
    public class TankArmItem
    {
        public int id;
        public string ArmName;
        public int ArmOwner;
        public bool isHeavyArm;
        public string ArmIntroduction;
        public float Num1;
        public float Num2;
    }
    [Serializable]
    public class TankArmData
    {
        public List <TankArmItem> TankArm=new List<TankArmItem>();
    }

    

    #endregion

    #region 敌人坦克数据类
    [Serializable]
    public class NormalTankData
    {
        public int id;
        public string TankName;
        public float AttackGap;
        public float speed;
        public float damage;
        public float health;
    }
    #endregion
}
