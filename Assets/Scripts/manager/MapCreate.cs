using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class MapCreate : MonoBehaviour
{
    private Tilemap _tilemap;
    private TileBase _tileBase;
    // public int WallNum; //小于128
    [Range(0,100)]
    public float SetProbability;
    [Range(0, 345)]
    public int WallNum;
    [Range(1,8)]
    public int AroundNum1;
    [Range(1,8)]
    public int AroundNum2;
    private void Awake() //x:-11~11 y:-10~5
    {
        _tilemap = transform.GetComponent<Tilemap>();
        _tileBase = Resources.Load<TileBase>("Palette/ground_0");
        RandomSwampWall();
        SmoothWall();

    }

    #region 随机游走算法产生地图
    private void RandomWalkSwampWall()
    {
        for (int i = -11; i <= 11; i++) 
        {
            for (int j = -9; j <= 5; j++) //最后一排不能放置障碍
            {
                _tilemap.SetTile(new Vector3Int(i,j,0),_tileBase);
            }
        }
        Stack _positionStack=new Stack();
        int ExistWallNum = 0;
        Vector3Int drawposition = new Vector3Int(0, 0, 0);
        while (ExistWallNum<WallNum)
        {
            _tilemap.SetTile(drawposition,null);
            ExistWallNum++;
            _positionStack.Push(drawposition);
            drawposition=FindNextMove(drawposition);
            if (drawposition==Vector3Int.zero)
            {
                drawposition = (Vector3Int)_positionStack.Pop();
                drawposition = FindNextMove(drawposition);
                while (drawposition==Vector3Int.zero&&_positionStack.Count>0)
                {
                    drawposition = (Vector3Int)_positionStack.Pop();
                    drawposition = FindNextMove(drawposition);
                }

                if (_positionStack.Count<=0)
                {
                    break;
                }
            }
        }
    }
    private Vector3Int FindNextMove(Vector3Int drawposition)
    {
        int num;
        List<Vector3Int> NextMoveList = new List<Vector3Int>();
        if (drawposition.x+1<=11)
        {
            NextMoveList.Add(new Vector3Int(drawposition.x+1,drawposition.y,0));
        }

        if (drawposition.y+1<=5)
        {
            NextMoveList.Add(new Vector3Int(drawposition.x,drawposition.y+1,0));
        }

        if (drawposition.x-1>=-11)
        {
            NextMoveList.Add(new Vector3Int(drawposition.x-1,drawposition.y,0));
        }

        if (drawposition.y-1>=-9)
        {
            NextMoveList.Add(new Vector3Int(drawposition.x,drawposition.y-1,0));
        }
        num = Random.Range(0, NextMoveList.Count);
        while (NextMoveList.Count>0&&!_tilemap.HasTile(NextMoveList[num]))
        {
            NextMoveList.RemoveAt(num);
            num = Random.Range(0, NextMoveList.Count);
        }

        if (NextMoveList.Count>0)
        {
            return NextMoveList[num];
        }
        else
        {
            return new Vector3Int(0,0,0);
        }
    }
    #endregion

    #region 细胞自动机产生地图
    private void RandomSwampWall()
    {
        for (int i = -11; i <= 11; i++) 
        {
            for (int j = -9; j <= 5; j++) //最后一排不能放置障碍
            {
                int k=Random.Range(0, 100);
                if (k<SetProbability)
                {
                    _tilemap.SetTile(new Vector3Int(i,j,0),_tileBase);
                }
            }
        }
    }

    private void SmoothWall()
    {
        for (int s = 0; s < 4; s++)
        {
            for (int i = 11; i >= -11; i--)  //更新障碍
            {
                for (int j = 5; j >= -10; j--) 
                {
                    Vector3Int _position = new Vector3Int(i, j, 0);
                    int wallonenum = 0;
                    int walltwonum = 0;
                    wallonenum=CheckOneWallNum(i, j);
                    walltwonum = CheckTwoWallNum(i, j);
                    if (wallonenum>=AroundNum1||walltwonum<=AroundNum2) //值取8,6得到的地图较为理想
                    {
                        _tilemap.SetTile(_position,_tileBase);
                    }
                    else
                    {
                        _tilemap.SetTile(_position,null);
                    }
                }
            }
        }
    }

    private void ClearMap()
    {
        for (int i = 11; i >= -11; i--)  //更新障碍
        {
            for (int j = 5; j >= -10; j--) 
            {
                Vector3Int _position = new Vector3Int(i, j, 0);
                _tilemap.SetTile(_position,null);
                
            }
        }
    }
    private int CheckOneWallNum(int i,int j)
    {
        int wallnum = 0;
        if (_tilemap.HasTile(new Vector3Int(i-1,j,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+1,j,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i,j-1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i,j+1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+1,j+1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-1,j+1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+1,j-1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-1,j-1,0)))
        {
            wallnum++;
        }
        return wallnum;
    }
    private int CheckTwoWallNum(int i,int j)
    {
        int wallnum = 0;
        wallnum = CheckOneWallNum(i, j);
        if (_tilemap.HasTile(new Vector3Int(i-2,j+2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-1,j+2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i,j+2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+1,j+2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+2,j+2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+2,j+1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+2,j,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+2,j-1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+2,j-2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i+1,j-2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i,j-2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-1,j-2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-2,j-2,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-2,j-1,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-2,j,0)))
        {
            wallnum++;
        }
        if (_tilemap.HasTile(new Vector3Int(i-2,j+1,0)))
        {
            wallnum++;
        }
        return wallnum;
    }
    #endregion
    private void OnGUI()
    {
        GUIStyle fontstyle = new GUIStyle();
        fontstyle.fontSize = 30;
        if (GUI.Button(new Rect(20,40,180,120),"重新生成",fontstyle))
        {
            
            ClearMap();
            RandomSwampWall();
            SmoothWall();
        }
    }
}