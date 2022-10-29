using Abed.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class HeatMapGenericVisual : MonoBehaviour
{
    gridG<HeatMapClass> grid;
    Mesh Mesh;
    Vector3[] vertciesT;
    Vector2[] uvT;
    int[] triangleT;
    bool updateMesh;
    public void SetGrid(gridG<HeatMapClass> grid)
    {
        this.grid = grid;
    }
    private void Start()
    {
        transform.position = Vector3.zero;   // this object should be in zero position
        Mesh = new Mesh();
        grid.OnValueChange += Grid_OnValueChange;
        GetComponent<MeshFilter>().mesh = Mesh;
        UpdateHeatMapMesh();
    }

    private void Grid_OnValueChange(object sender, gridG<HeatMapClass>.OnvalueChangeClass e)
    {
        updateMesh = true;
    }
    private void LateUpdate()
    {
        if (updateMesh)
        {
            UpdateHeatMapMesh();
            updateMesh = false;
        }
    }
    void UpdateHeatMapMesh()
    {
        int quadCount = grid.GetWidth() * grid.GetHeight();
        MeshU.CreateEmptyMeshArrys(quadCount, out vertciesT, out uvT, out triangleT);
        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                int indext = x * grid.GetHeight()+ y;
                Vector3 size = new Vector3(grid.GetCellSize(), grid.GetCellSize());
                HeatMapClass heatmapclass = grid.GetGridObject(x,y);
                float normalizeValueForUV = heatmapclass.GetValueNormalized();
                print(heatmapclass.GetValue()+"value||"+ heatmapclass.GetValueNormalized() + "normal");
                Vector2 uvByValue = new Vector2(normalizeValueForUV, normalizeValueForUV);
                MeshU.SetMeshArrays(indext, vertciesT, uvT, triangleT, size, grid.GetWorldPosition(x, y), uvByValue, uvByValue);
            }
        }
        Mesh.vertices = vertciesT;
        Mesh.uv = uvT;
        Mesh.triangles = triangleT;
    }
}
