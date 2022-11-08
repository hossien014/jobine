using Abed.RPF;
using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Rail_RPF
{
    List<RailPoint_RPF> RailPointList;
    List<Vector3> ways;
    Dictionary<float, RailPoint_RPF> RailDic;
    GameObject[] objs;
    Vector3[,] lines;
    int Cellsize = 3;
    Vector3 CellSizeVector;
    int id = 0;

    float Space;
    bool DbugeView = true;


    public Rail_RPF(Vector3 start, Vector3 end, int Cellsize)
    {
        this.Cellsize = Cellsize;
        // Initialization 
        Initialization();
        MakeRailPoint(start, end, Cellsize);
    }
    private void Initialization()
    {
        ways = new List<Vector3>();
        RailPointList = new List<RailPoint_RPF>();
        RailDic = new Dictionary<float, RailPoint_RPF>();
    }

    private void MakeRailPoint(Vector3 start, Vector3 end, int Cellsize)
    {
        this.Cellsize = Cellsize;
        CellSizeVector = new Vector3(Cellsize, Cellsize);
        if (start.x > end.x) Debug.LogError("Rail start input should be smaller then end input");
        float Cycle = Mathf.Abs(start.x - end.x);
        float slope = end.y - start.y;
        float numberToAdd = slope / Cycle;
        Debug.Log("count = " + (Cycle) + " slope = " + slope + " number to add = " + numberToAdd);
        if (DbugeView) Debug.DrawLine(start, end, Color.white, 1000);

        ways.Add(start);
        RailPointList.Add(new RailPoint_RPF(start, Cellsize, id));

        Vector3 currenVector = start;
        _Utils.DrawDebugSquer(start, Cellsize);
        bool stop = false;

        while (!stop)
        {
            id++;
            Vector3 nextPo = new Vector3(currenVector.x + Cellsize, currenVector.y + numberToAdd);
            currenVector = nextPo;
            Debug.Log(" neext pos = " + nextPo + " current pos " + currenVector);
            float CurrenCycle = currenVector.y - end.y;
            if (currenVector.x >= end.x) { ways.Add(end); stop = true; break; }
            //  Debug.Log("nex pos = " + nextPo);
            _Utils.DrawDebugSquer(nextPo, Cellsize);
            ways.Add(nextPo);
            RailPointList.Add(new RailPoint_RPF(nextPo, Cellsize, id));

            foreach (Vector3 w in ways) { Debug.Log("way" + w); }
        }
    }


    public Rail_RPF(string tag, int Cellsize)
    {
        Initialization();
        this.Cellsize = Cellsize;

        objs = GameObject.FindGameObjectsWithTag(tag);
        lines = new Vector3[objs.Length, 2];
        for (int i = 0; i < lines.GetLength(0); i++)
        {
            for (int j = 0; j < lines.GetLength(1); j++)
            {
                Vector3[] currentline = _Utils.GetColiderTopLine(objs[i]);
                lines[i, j] = currentline[j];
            }
        }
        for (int l = 0; l < lines.GetLength(0); l++)
        {
            MakeRailPoint(lines[l, 0], lines[l, 1], Cellsize);
            //  Debug.Log(lines[l, 0]+"__" + lines[l,0]);
        }
    }
    public Vector3[,] GetLines()
    {

        return lines;
    }
    public List<Vector3> GetWays()
    {
        return ways;
    }
    public RailPoint_RPF GetRailPoint(Vector3 position)
    {
        float key = position.x + position.y;
        RailDic.TryGetValue(key, out RailPoint_RPF result);
        return result;
    }
}
