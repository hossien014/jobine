using UnityEngine;

namespace Abed.RPF
{
    public class RailPoint_RPF
    {
        bool DebugView = true;
        Vector3 Pos;
        int Cellsize;
        RailPoint_RPF ExploredForm;
        int id = 0;
        public RailPoint_RPF(Vector3 Pos, int Cellsize, int id)
        {
            this.Pos = Pos;
            this.Cellsize = Cellsize;
            this.id = id;
            if (DebugView)
            {
                GameObject parent = GameObject.Find("railPointParent");
                if (parent == null)
                {
                    parent = new GameObject("railPointParent");
                }
                GameObject deboug = GameObject.CreatePrimitive(PrimitiveType.Plane);
                deboug.name = (Pos.x) + "/" + (Pos.y).ToString();
                deboug.transform.parent = parent.transform;
                deboug.transform.localPosition = Pos;//+(new Vector3(Cellsize, Cellsize) /2);
                deboug.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                deboug.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                // deboug.transform.GetComponent
            }
        }
    }
}
