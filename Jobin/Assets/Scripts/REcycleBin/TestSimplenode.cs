//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//public class TestSimplenode : MonoBehaviour
//{
//    Dictionary<Vector2, NodeG> NodesDic;
//    //[SerializeField] GameObject radiusObj;

//    public List<NodeG> GetInRangeNode(Dictionary<Vector2, NodeG> NodeDictionery, GameObject radiusObj)
//    {
//        List<NodeG> NearNodesList = new List<NodeG>();
//        int size = Mathf.RoundToInt(radiusObj.transform.lossyScale.x);
//        var orgin = radiusObj.transform.position - new Vector3(size / 2, size / 2);
//        int radiusX = Mathf.RoundToInt(orgin.x + size);
//        int radiusY = Mathf.RoundToInt(orgin.y + size);

//        for (int x = Mathf.RoundToInt(orgin.x); x < radiusX; x++)
//        {
//            for (int y = Mathf.RoundToInt(orgin.y); y < radiusY; y++)
//            {
//                // Gizmos.DrawSphere(new Vector2(x, y), 0.1f); // debug
//                if (NodeDictionery.ContainsKey(new Vector2(x, y)))
//                {
//                    NodeDictionery.TryGetValue(new Vector2(x, y), out NodeG nod);
//                    NearNodesList.Add(nod);
//                    // print(nod.Key+"N");
//                    nod.transform.GetComponent<SpriteRenderer>().color = Color.yellow; //debug
//                }
//            }
//        }
//        if (NearNodesList.Count == 0) Debug.Log("There is No  Nod In Range");
//        else { Debug.Log("list of In Range Nodes  was send " + NearNodesList.Count); }
//        return NearNodesList;
//    }
//    public NodeG GetNearestInRangNode(List<NodeG> InRangeNodes, Vector3 pos)
//    {
//        NodeG nearestNode;
//        if (InRangeNodes.Count > 0)
//        {
//            nearestNode = InRangeNodes[0];

//            foreach (NodeG nod in InRangeNodes)
//            {
//                float distance_off_nearest = Vector3.Distance(pos, nearestNode.pos);
//                float distance = Vector3.Distance(pos, nod.pos);
//                if (distance < distance_off_nearest)
//                {
//                    nearestNode = nod;
//                }
//            }
//            return nearestNode;
//        }
//        return null;
//    }
//    public NodeG GetNearestInRangNode(Vector3 pos, GameObject radiusObj)
//    {
//        NodesDic = FindObjectOfType<nodManegerTest>().GetNodesDic(); Debug.Log("nodes Dictonery recived" + NodesDic.Count);
//        if (NodesDic.Count > 0)
//        {
//            List<NodeG> InRangeNodes = GetInRangeNode(NodesDic, radiusObj);
//            if (InRangeNodes.Count > 0)
//            {
//                NodeG NearestNode = GetNearestInRangNode(InRangeNodes, pos);
//                Debug.Log("Nearest Node is = " + NearestNode);
//                return NearestNode;
//            }

//        }
//        Debug.Log("nearest Nod Not Found");
//        return null;
//    }
//    public NodeG GetNearestNode(NodeG ANode)
//    {
//        if (NodesDic.Count < 0)
//        {
//            NodesDic = FindObjectOfType<nodManegerTest>().GetNodesDic();
//            if (NodesDic.Count < 0) { print("there is no Node in sceen"); return null; }
//        }

//        NodeG Nearestnod = NodesDic.ElementAt(0).Value;
//        foreach (KeyValuePair<Vector2, NodeG> nod in NodesDic)
//        {
//            NodeG BNode = nod.Value;
//            float NearaNodDis = Vector3.Distance(ANode.pos, Nearestnod.pos);
//            float BNodeDis = Vector3.Distance(ANode.pos, BNode.pos);
//            if (BNodeDis < NearaNodDis)
//            {
//                Nearestnod = BNode;
//            }
//        }
//        return Nearestnod;
//    }
//    private void OnDrawGizmos()
//    {
//        /* Nodes = FindObjectOfType<nodManegerTest>().GetNodes();
//         if (Nodes.Count > 0)
//         {
//             List<testnod> listofnod = GetInRangeNode(Nodes);
//             if (listofnod.Count > 0)
//             {
//                 testnod nod = GetNearestNode(listofnod,transform.position);
//                 Gizmos.color = Color.blue;
//                 Gizmos.DrawSphere(nod.pos, 0.5f);
//             }

//        } */
//    }
//}