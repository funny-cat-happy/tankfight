using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NormalTank : MonoBehaviour
{
    public float speed=3.0f;
    private float SpeedX=0;
    private float SpeedY=0;
    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Transform bullet_position;
    private GameObject RedBullet;
    private Animator _animator;
    private DamageAble _damageAble;
    private BoxCollider2D _boxCollider2D;
    private enum TankStatus
    {
        born,
        norml,
        dead
    }
    private TankStatus CurrentTankStatus;
    public enum MoveStatus
    {
        still,
        patrol,
        search,
        hunt
    }
    public MoveStatus CurrentMoveStatus;
    private string TankDirection;
    private bool ChangePosition = true;
    public float ChangePositionGap=0.5f;
    private int RandonNum=0;
    private int[,] NavigationArray;
    //导航变量
    private AstarManager _AstarManager;
    private Transform _playerTransform;
    private int[] _currentTankPosition;
    private int[] _huntPosition;
    private List<AstarNode> _movePath;
    private Vector2 _nextPath;
    private bool isCanMove=false;
    //坦克游戏属性
    public float AttackGap=1f;
    private bool AttackStatus=true;
    public float AttackPower = 1;
    private float CurrentAttackPower;
    private void Awake()
    {
        //组件初始化
        _rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        bullet_position = transform.Find("bullet_position");
        RedBullet = Resources.Load<GameObject>(FilePath.BulletPath+"red_bullet_enemy");
        _animator = transform.GetComponent<Animator>();
        _damageAble = transform.GetComponent<DamageAble>();
        _boxCollider2D = transform.GetComponent<BoxCollider2D>();
        _playerTransform = GameObject.Find("Player1").transform.GetChild(0).transform;
        //方向初始化
        DirectionMove("up");
        //导航管理器初始化
        _AstarManager = new AstarManager();
        CurrentAttackPower = AttackPower;
    }
    
    void Start()
    {
        _damageAble.OnDead += TankDead;
        _damageAble.health = 1;
        _damageAble.isProtected = true;
        //数据初始化
        speed = DataManager.Instance.currentNormalTankData.speed;
        NavigationArray = MapManager.Instance.NavigationArray;
        //导航初始化
        _AstarManager.InitMap(NavigationArray);
        _huntPosition = new int[2] {-1, -1};
        CurrentMoveStatus = MoveStatus.patrol;
        //BUFF初始化
        BuffManager.Instance.EnemyMoveStatus += MoveStatusChange;
    }
    void Update()
    {
        if (CurrentTankStatus==TankStatus.born)
        {
            return;
        }
        if (CurrentTankStatus==TankStatus.norml)
        {
            //TankMove();
            TankAttack();
        }
    }

    #region 坦克移动
    private void TankMove()
    {
        switch (CurrentMoveStatus)
        {
            case MoveStatus.still:
                break;
            case MoveStatus.patrol:
                _rigidbody2D.velocity = new Vector2(SpeedX, SpeedY);
                TankPatrol();
                break;
            case MoveStatus.search:
                _rigidbody2D.velocity = new Vector2(SpeedX, SpeedY);
                break;
            case MoveStatus.hunt:
                TankHunt();
                break;
        }
    }

    private void TankPatrol()
    {
        if (ChangePosition)
        {
            RandonNum = Random.Range(0, 8);
            ChangePosition = false;
            Invoke("ResetChangePosition",ChangePositionGap);
        }
        
        if (RandonNum>=0&&RandonNum<3)
        {
            DirectionMove("down");
        }else if (RandonNum>=3&&RandonNum<5)
        {
            DirectionMove("left");
        }else if (RandonNum>=5&&RandonNum<7)
        {
            DirectionMove("right");
        }else if (RandonNum==7)
        {
            DirectionMove("up");
        }
    }

    private void ResetChangePosition()
    {
        ChangePosition = true;
    }
    private void DirectionMove(string direction)
    {
        if (direction=="up")
        {
            SpeedX = 0;
            SpeedY = speed;
            transform.rotation=Quaternion.Euler(0,0,0);
            TankDirection = "up";
        }else if (direction=="down")
        {
            SpeedX = 0;
            SpeedY = -speed;
            transform.rotation=Quaternion.Euler(0,0,180);
            TankDirection = "down";
        }else if (direction=="left")
        {
            SpeedX = -speed;
            SpeedY = 0;
            transform.rotation=Quaternion.Euler(0,0,90);
            TankDirection = "left";
        }else if (direction=="right")
        {
            SpeedX = speed;
            SpeedY = 0;
            transform.rotation=Quaternion.Euler(0,0,270);
            TankDirection = "right";
        }
    }
    
    private void OnCollisionStay2D(Collision2D other)
    {
        if (CurrentMoveStatus==MoveStatus.search)
        {
            if (other.collider.CompareTag("wall"))
            {
                if (TankDirection=="down")
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
                    RandonNum = Random.Range(0, 2);
                    if (RandonNum==0)
                    {
                        DirectionMove("left");
                    }
                    else
                    {
                        DirectionMove("right");
                    }
                }
                else if (TankDirection=="left")
                {
                    transform.position = new Vector3(transform.position.x+0.1f, transform.position.y, transform.position.z);
                    RandonNum = Random.Range(0, 3);
                    if (RandonNum==0)
                    {
                        DirectionMove("right");
                    }
                    else
                    {
                        DirectionMove("down");
                    }
                }
                else if (TankDirection=="right")
                {
                    transform.position = new Vector3(transform.position.x-0.1f, transform.position.y, transform.position.z);
                    RandonNum = Random.Range(0, 3);
                    if (RandonNum==0)
                    {
                        DirectionMove("up");
                    }
                    else
                    {
                        DirectionMove("down");
                    }
                }
                else if (TankDirection=="up")
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y-0.1f, transform.position.z);
                    DirectionMove("down");
                }
            }else if (other.collider.CompareTag("Enemy"))
            {
                if (TankDirection=="down")
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y+0.1f, transform.position.z);
                    DirectionMove("up");
                }
                else if (TankDirection=="left")
                {
                    transform.position = new Vector3(transform.position.x+0.1f, transform.position.y, transform.position.z);
                    DirectionMove("right");
                }
                else if (TankDirection=="right")
                {
                    transform.position = new Vector3(transform.position.x-0.1f, transform.position.y, transform.position.z);
                    DirectionMove("left");
                }
                else if (TankDirection=="up")
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y-0.1f, transform.position.z);
                    DirectionMove("down");
                }
            }
        }
    }

    private void TankHunt()
    {
        if (isCanMove)   //是否移动完下一格
        {
            if (!transform.position.Equals(_nextPath))
            {
                ChangeHuntDirection(new int[2]{_movePath[1].x,_movePath[1].y});
                transform.position=Vector2.MoveTowards(transform.position, _nextPath, 0.04f);
            }
            else
            {
                isCanMove = false;
            }
        }
        else
        {
            _currentTankPosition=MapManager.Instance.PositionToCoordinate(transform.position);
            _huntPosition=MapManager.Instance.PositionToCoordinate(_playerTransform.position);
            _movePath = _AstarManager.FindPath(_currentTankPosition, _huntPosition);
            if (_movePath==null||_movePath.Count<1)
            {
                CurrentMoveStatus = MoveStatus.search;
            }
            _nextPath=new Vector2(_movePath[1].y - 10.5f, _movePath[1].x - 9.5f);
            isCanMove = true;
        }
    }

    private void ChangeHuntDirection(int[] _nextPath)
    {
        if (_currentTankPosition[0]==_nextPath[0])
        {
            if (_currentTankPosition[1]>=_nextPath[1])
            {
                transform.rotation=Quaternion.Euler(0,0,90);
                TankDirection = "left";
            }
            else
            {
                transform.rotation=Quaternion.Euler(0,0,270);
                TankDirection = "right";
            }
        }else if (_currentTankPosition[1]==_nextPath[1])
        {
            if (_currentTankPosition[0]>=_nextPath[0])
            {
                transform.rotation=Quaternion.Euler(0,0,180);
                TankDirection = "down";
            }
            else
            {
                transform.rotation=Quaternion.Euler(0,0,0);
                TankDirection = "up";
            }
        }
    }
    #endregion

    #region 坦克攻击
    private void TankAttack()
    {
        if (AttackStatus==false)
        {
            return;
        }

        if (AttackStatus)
        {
            GameObject gameObject = GameObject.Instantiate(RedBullet);
            gameObject.GetComponent<CauseDamage>().damage = CurrentAttackPower;
            gameObject.transform.position = bullet_position.position;
            gameObject.transform.rotation=Quaternion.Euler(0,0,transform.rotation.eulerAngles.z);
            AttackStatus = false;
            AudioManager.Instance.PlaySound("GunFire",0);
            Invoke("ResetAttackStatus",AttackGap);
        }
    }

    private void ResetAttackStatus()
    {
        AttackStatus = true;
    }

    private void DamageChange(int damage)
    {
        if (damage>AttackPower)
        {
            _spriteRenderer.color = new Color(255, 150, 150, 255);
        }
        else
        {
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
        CurrentAttackPower = damage;
    }
    #endregion

    #region 坦克状态
    private void TankBornFinish()
    {
        CurrentTankStatus = TankStatus.norml;
        _damageAble.isProtected = false;
        AudioManager.Instance.PlaySound("transport");
    }
    private void TankDead()
    {
        _boxCollider2D.enabled = false;
        CurrentMoveStatus = MoveStatus.still;
        MapManager.Instance.currentEnemyTankNumber--;
        _animator.SetTrigger("dead");
        AudioManager.Instance.PlaySound("TankExplosion");
        Destroy(gameObject,0.6f);
    }
    #endregion


    #region 寻路算法

    //导航格子定义
    public class AstarNode
    {
        public float f;
        public float g;
        public float h;
        public AstarNode father;
        public int x;
        public int y;
        public bool type;

        public AstarNode(int x, int y, bool type)
        {
            this.x = x;
            this.y = y;
            this.type = type;
        }
    }
    
    //格子管理器
    public class AstarManager
    {
        private AstarNode[,] nodes;
        private List<AstarNode> openList=new List<AstarNode>();
        private List<AstarNode> closeList=new List<AstarNode>();
        private int MaxRow = 16;
        private int MinRow = 0;
        private int MaxCol = 23;
        private int MinCol = 0;
        public void InitMap(int[,] NavigationNetArray)
        {
            nodes = new AstarNode[MaxRow, MaxCol];
            for (int i = 0; i < MaxRow; i++)
            {
                for (int j = 0; j < MaxCol; j++)
                {
                    if (NavigationNetArray[i,j]==1)
                    {
                        nodes[i,j] = new AstarNode(i, j, false);
                    }
                    else
                    {
                        nodes[i,j] = new AstarNode(i, j, true);
                    }
                }
            }
        }

        public List<AstarNode> FindPath(int[] startpos, int[] endpos)
        {
            if (startpos[0] < MinRow || startpos[1] < MinCol || startpos[0] > MaxRow || startpos[1] > MaxCol ||
                endpos[0] < MinRow || endpos[1] < MinCol || endpos[0] > MaxRow || endpos[1] > MaxCol
            )
                return null;
            AstarNode start = nodes[startpos[0], startpos[1]];
            AstarNode end = nodes[endpos[0], endpos[1]];
            closeList.Clear();
            openList.Clear();
            start.father = null;
            start.g = 0;
            start.f = 0;
            start.h = 0;
            closeList.Add(start);
            while (true)  //此时的x代表行0-15，y代表列0-22
            {
                FindNearlyNode(start.x+1,start.y,1,start,end);
                FindNearlyNode(start.x-1,start.y,1,start,end);
                FindNearlyNode(start.x,start.y+1,1,start,end);
                FindNearlyNode(start.x,start.y-1,1,start,end);
                //无法找到路径
                if (openList.Count == 0)
                {
                    return null;
                }
                openList.Sort(SortOpenList);
                closeList.Add(openList[0]);
                start = openList[0];
                openList.RemoveAt(0);
                //找到路径
                if (start == end)
                {
                    List<AstarNode> path = new List<AstarNode>();
                    path.Add(end);
                    while (end.father!=null)
                    {
                        path.Add(end.father);
                        end = end.father;
                    }
                    path.Reverse();
                    return path;
                }
            }
        }

        private int SortOpenList(AstarNode a, AstarNode b)
        {
            if (a.f >= b.f)
                return 1;
            else
                return -1;
        }
        private void FindNearlyNode(int x, int y,float g,AstarNode father,AstarNode endpos)
        {
            if (x < MinRow || y < MinCol || x >= MaxRow || y >= MaxCol)
                return;
            AstarNode node = nodes[x, y];
            if(node==null||node.type==false||closeList.Contains(node)||openList.Contains(node))
                return;
            node.father = father;
            node.g = father.g + g;
            node.h = Mathf.Abs(endpos.x - x) + Mathf.Abs(endpos.y - y);
            node.f = node.g + node.h;
            openList.Add(node);
        }
    }

    #endregion

    #region BUFF方法

    private void MoveStatusChange(MoveStatus status)
    {
        CurrentMoveStatus = status;
    }
    

    #endregion
}
