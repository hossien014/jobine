
using Abed.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Path : MonoBehaviour
{
    [SerializeField] bool itsEnemy;
    [SerializeField] Transform Destination;
    [SerializeField] List<testnod> pathtoPalyer;
    [SerializeField] int radius = 10;
    [SerializeField] ScreenLog_Utils Slog;

    testnod ss;
    testnod ee;

    void LateUpdate()
    {
        if (itsEnemy)
        {
                GetStartAndEnd(out testnod start, out testnod end);
                 GameObject StartAndEndLog = GameObject.Find("StartAndEndLog");
            if (StartAndEndLog == null)
                {
                    StartAndEndLog = new GameObject("StartAndEndLog", typeof(TextMesh));
                }
            if (start == null || end == null)
            {
                    StartAndEndLog.GetComponent<TextMesh>().text = "strat or end not found";
                    StartAndEndLog.transform.position =transform.position+Vector3.up*13;
            }
            else
            {
                StartAndEndLog.GetComponent<TextMesh>().text = "*";
                Debug.DrawLine(start.pos + Vector3.up, end.pos + Vector3.up, Color.cyan);
                pathtoPalyer = GetPath(start, end);

                for (int i = 0; i < pathtoPalyer.Count; i++)
                {
                    if (i + 1 > pathtoPalyer.Count - 1) break;
                    Debug.DrawLine(pathtoPalyer[i].pos, pathtoPalyer[i + 1].pos, Color.blue, 0.1f);
                }
            }
            _Utils.reloadScene();
        }
    }
    List<testnod> GetPath(testnod start, testnod end)
    {
        List<testnod> path = new List<testnod>();

        var noddic = FindObjectOfType<ConectNode>().GetNodesDictionery();
        foreach (var nod in noddic)
        {
            nod.Value.exploredFrome = null;
        }
        start.exploredFrome = null;

        List<testnod> openlist = new List<testnod>();
        openlist.Add(start);

        int i = 0;
        while (openlist.Count > 0)
        {
            i++;
            var currentNode = openlist[0];
            openlist.Remove(currentNode);

          //  print(openlist.Count + "  open list count ");
           // print(currentNode.ConectedList.Count + "  ConectedList.Count");

            foreach (testnod nod in currentNode.ConectedList)
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
    private List<testnod> CuaculatePath(testnod start, testnod end)
    {
      //  print("start to calcluate path ");
        List<testnod> Path = new List<testnod>();
        var currentnod = end;

        while (currentnod != start)
        {
            Path.Add(currentnod);
            currentnod = currentnod.exploredFrome;
        }
        Path.Add(start);
        Path.Reverse();
        pathtoPalyer = Path;
        return Path;
    }
    testnod GetNode(Transform Target)
    {
        var nodedic = FindObjectOfType<ConectNode>().GetNodesDictionery();
        var key = new Vector2(Mathf.FloorToInt(Target.transform.position.x), Mathf.FloorToInt(Target.transform.position.y));
        List<testnod> nearlist = FindObjectOfType<ConectNode>().GetInRangeNodeList(key, radius, true);

        //  nodedic.TryGetValue(key, out testnod PNode);
        if (nearlist.Count > 0)
        {
            var nearestNode = FindObjectOfType<ConectNode>().GetNearestNode(key, nearlist);
            return nearestNode;
        }
        else return null;
    }
    private void GetStartAndEnd(out testnod Start, out testnod End)
    {
        Start = GetNode(transform);
        End = GetNode(Destination);
        //  Debug.DrawLine(Start.pos, End.pos, Color.magenta, 100);
    }
    private void OnDrawGizmos()
    {

        //Gizmos.color = Color.green;
        var ThisObjNode = GetNode(transform);
        //print(ThisObjNode.exploredFrome);
       // Gizmos.DrawWireSphere(ss.pos, 0.5f);
        // Gizmos.DrawWireSphere(ee.pos,0.5f);
        //if (itsEnemy)
        //{
        // var DestinationNode = GetNode(Destination);
        // Gizmos.DrawSphere(DestinationNode.pos, 0.7f);
        //}

    }

}
