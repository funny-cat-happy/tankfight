using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    private GameObject _player1;
    //游戏物体声明
    private GameObject BornMachine;
    private GameObject MissileAim;
    private GameObject MissileAirport;
    private GameObject BombAirport;
    private GameObject CarriageAirport;
    private GameObject _windStormTank;
    private GameObject _airdropAirport;
    private List<Vector2> AllBornPlace=new List<Vector2>();
    private Tilemap _tilemap;
    private TileBase _tileBase;
    //数据对象声明
    private DataManager.NormalTankData _normalTankData;
    public int[,] NavigationArray;
    private List<int> _exitEnergyTowerPosition=new List<int>();
    //敌人变量声明
    public int MaxEnemyTankNumber=10;
    public int currentEnemyTankNumber=0;


    private void Awake()
    {
        Instance = this;
        BornMachine = Resources.Load<GameObject>("Prefabs/BornMachine");
        _tileBase = Resources.Load<TileBase>("Palette/ground_0");
        MissileAim = Resources.Load<GameObject>("Prefabs/MissileAim");
        _tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        MissileAirport = Resources.Load<GameObject>(FilePath.AirportPath+"MissileAirport");
        BombAirport = Resources.Load<GameObject>(FilePath.AirportPath+"BombAirport");
        CarriageAirport = Resources.Load<GameObject>(FilePath.AirportPath+"CarriageAirport");
        _windStormTank = Resources.Load<GameObject>(FilePath.TankPath+"WindStormTank");
        _airdropAirport = Resources.Load<GameObject>(FilePath.AirportPath+"AirdropAirport");
        _normalTankData = DataManager.Instance.ReadJsonData<DataManager.NormalTankData>("Data/EnemyTank/NormalTank");
        _player1=GameObject.Find("Player1");
        NavigationArray=NavigationNetInit();
    }

    // private void Start()
    // {
        // BombAirportAttack(7);
        // CreateAirdropAirport(5);
    // }

    #region 坐标方法集
    
    private Vector2 CoordinateToPosition(int x, int y) //x:-11——11 y:-10——5
    {
        Vector2 position = new Vector2(0.5f+x,0.5f+y);
        return position;
    }
    private Vector2 CoordinateToPosition(float x, float y) 
    {
        Vector2 position = new Vector2(x,y);
        return position;
    }
    public int[] PositionToCoordinate(Vector2 position)
    {
        int[] Coordinate = new int[2];
        Coordinate[1] = (int)Mathf.Floor(position.x)+11;
        Coordinate[0] = (int)Mathf.Floor(position.y)+10;
        return Coordinate;
    }

    private float Vector2Distance(Vector2 VectorOne,Vector2 VectorTwo)
    {
        return Mathf.Sqrt(Mathf.Pow(VectorOne.x - VectorTwo.x,2.0f) +Mathf.Pow(VectorOne.y - VectorTwo.y,2.0f));
    }

    
    #endregion



    #region 游戏物体调用

    public void CreatePlayer()  //玩家位置初始化
    {
        int num = Random.Range(-11, 11);
        GameObject _gameObject=Instantiate(_windStormTank, CoordinateToPosition(num, -9.5f), Quaternion.Euler(0, 0, 0));
        _gameObject.transform.SetParent(_player1.transform);
    }
    
    
    
    public void CreateBornMachine()  //敌人传送点
    {
        int x,y;
        Vector2 TransportPositon;
        x = Random.Range(-11, 12);
        y = Random.Range(-1, 6);
        TransportPositon = CoordinateToPosition(x, y);
        while(_tilemap.HasTile(new Vector3Int(x,y,0))||(AllBornPlace.Contains(TransportPositon)))
        {
            x = Random.Range(-11, 12);
            y = Random.Range(-1, 6);
            TransportPositon = CoordinateToPosition(x, y);
        }
        AllBornPlace.Add(TransportPositon);
        Instantiate(BornMachine, TransportPositon, Quaternion.Euler(0, 0, 0));
    }

    public void BornMachineInit()
    {
        CreateBornMachine();
        Invoke(nameof(CreateBornMachine),5.0f);
        Invoke(nameof(CreateBornMachine),10.0f);
    }

    public void CreateAirdropAirport(int dropnumber)  //创建空投飞机
    {
        int j;//空投地点初始化
        int i;
        Vector2 _position;
        List<Vector2> AllDropPosition = new List<Vector2>();
        while (dropnumber>0)
        {
            i = Random.Range(-11, 12);
            j = Random.Range(-6, -1);
            _position = CoordinateToPosition(i, j);
            if (_tilemap.HasTile(new Vector3Int(i,j,0))==false&&(AllDropPosition.Contains(_position)==false))
            {
                AllDropPosition.Add(_position);
                dropnumber--;
            }
        }

        foreach (var eve in AllDropPosition)
        {
            GameObject airportGameObject=Instantiate(_airdropAirport, CoordinateToPosition(eve.x,8.5f), Quaternion.Euler(0, 0, 90));
            AirdropAirport _airport = airportGameObject.GetComponent<AirdropAirport>();
            _airport.DropPosition = eve;
        }
    }
    
    
    public void MissileAttack(int MissileNumber)
    {
        int j = Random.Range(-11, 12);
        //发射地点初始化
        int i = Random.Range(0, 10);
        Vector2 lanchPositionVector2 = new Vector2(j+0.5f, -2.5f+ i);
        //爆炸时间计算
        float ExplosionTime;
        float AirportY = 8.5f;
        float FlyDistance = AirportY - lanchPositionVector2.y;
        float FlyTime = FlyDistance / 0.1f*0.02f;
        float MissileFlyTime;
        //瞄准点初始化
        int x,y;
        List<Vector2> AllMissileAimPlace=new List<Vector2>();
        Vector2 positon;
        while (MissileNumber>0)
        {
            x = Random.Range(-11, 12);
            y = Random.Range(-10, 6);
            positon = CoordinateToPosition(x, y);
            if (_tilemap.HasTile(new Vector3Int(x,y,0))==false&&(AllMissileAimPlace.Contains(positon)==false))
            {
                AllMissileAimPlace.Add(positon);
                MissileNumber--;
            }
        }
        foreach (Vector2 eve in AllMissileAimPlace)
        {
            MissileFlyTime = Vector2Distance(eve, lanchPositionVector2)/ 0.1f * 0.02f;
            ExplosionTime = MissileFlyTime +FlyTime;
            GameObject gameObject = GameObject.Instantiate(MissileAim, eve, Quaternion.Euler(0, 0, 0));
            gameObject.GetComponent<MissileAim>().ExplosionTime = ExplosionTime;

        }
        //飞机初始化
        GameObject airportGameObject=GameObject.Instantiate(MissileAirport, CoordinateToPosition(j,8.5f), Quaternion.Euler(0, 0, 0));
        MissileAirport _airport = airportGameObject.GetComponent<MissileAirport>();
        _airport.AllMissilePosition = AllMissileAimPlace.ToArray();
        _airport.LaunchPosition = lanchPositionVector2;
    }

    public void BombAirportAttack(int BombNumber)
    {
        int i = Random.Range(-11, 12);
        GameObject _gameObject=Instantiate(BombAirport, CoordinateToPosition(i,8.5f), Quaternion.Euler(0, 0, 0));
        _gameObject.transform.GetComponent<BombAirport>().AirportLoad = BombNumber;

    }

    public void CreateEnergyTower()
    {
        int i = Random.Range(-11, 12);
        if (_exitEnergyTowerPosition.Count>23)
        {
            return;
        }
        while(_exitEnergyTowerPosition.Contains(i))
        {
            i = Random.Range(-11, 12);
        }
        _exitEnergyTowerPosition.Add(i);
        Instantiate(CarriageAirport, CoordinateToPosition(i,-12.5f), Quaternion.Euler(0, 0, 0));
    }
    #endregion


    //导航网络初始化
    private int[,] NavigationNetInit()
    {
        int[,] GridArray = new int[16,23];
        for (int i = 0; i < 16; i++)
        {
            for (int j = 0; j < 23; j++)
            {
                if (_tilemap.HasTile(new Vector3Int(j-11,i-10,0)))
                {
                    GridArray[i, j] = 1;
                }
                else
                {
                    GridArray[i, j] = 0;
                }
            }
        }

        return GridArray;
    }
}
