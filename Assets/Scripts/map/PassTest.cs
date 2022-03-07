using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassTest : MonoBehaviour
{
    public int x1;
    public int y1;
    public int x2;
    public int y2;
    private Grid Gridfrom;
    private Grid Gridto;
    private int[,] map;

    private void Awake()
    {
        Gridfrom = new Grid(x1,y1);
        Gridto = new Grid(x2,y2);
        map = new int[10,10];
    }

    private void Start()
    {
        GetLine();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Gridfrom = new Grid(x1,y1);
            Gridto = new Grid(x2,y2);
            map = new int[10,10];
            GetLine();
        }
    }

    private void GetLine()
    {
        int x = Gridfrom.GridX;
        int y = Gridfrom.GridY;
        bool inverted = false;
        int dx = Mathf.Abs(Gridto.GridX - Gridfrom.GridX);
        int dy = Mathf.Abs(Gridto.GridY - Gridfrom.GridY);
        int longstep;
        int shortstep;
        int allstep;
        float k;
        int gridsize = 1;
        float vertical = 0.5f;
        if (dy<dx)
        {
            longstep = (Gridto.GridX - Gridfrom.GridX) > 0 ? 1 : -1;
            shortstep = (Gridto.GridY - Gridfrom.GridY) > 0 ? 1 : -1;
            k = dx == 0 ? 0 : (float)dy / dx;
            allstep = dx;
        }
        else
        {
            inverted = true;
            longstep = (Gridto.GridY - Gridfrom.GridY) > 0 ? 1 : -1;
            shortstep = (Gridto.GridX - Gridfrom.GridX) > 0 ? 1 : -1;
            k = dy == 0 ? 0 : (float)dx / dy;
            allstep = dy;
        }
        for (int i = 0; i < allstep; i++)
        {
            map[x, y] = 1;
            vertical += k;
            if (inverted)
            {
                y += longstep;
                if (vertical>=gridsize)
                {
                    x += shortstep;
                    gridsize++;
                }
            }
            else
            {
                x += longstep;
                if (vertical>=gridsize)
                {
                    y += shortstep;
                    gridsize++;
                }
            }
        }
    }

    private struct Grid
    {
        public int GridX;
        public int GridY;

        public Grid(int x,int y)
        {
            GridX = x;
            GridY = y;
        }
    }

    private void OnDrawGizmos()
    {
        if (map != null)
        {
            Vector3 pos;
            for (int x = 0; x < 10; x ++) {
                for (int y = 0; y < 10; y ++) {
                    Gizmos.color = (map[x,y] == 1)?Color.black:Color.white;
                    pos = new Vector3(-5 + x + .5f,0, -5 + y+.5f);
                    Gizmos.DrawCube(pos,Vector3.one);
                }
            }
            Gizmos.color = Color.red;
            pos = new Vector3(-5 + Gridfrom.GridX + .5f,0, -5 + Gridfrom.GridY+.5f);
            Gizmos.DrawCube(pos,Vector3.one);
            Gizmos.color = Color.yellow;
            pos = new Vector3(-5 + Gridto.GridX + .5f,0, -5 + Gridto.GridY+.5f);
            Gizmos.DrawCube(pos,Vector3.one);
        }
    }
}
