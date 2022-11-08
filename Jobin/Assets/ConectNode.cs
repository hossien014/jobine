using Abed.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using UnityEngine;
public class ConectNode : MonoBehaviour
{
    [SerializeField] ScreenLog_Utils SLog;
    public Dictionary<Vector2, testnod> GetNodesDictionery()
    {
        Dictionary<Vector2, testnod> Nodes = new Dictionary<Vector2, testnod>();
        testnod[] AllTNodeInSceen = FindObjectsOfType<testnod>();
        if (AllTNodeInSceen.Length == 0) { Debug.Log("Ther is No Node in sceen"); return null; }
        else
        {
            foreach (testnod nod in AllTNodeInSceen)
            {
                if (Nodes.ContainsKey(nod.Key)) continue;
                Nodes.Add(nod.Key, nod);
                // print(nod.Key+" add");
            }
            return Nodes;
        }
    }
    public List<testnod> GetInRangeNodeList(Vector3 TargetNode, int range, bool ShowRadius)
    {
        Dictionary<Vector2, testnod> NodeDictionery = GetNodesDictionery();
        #region print text in SCeen
        GameObject DicOrgin = GameObject.Find("dicOrgin");
        if (GameObject.Find("dicOrgin") == null)
        {
          DicOrgin = new GameObject(("dicOrgin"), typeof(TextMesh));
        }
        DicOrgin.GetComponent<TextMesh>().text = "dic[0]" + NodeDictionery.ElementAt(0).Value.Key;
        DicOrgin.transform.position = NodeDictionery.ElementAt(0).Value.pos + Vector3.up * 2; 
        Debug.DrawLine(NodeDictionery.ElementAt(0).Value.pos, NodeDictionery.ElementAt(0).Value.pos +Vector3.up * 3, Color.magenta);
        #endregion
        List<testnod> NearNodesList = new List<testnod>();
        for (int x = Mathf.RoundToInt(TargetNode.x - range); x <= TargetNode.x + range; x++)
        {
            for (int y = Mathf.RoundToInt(TargetNode.y - range); y <= TargetNode.y + range; y++)
            {
                //Gizmos.DrawSphere(new Vector2(x, y), 0.1f); // debug
                if (NodeDictionery.ContainsKey(new Vector2(x, y)))
                {
                    NodeDictionery.TryGetValue(new Vector2(x, y), out testnod nearNode);
                    if (nearNode.pos == TargetNode) continue;
                    NearNodesList.Add(nearNode);
                }
            }
        }
        //if (NearNodesList.Count == 0) Debug.Log("There is No  Nod In Range OF " + TargetNode);
        //  else { Debug.Log("list of In Range Nodes  was send " + NearNodesList.Count); }
        if (ShowRadius) ShowSearchBox(TargetNode, range);
        if (ShowRadius) DarawLineInList(NearNodesList);
        return NearNodesList;
    }
    public testnod GetNearestNode(Vector3 CenterNod, List<testnod> nearList)
    {
        if (nearList.Count == 0) return null;
        testnod Nearestnod = nearList[0];
        Debug.DrawLine(nearList[0].pos, nearList[0].pos + Vector3.up * 3, Color.green);
        foreach (testnod Bnod in nearList)
        {
            float NearaNodDis = Vector3.Distance(CenterNod, Nearestnod.pos);
            float BNodeDis = Vector3.Distance(CenterNod, Bnod.pos);
            if (BNodeDis < NearaNodDis)
            {
                Nearestnod = Bnod;
            }
        }
        return Nearestnod;
    }
    #region Debug 
    private void DarawLineInList(List<testnod> NodeList)
    {
        foreach (testnod Nod in NodeList)
        {
            Debug.DrawLine(Nod.pos, Nod.pos + Vector3.up ,Color.black);
        }
    }
    void ShowSearchBox(Vector3 pos, int radius)
    {
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y - radius)), new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y - radius)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y + radius)), new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y + radius)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y + radius)), new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y - radius)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y + radius)), new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y - radius)));
    }
    void ConectNodesVitsialy(List<testnod> ConectedNod)
    {
        if (ConectedNod.Count > 2)
            for (int i = 0; i < ConectedNod.Count - 1; i++)
            {
                //if(i+1 < nodes.Count)
                Debug.DrawLine(ConectedNod[i].pos, ConectedNod[i + 1].pos, Color.green, 0.1f);
            }
    }
    #endregion
}
