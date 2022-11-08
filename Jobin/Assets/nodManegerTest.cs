using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class nodManegerTest : MonoBehaviour
{
    Dictionary<Vector2, testnod> Nodes;
    List<testnod> openlist;
    List<testnod> closelist;
    List<testnod> ThePath;

    [SerializeField] GameObject thisRadius;
    [SerializeField] GameObject taget;
    [SerializeField] GameObject tagetRadius;
    private void Start()
    {
        //Pathfinding(taget);
    }

    void Pathfinding(GameObject target)
    {
        testnod startnode, endnode;
        FindStartAndEndNode(target, out startnode, out endnode);

        openlist = new List<testnod>();
        closelist = new List<testnod>();
        openlist.Add(startnode); Debug.Log(startnode + "ADD IN Open liste _ open list Count = " + openlist.Count);
        startnode.exploredFrome = null;
        while (openlist.Count > 0)
        {
            if (openlist.Count == 0) { print(" open list is empty"); break; }
            testnod searchCenter = openlist[0];
            SearchNearNode(searchCenter, endnode);
        }

    }

    private void FindStartAndEndNode(GameObject target, out testnod startnode, out testnod endnode)
    {
        TestSimplenode simplNode = GetComponent<TestSimplenode>();
        startnode = simplNode.GetNearestInRangNode(transform.position, thisRadius);
        endnode = simplNode.GetNearestInRangNode(target.transform.position, tagetRadius);
        print("S= " + startnode.Key + "E= " + endnode.Key);
    }

    public void SearchNearNode(testnod SearchCneter, testnod endnode)
    {
        openlist.Remove(SearchCneter);
        closelist.Add(SearchCneter);
        var nearestNod = FindObjectOfType<TestSimplenode>().GetNearestNode(SearchCneter);
        nearestNod.exploredFrome = SearchCneter;
        if (nearestNod == endnode) { CalculatePath(endnode); }
        else
        {
            openlist.Add(nearestNod);
        }

    }
    private void searchNeighbors(testnod SearchCneter, testnod endnode)
    {
        print("nieghberSeeach invoke");
        Vector3[] dirctions = _Utils.DirctionArray4();
        if (!closelist.Contains(SearchCneter))
        {
            closelist.Add(SearchCneter);
            openlist.Remove(SearchCneter);
            Debug.Log(SearchCneter.Key + "Add in close List and remove form open_ open list count =" + openlist.Count);
            // Debug.Log(dirctions.Length);
            for (int i = 0; i < dirctions.Length; i++)
            {
                var neaber = SearchCneter.pos + dirctions[i];
                if (Nodes.ContainsKey(neaber))
                {
                    Nodes.TryGetValue(neaber, out testnod neberNode);
                    Debug.Log(neberNode + " found In neighbors");
                    if (openlist.Contains(neberNode)) continue;
                    neberNode.exploredFrome = SearchCneter;

                    if (neberNode == endnode) { Debug.Log(" end nod find" + endnode); CalculatePath(endnode); break; }
                    else { openlist.Add(neberNode); Debug.Log(neberNode + " Add to Open list_ open list count = " + openlist.Count); }
                }
            }
        }
    }

    private List<testnod> CalculatePath(testnod endNode)
    {
        List<testnod> Path = new List<testnod>();
        testnod currentNode = endNode;
        while (currentNode == null)
        {
            Path.Add(currentNode);
            currentNode = currentNode.exploredFrome;
        }
        Path.Reverse();
        ThePath = Path;
        return Path;
    }

    public Dictionary<Vector2, testnod> GetNodesDic()
    {
        Nodes = new Dictionary<Vector2, testnod>();
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
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //  var near = GetComponent<TestSimplenode>().GetNearestNode(transform.position);
        //Vector3 upneber = near.pos + _Utils.DirctionArray()[2];
        // Gizmos.DrawSphere(upneber, 0.3f);

    }
}
