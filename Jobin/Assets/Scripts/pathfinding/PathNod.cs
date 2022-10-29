using Abed.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathNod 
{
    public gridG<PathNod> grid;
    public PathNod ExporldForm;
    public bool blocked=false;
    public int x;
    public int y;
    public int Gcost;
    public int Hcost;
    public int Fcost;
    public int sizei=20;
public PathNod(gridG<PathNod> grid,int width,int height )
    {
        this.grid = grid;
        this.x = width;
        this.y=height;
    }
    public void calculateFcost()
    {
        Fcost = Gcost + Hcost;
    }
    public override string ToString()
    {
        string name = (x + "||" + y);
        string fcostS = ("\n"+"F"+ Fcost );
        string gcostS = ("\n" + "G" + Gcost); ;
        string HcostS = ("\n" + "H" + Hcost); ;
        return name+fcostS+gcostS+HcostS.ToString();
    }
    public void SetBlock(bool isblock)
    {
        blocked = isblock;
    }
    public void SetBlock(bool isblock, bool showMesh)
    {
        blocked = isblock;
        if (showMesh)
        {
            GameObject blockedMeshVisual = new GameObject("block_Mesh_Visual", typeof(MeshFilter), typeof(MeshRenderer));
            Vector3 size = new Vector3(sizei, sizei);
            Vector3 position= grid.GetWorldPosition(x, y);
            Mesh Mymesh = MeshU.CreateQuaedMesh(size, position, Vector2.zero, Vector2.zero);
            blockedMeshVisual.GetComponent<MeshFilter>().mesh = Mymesh;   
        }
    }
}
