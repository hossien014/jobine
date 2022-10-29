using System.Collections;
using UnityEngine;
using Abed.Utils;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public class testHitmap : MonoBehaviour
{
   
    gridG<HeatMapClass> gridg;
    //    [SerializeField] HeatMapVisual heatmapvitual;
    //  [SerializeField] HeatMapBoolVisual heatmapboolvitual;
    [SerializeField] HeatMapGenericVisual heatMapGenericvitual;
    void Awake()
    {
        int cellsize = 5;
        gridg = new gridG<HeatMapClass>
       (30, 30,cellsize, new Vector3(-60, -25), (gridG<HeatMapClass> Tgrid,int x ,int y) => { return new HeatMapClass(Tgrid,x,y); });
        heatMapGenericvitual.SetGrid(gridg);
        StartCoroutine(counter());
       
    }
    private void Update()
    {
        updatheatmap();
    }
     IEnumerator counter()
    {
        while (true)
        {
            updatheatmap();
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void updatheatmap()
    {
        if (Input.GetMouseButtonDown(0))
        {
         //   bool value = gridg.GetValue(position);
         //   int addvlue = 10;
           // int fullyrange = 2;
       //     int totalrange = 10;
           // gridg.SetValue(position, true);

            Vector3 position = Utilis.GetMousePos();
            HeatMapClass Heatmapclass = gridg.GetGridObject(position);
            if (Heatmapclass != null)
            {
                Heatmapclass.addIntValue(5);

            }
        }
        Vector3 positionN = Utilis.GetMousePos();


        for(int i=0; i < Utilis.AlphabetKeycod().Length; i++)
        {
            if (Input.GetKeyDown(Utilis.AlphabetKeycod()[i]))
            {
                gridg.GetGridObject(positionN).addStringValue(Utilis.AlphabetKeyStrung()[i]);
            }
        }
    }
}
public class HeatMapClass{
    gridG<HeatMapClass> grid;
    int x;
    int y;
    int value;
    int Min_Value = 0;
    int Max_Value = 100;

    string stringValue = ("\n" + "null");

    public HeatMapClass(gridG<HeatMapClass> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void addIntValue(int addvalue)
    {
        value += addvalue;
        value = Mathf.Clamp(value, Min_Value, Max_Value);
        grid.TriggerGridObjectChanged(x, y);
    }
    public void addStringValue(string text)
    {
        stringValue = ("\n" + text);
        grid.TriggerGridObjectChanged(x, y);
    }
    public float GetValueNormalized()
    {
        return (float)value / Max_Value;
    }
    public int GetValue()
    {
        return value;
    }
    public override string ToString()
    {

        return value + stringValue.ToString();
    }


}