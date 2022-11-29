using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

// namespace Abed.GraphPathFinding
// {
    public enum NodeType { Normal, Jump, fall,blocked }
    public enum searchCenter{orgine,center}
    [ExecuteInEditMode]
    public class Path : MonoBehaviour
    {
        [SerializeField] Transform Destination; //target
        [SerializeField] bool itsEnemy; //delete
        [SerializeField] bool DebugView;
        public List<NodeG> ThePath;
        [SerializeField] int radius = 10;
        NodeG lastEndNode;


        private void Awake()
        {
            ThePath = null;
         //   FindingPathProcees();
        }
        void LateUpdate()
        {
            
           // FindingPathProcees();
            _Utils.reloadScene();
        }
        public void FindingPathProcees(Transform thisObj,Transform targetObj)
        {
            if (!itsEnemy) return;
            GetStartAndEnd(thisObj,targetObj,out NodeG start, out NodeG end);
            if(end != null) lastEndNode = end;
            Debug.DrawLine(lastEndNode.pos, lastEndNode.pos + Vector3.up * 5,Color.red);
            DEbug_Behaviour(start, lastEndNode);
            if (start == null || lastEndNode == null) ThePath = null;
            else
            {
                ThePath = GetPath(start, lastEndNode);;
                CalculateNodeType(ThePath);
            }
        }
        private void GetStartAndEnd(Transform ThisObj,Transform targtObj,out NodeG Start, out NodeG End)
        {
            Start = GetNode(ThisObj,searchCenter.orgine);
            End = GetNode(targtObj,searchCenter.orgine);
            if (DebugView) Debug.DrawLine(Start.pos, End.pos, Color.magenta, 0.3f);
        }
        private List<NodeG> GetPath(NodeG start, NodeG end)
        {
            List<NodeG> path = new List<NodeG>();

            var noddic = GetNodesDictionery();
            foreach (var nod in noddic)
            {
                nod.Value.exploredFrome = null;
            }
            start.exploredFrome = null;

            List<NodeG> openlist = new List<NodeG>();
            openlist.Add(start);

            int i = 0;
            while (openlist.Count > 0)
            {
                i++;
                var currentNode = openlist[0];
                openlist.Remove(currentNode);

                //  print(openlist.Count + "  open list count ");
                // print(currentNode.ConectedList.Count + "  ConectedList.Count");

                foreach (NodeG nod in currentNode.ConectedList)
                {


                    if (nod.exploredFrome == null && nod != start)
                    {
                        nod.exploredFrome = currentNode;
                        // print(currentNode + "set nod explor");
                        openlist.Add(nod);
                        if (nod == end) { nod.exploredFrome = currentNode; path = CuaculatePath(start, end); openlist.Clear(); break; }
                    }
                }
            }
            return path;
        }
        private List<NodeG> CuaculatePath(NodeG start, NodeG end)
        {
            //  print("start to calcluate path ");
            List<NodeG> Path = new List<NodeG>();
            var currentnod = end;

            while (currentnod != start)
            {
                Path.Add(currentnod);
                currentnod = currentnod.exploredFrome;
            }
            Path.Add(start);
            Path.Reverse();
            ThePath = Path;
            return Path;
        }
    #region Node Tools 
        public Dictionary<Vector2, NodeG> GetNodesDictionery()
        {
            Dictionary<Vector2, NodeG> Nodes = new Dictionary<Vector2, NodeG>();
            NodeG[] AllTNodeInSceen = FindObjectsOfType<NodeG>();
            if (AllTNodeInSceen.Length == 0) { Debug.Log("*Ther is No Node in sceen*"); return null; }
            else
            {
                foreach (NodeG nod in AllTNodeInSceen)
                {
                    if (Nodes.ContainsKey(nod.Key))
                    {
                        Debug.Log("Duplicate Node found "+ nod.Key.ToString());
                       // DuplicateNode.Add(nod);
                        continue; 
                    } 
                    Nodes.Add(nod.Key, nod);
                }
                return Nodes;
            }
        }
    NodeG GetNode(Transform Target,searchCenter searchcenter)
        {
            //Vector2 key = new Vector2(Mathf.FloorToInt(Target.transform.position.x), Mathf.FloorToInt(Target.transform.position.y));
            Vector2 key = Target.transform.position.FloorVector();
            List<NodeG> nearlist = GetInRangeNodeList(key, radius,searchcenter,true);

            //  nodedic.TryGetValue(key, out testnod PNode);
            if (nearlist.Count > 0)
            {
                var nearestNode = GetNearestNode(key, nearlist);
                return nearestNode;
            }
            else return null;
        }
        #region Get nearest Liste
    // noraml get in range 
    public List<NodeG> GetInRangeNodeList(Vector3 TargetNode, int range,searchCenter searchcenter, bool DebugView)
    {
        var NodeDictionery = GetNodesDictionery(); // just do do it at start 
        var NearNodesList = new List<NodeG>();

          Getstartsearch(range, searchcenter,out int startPoint,out int endPoint);

        for (int x = Mathf.RoundToInt(TargetNode.x - (range - startPoint)); x <= TargetNode.x + range+endPoint; x++)
        {
            for (int y = Mathf.RoundToInt(TargetNode.y - (range - startPoint)); y <= TargetNode.y + range+endPoint; y++)
            {
                //Gizmos.DrawSphere(new Vector2(x, y), 0.1f); // debug
                if (NodeDictionery.ContainsKey(new Vector2(x, y)))
                {
                    NodeDictionery.TryGetValue(new Vector2(x, y), out NodeG nearNode);
                    if (nearNode.pos == TargetNode) continue;
                    NearNodesList.Add(nearNode);
                }
            }
        }
        
        if (DebugView) ShowSearchBox(TargetNode, range, searchcenter);
        if (DebugView) DarawLineInList(NearNodesList);
        return NearNodesList;
    }
   // x and y get in range 
    public List<NodeG> GetInRangeNodeList(Vector3 TargetNode, int xrange, int yrange,  bool DebugView)
    {
        var NodeDictionery = GetNodesDictionery(); // just do do it at start 
        var NearNodesList = new List<NodeG>();


        for (int x = Mathf.RoundToInt(TargetNode.x -xrange); x <= TargetNode.x + xrange; x++)
        {
            for (int y = Mathf.RoundToInt(TargetNode.y -yrange); y <= TargetNode.y + yrange; y++)
            {
                //Gizmos.DrawSphere(new Vector2(x, y), 0.1f); // debug
                if (NodeDictionery.ContainsKey(new Vector2(x, y)))
                {
                    NodeDictionery.TryGetValue(new Vector2(x, y), out NodeG nearNode);
                    if (nearNode.pos == TargetNode) continue;
                    NearNodesList.Add(nearNode);
                }
            }
        }

        if (DebugView) ShowSearchBox(TargetNode, xrange,yrange);
        if (DebugView) DarawLineInList(NearNodesList);
        return NearNodesList;
    }
    #endregion
        public NodeG GetNearestNode(Vector3 pos, List<NodeG> nearList)
        {
            if (nearList.Count == 0) return null;
            NodeG Nearestnod = nearList[0];
            foreach (NodeG Bnod in nearList)
            {
                float NearaNodDis = Vector3.Distance(pos, Nearestnod.pos);
                float BNodeDis = Vector3.Distance(pos, Bnod.pos);
                if (BNodeDis < NearaNodDis)
                {
                    Nearestnod = Bnod;
                }
            }
            Debug.DrawLine(Nearestnod.pos, Nearestnod.pos + Vector3.up * 3, Color.green);
            return Nearestnod;
        }
        public void CalculateNodeType(List<NodeG> Path)
        {

            for (int i = 0; i < Path.Count - 1; i++)
            {
                CalculateNodDircion(Path, i);
                if (Path[i].pos.y < Path[i + 1].pos.y) // jump node
                {
                    Path[i].Type = NodeType.Jump;
                }
                else if (Path[i].pos.y > Path[i + 1].pos.y) // fall node
                {
                    Path[i].Type = NodeType.fall;
                }
                else if (Path[i].pos.y == Path[i + 1].pos.y) // normal
                {
                    Path[i].Type = NodeType.Normal;
                }
            }
        }
        private static void CalculateNodDircion(List<NodeG> Path, int i)
        {
            var distance = (Path[i + 1].pos - Path[i].pos); var XDistance = distance.x;
            if (XDistance > 0) Path[i].Dir = Vector3.right;
            if (XDistance < 0) Path[i].Dir = Vector3.left;
        }
        private void Getstartsearch(int range, searchCenter searchcenter,out int startPoint, out int endPoint)
    {
        if (searchcenter == searchCenter.center){ startPoint = range; endPoint=range / 2;}
        else {startPoint = 0; endPoint=range=0;}
    }
    #endregion
    #region Debug 
        public void DEbug_Behaviour(NodeG start, NodeG end)
        {
            if (DebugView) return;
            GameObject StartAndEndLog = GameObject.Find("StartAndEndLog");
            if (StartAndEndLog == null)
            {
                StartAndEndLog = new GameObject("StartAndEndLog", typeof(TextMesh));
            }
            if (start == null || end == null)
            {
                StartAndEndLog.GetComponent<TextMesh>().text = "strat or end not found";
                StartAndEndLog.transform.position = transform.position + Vector3.up * 13;
            }
            else
            {
                Debug.DrawLine(start.pos + Vector3.up, end.pos + Vector3.up, Color.cyan * 0.1f);
                StartAndEndLog.GetComponent<TextMesh>().text = "*";
                DrawPathLine();
            }

        }
        private void DrawPathLine()
        {
            if (ThePath == null) return;
            for (int i = 0; i < ThePath.Count - 1; i++)
            {
                Debug.DrawLine(ThePath[i].pos, ThePath[i + 1].pos, Color.blue, 0.1f);
            }
        }
        private void DarawLineInList(List<NodeG> NodeList)
        {
            foreach (NodeG Nod in NodeList)
            {
                Debug.DrawLine(Nod.pos, Nod.pos + Vector3.up, Color.black);
            }
        }
    void ShowSearchBox(Vector3 pos, int radius,searchCenter searchcenter) //simple range 
    {
        Getstartsearch(radius,searchcenter,out int starsearch, out int endPoint);
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - (radius-starsearch) ), Mathf.RoundToInt(pos.y - (radius - starsearch))), new Vector3(Mathf.RoundToInt(pos.x + radius+endPoint), Mathf.RoundToInt(pos.y - (radius - starsearch)))); 
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - (radius - starsearch)), Mathf.RoundToInt(pos.y + radius+endPoint)), new Vector3(Mathf.RoundToInt(pos.x + radius+endPoint), Mathf.RoundToInt(pos.y + radius+endPoint)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - (radius - starsearch)), Mathf.RoundToInt(pos.y + radius+endPoint)), new Vector3(Mathf.RoundToInt(pos.x - (radius - starsearch)), Mathf.RoundToInt(pos.y - (radius - starsearch))));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x + radius+endPoint), Mathf.RoundToInt(pos.y + radius+endPoint)), new Vector3(Mathf.RoundToInt(pos.x + radius+endPoint), Mathf.RoundToInt(pos.y - (radius - starsearch))));
    }
    void ShowSearchBox(Vector3 pos, int radiusx,int radiusy) // x and y range 
    {
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radiusx), Mathf.RoundToInt(pos.y - radiusy)), new Vector3(Mathf.RoundToInt(pos.x +radiusx), Mathf.RoundToInt(pos.y - radiusy)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radiusx), Mathf.RoundToInt(pos.y + radiusy)), new Vector3(Mathf.RoundToInt(pos.x + radiusx), Mathf.RoundToInt(pos.y + radiusy)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radiusx), Mathf.RoundToInt(pos.y + radiusy)), new Vector3(Mathf.RoundToInt(pos.x -radiusx), Mathf.RoundToInt(pos.y - radiusy)));
        Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x + radiusx), Mathf.RoundToInt(pos.y + radiusy)), new Vector3(Mathf.RoundToInt(pos.x + radiusx), Mathf.RoundToInt(pos.y - radiusy)));
    }
        #endregion

    }
//}
