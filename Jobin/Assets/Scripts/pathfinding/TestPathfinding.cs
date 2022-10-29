using Abed.Utils;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class TestPathfinding : MonoBehaviour
{
    gridG<PathNod> Grid;
    PathFinding pathFinding;
    List<PathNod> BlockedNoodList;

    [SerializeField] Transform character;
    PathNod characterNode;

    Vector3 characterPos;
    Vector3 VectorSize;
    [SerializeField] int hieght=10;
    [SerializeField] int wideth=10;
    int cellsize;
    [SerializeField]bool DbugView = true;

    void Start()
    {
        BlockedNoodList = new List<PathNod>();
        transform.position = Vector2.zero;
        pathFinding = new PathFinding(wideth, hieght);
        Grid = pathFinding.GetGrid();
        cellsize = Grid.GetCellSize() / 2;
        VectorSize = new Vector3(cellsize, cellsize);
        PutCharctorInCenterOfNode();
    }
    private void PutCharctorInCenterOfNode()
    {
        PathNod CharStartNood = Grid.GetGridObject(character.position);
        Vector3 charstartPos = Grid.GetWorldPosition(CharStartNood.x, CharStartNood.y);
        charstartPos += VectorSize;
        FindObjectOfType<CharacterMovePathfinding>().setStart(charstartPos);
    }
    private void UpdateCharcterInfo()
    {
         characterPos = character.transform.position;
         characterNode = Grid.GetGridObject(characterPos);
    }
    private void Update()
    {
        UpdateCharcterInfo();
        IfClick_StartFindPathToMousePos();
        IfClick_BlockNode();
    }
    private void IfClick_StartFindPathToMousePos()
    {
        if (Input.GetMouseButtonDown(0)) // for path finding 
        {
            var MousePos = Utilis.GetMousePos();
            PathNod target = Grid.GetGridObject(MousePos);
            if(target.blocked!=true) StartPathFindingProsses(target.x, target.y, target);
        }
    }
    private void StartPathFindingProsses(int targetX,int targetY,PathNod target)
    {
        GetPath(targetX, targetY, target, out List<PathNod> path_N, out List<Vector3> path_V);
        if (DbugView) DrawLineForPath(path_N);
        FindObjectOfType<CharacterMovePathfinding>().Go(path_V, VectorSize);
    }
    private void GetPath(int targetX, int targetY, PathNod target, out List<PathNod> path_N, out List<Vector3> path_V)
    {
        path_N = pathFinding.FindPath(characterNode.x, characterNode.y, targetX, targetY);
        path_V = pathFinding.FindPath(character.position, Grid.GetWorldPosition(target.x, target.y) + VectorSize);
    }
    private void IfClick_BlockNode()
    {
        if (Input.GetMouseButtonDown(1)) // for block
        {
            Vector3 position = Utilis.GetMousePos();
            PathNod target = Grid.GetGridObject(position);
            target.SetBlock(true);
            if (!BlockedNoodList.Contains(target)) BlockedNoodList.Add(target);
            if(DbugView) CreatMeshForBlockList(BlockedNoodList, cellsize);
        }
    }
    void CreatMeshForBlockList(List<PathNod> blockednode, int cellsize)
    {
        MeshU.CreateEmptyMeshArrys(blockednode.Count, out Vector3[] vertciese, out Vector2[] uv, out int[] triangle);

        for (int i = 0; i < blockednode.Count; i++)
        {
            PathNod targetNode = blockednode[i];
            Vector3 position = Grid.GetWorldPosition(targetNode.x, targetNode.y);
            MeshU.SetMeshArrays(i, vertciese, uv, triangle, cellsize, position);
        }
        Mesh mymesh = new Mesh();
        mymesh.vertices = vertciese;
        mymesh.uv = uv;
        mymesh.triangles = triangle;
        GetComponent<MeshFilter>().mesh = mymesh;
    }
    private void DrawLineForPath(List<PathNod> path)
    {
        if (path != null)
            for (int i = 0; i < path.Count; i++)
            {
                if (i + 1 < path.Count)
                {
                    Vector3 start = path[i].grid.GetWorldPosition(path[i].x, path[i].y);
                    Vector3 end = path[i].grid.GetWorldPosition(path[i + 1].x, path[i + 1].y);
                    Debug.DrawLine(start+ VectorSize, end+ VectorSize, Color.green, 100f);
                }
                path[i].grid.TriggerGridObjectChanged(path[i].x, path[i].y);
            }
    }
}
