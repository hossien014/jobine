using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

// namespace Abed.GraphPathFinding
// {
    public enum NodeType { Normal, Jump, fall }
    [ExecuteInEditMode]
    [SelectionBase]
    public class Path : MonoBehaviour
    {
        [SerializeField] bool itsEnemy;
        [SerializeField] bool DebugView;
        [SerializeField] Transform Destination;
        public List<NodeG> ThePath;
        [SerializeField] int radius = 10;
        Vector3 lastTargetPos;


        private void Awake()
        {
            ThePath = null;
            FindingPathProcees();
        }
        void LateUpdate()
        {
            // if (lastTargetPos != Destination.position) { FindingPathProcees(); }
            FindingPathProcees();
            _Utils.reloadScene();
        }
        private void FindingPathProcees()
        {
            lastTargetPos = Destination.position;
            if (!itsEnemy) return;
            GetStartAndEnd(out NodeG start, out NodeG end);
            DEbug_Behaviour(start, end);
            if (start == null || end == null) ThePath = null;
            else
            {
                ThePath = GetPath(start, end);
                CalculateNodeType(ThePath);
            }
        }
        private void GetStartAndEnd(out NodeG Start, out NodeG End)
        {
            Start = GetNode(transform);
            End = GetNode(Destination);
            if (DebugView) Debug.DrawLine(Start.pos, End.pos, Color.magenta, 0.3f);
        }
        NodeG GetNode(Transform Target)
        {
            //Vector2 key = new Vector2(Mathf.FloorToInt(Target.transform.position.x), Mathf.FloorToInt(Target.transform.position.y));
            Vector2 key = Target.transform.position.FloorVector();
            List<NodeG> nearlist = GetInRangeNodeList(key, radius, true);

            //  nodedic.TryGetValue(key, out testnod PNode);
            if (nearlist.Count > 0)
            {
                var nearestNode = GetNearestNode(key, nearlist);
                return nearestNode;
            }
            else return null;
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
        public Dictionary<Vector2, NodeG> GetNodesDictionery()
        {
            Dictionary<Vector2, NodeG> Nodes = new Dictionary<Vector2, NodeG>();
            NodeG[] AllTNodeInSceen = FindObjectsOfType<NodeG>();
            if (AllTNodeInSceen.Length == 0) { Debug.Log("*Ther is No Node in sceen*"); return null; }
            else
            {
                foreach (NodeG nod in AllTNodeInSceen)
                {
                    if (Nodes.ContainsKey(nod.Key)) continue;
                    Nodes.Add(nod.Key, nod);
                }
                return Nodes;
            }
        }
        public List<NodeG> GetInRangeNodeList(Vector3 TargetNode, int range, bool DebugView)
        {
            var NodeDictionery = GetNodesDictionery();
            var NearNodesList = new List<NodeG>();
            for (int x = Mathf.RoundToInt(TargetNode.x - range); x <= TargetNode.x + range; x++)
            {
                for (int y = Mathf.RoundToInt(TargetNode.y - range); y <= TargetNode.y + range; y++)
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
            if (DebugView) ShowSearchBox(TargetNode, range);
            if (DebugView) DarawLineInList(NearNodesList);
            return NearNodesList;
        }
        public NodeG GetNearestNode(Vector3 CenterNod, List<NodeG> nearList)
        {
            if (nearList.Count == 0) return null;
            NodeG Nearestnod = nearList[0];
            Debug.DrawLine(nearList[0].pos, nearList[0].pos + Vector3.up * 3, Color.green);
            foreach (NodeG Bnod in nearList)
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
        void ShowSearchBox(Vector3 pos, int radius)
        {
            Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y - radius)), new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y - radius)));
            Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y + radius)), new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y + radius)));
            Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y + radius)), new Vector3(Mathf.RoundToInt(pos.x - radius), Mathf.RoundToInt(pos.y - radius)));
            Debug.DrawLine(new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y + radius)), new Vector3(Mathf.RoundToInt(pos.x + radius), Mathf.RoundToInt(pos.y - radius)));
        }
        #endregion

    }
//}
