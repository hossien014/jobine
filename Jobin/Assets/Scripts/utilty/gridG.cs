using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Abed.Utils
{  // Grid G is generiekc class 
    public class gridG<TGridObject>
    {
        public event EventHandler<OnvalueChangeClass> OnValueChange;
        public class OnvalueChangeClass : EventArgs { public int x; public int y; }

        Vector3 orgine;
        BoxCollider2D[] coliderArray;
        [SerializeField] Transform parent;
        TextMesh[,] DebugTexArray;
        Vector3[] dirctions;

        string name = "grid/xmh/";

        int width;
        int height;
       [SerializeField] TGridObject[,] gridArray;
        int cellSize = 5;
        int HitMap_Max_value = 100;
        int HitMap_Min_value = 0;

        public bool DeBugView = true;

        #region Class constracter
        private void SetInputToFillds(int width, int height, int cellSize, Vector3 orgine)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.orgine = orgine;
            gridArray = new TGridObject[width, height];
            DebugTexArray = new TextMesh[width, height];
        }
        public gridG(int width, int height, int cellSize, Vector3 orgine,Func<gridG<TGridObject>,int,int,TGridObject> creatGridTobject)
        {
            SetInputToFillds(width, height, cellSize, orgine);
            CreatNewTGridObjectForArray(creatGridTobject);
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Vector3 Localposition = GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f;
                    string text = "";
                    int fontsize = 30;
                    Utilis.createTextInWorld((name), parent, Localposition, text, fontsize, Color.red, TextAnchor.MiddleCenter);
                    //drew lines
                    if (DeBugView)
                    {
                        string Text = gridArray[x, y].ToString();
                        DebugTexArray[x, y] = Utilis.createTextInWorld((name), parent, Localposition, Text, fontsize, Color.white, TextAnchor.MiddleCenter);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 500);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 500);
                    }
                }
            }
            if (DeBugView)
            {
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 500);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 500);
            }
            OnValueChange += (object sender, OnvalueChangeClass arge) =>
            {
                DebugTexArray[arge.x, arge.y].text = gridArray[arge.x, arge.y].ToString();
            };
        }

        private void CreatNewTGridObjectForArray(Func<gridG<TGridObject>, int, int, TGridObject> creatGridTobject)
        {
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = creatGridTobject(this, x, y);
                }
            }
        }

        public gridG(int cellsize,string tag)
        {
            this.cellSize = cellsize;
            int groundCount = GameObject.FindGameObjectsWithTag(tag).Length;
            coliderArray = new BoxCollider2D[groundCount];
            for (int i = 0; i < groundCount; i++)
            {
                coliderArray[i] = GameObject.FindGameObjectsWithTag(tag)[i].transform.GetComponent<BoxCollider2D>();
                if (coliderArray[i] == null) continue;
                Vector3 coliderOregin = GetColiderOrgin(coliderArray[i]);
                int x; int y;
                GetColiderWidthAndHight(coliderArray[i], out x, out y);
               // new gridG(x, y, cellSize, coliderOregin);
            }
        }
        #endregion
        #region set
        public void SetValue(int x, int y, TGridObject value)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                gridArray[x, y] = value;
                if (DeBugView) { DebugTexArray[x, y].text = value.ToString(); }
                OnValueChange?.Invoke(this, new OnvalueChangeClass() { y = y, x = x });
            }
        }
        public void TriggerGridObjectChanged(int x ,int y)
        {
            if (DeBugView)
            {

            DebugTexArray[x, y].text = gridArray[x, y].ToString();
            }
            OnValueChange?.Invoke(this, new OnvalueChangeClass() { y =y, x = x }) ;
        }
        public void SetValue(Vector3 worldposition, TGridObject value)
        {
            //int x;
            //int y;
            GetXY(worldposition, out int x, out int y);
            SetValue(x, y, value);
        }
        #endregion
        #region Get
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + orgine;
        }
       public void GetXY(Vector3 worldpositon, out int x, out int y)
        {
            x = Mathf.FloorToInt((worldpositon - orgine).x / cellSize);
            y = Mathf.FloorToInt((worldpositon - orgine).y / cellSize);
        }
        public TGridObject GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                return gridArray[x, y];
            }
            return default(TGridObject);
        }
        public TGridObject GetGridObject(Vector3 worldPostion)
        {
            int x;
            int y;
            GetXY(worldPostion, out x, out y);
            GetGridObject(x, y);
            return GetGridObject(x, y);
        }
        public int GetHeight()
        {
            return height;
        }
        public int GetWidth()
        {
            return width;
        }
        public int GetCellSize()
        {
            return cellSize;
        }
        #endregion
        #region colider
        public void GetColiderWidthAndHight(BoxCollider2D targetColider, out int x, out int y)
        {
            Vector3 scale = targetColider.transform.localScale;
            x = Mathf.FloorToInt(Mathf.Abs(targetColider.size.x * scale.x / cellSize));
            y = Mathf.FloorToInt(Mathf.Abs(targetColider.size.y * scale.y / cellSize));
        }
        public Vector3 GetColiderOrgin(BoxCollider2D targetColider)
        {
            Vector3 extend = targetColider.bounds.extents;
            Vector3 center = targetColider.bounds.center;
            return -extend + center;
        }
        public void get2DColiderInfo(BoxCollider2D colider,out Vector3 orgine,out int width,out int heghit)
        {
            orgine = GetColiderOrgin(colider);
            GetColiderWidthAndHight(colider,out width,out heghit);
        }
        #endregion
        #region For Heat Map Work whit int
      /*  public void Addvalue(int x , int y ,int plusvalue)
        {
            int currentValue= GetValue(x, y);
            SetValue(x, y, currentValue + plusvalue);
        }
        public void Addvalue(Vector3 postion, int value, int fullyrang,int totalRange)
        {
            int lowerValueAmount = Mathf.RoundToInt((float)value / (totalRange - fullyrang));
            Debug.Log(lowerValueAmount);
            GetXY(postion, out int Xorgin, out int Yorgin);   
            for(int x=0; x< totalRange; x++)
            {
                for(int y = 0; y < totalRange - x; y++)
                {
                  int  radius = x + y;
                  int valueToAdd = value ;
                    if (radius > fullyrang)
                    {
                        valueToAdd -= lowerValueAmount * (radius - fullyrang);
                    }

                    Addvalue(Xorgin+x,Yorgin+y, valueToAdd);
                    if (x !=0)
                    {
                    Addvalue(Xorgin-x,Yorgin+y, valueToAdd);   
                    }
                    if(y != 0)
                    {
                    Addvalue(Xorgin+x,Yorgin-y, valueToAdd);  
                        if(x != 0)
                        {
                    Addvalue(Xorgin-x,Yorgin-y, valueToAdd);  

                        }
                    }

                }
            }

         
        }
      */
        #endregion

    }
}

