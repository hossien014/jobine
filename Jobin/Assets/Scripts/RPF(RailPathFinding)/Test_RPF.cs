using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

public class Test_RPF : MonoBehaviour
{
    //   [SerializeField] Vector3 start;
    //[SerializeField] Vector3 end;
    [SerializeField] BoxCollider2D b2d;
    [SerializeField] GameObject obj2d;
    [SerializeField] GameObject charctor;
    [SerializeField] Transform OrgineTrans;
    [SerializeField] Transform anker;
    [SerializeField] Transform anker2;
    [SerializeField] GameObject T2;


    Rail_RPF Rail;
    List<Vector3> ways;
    Vector3[] vertices;
    Grid_Utils<bool> grid;

    void Awake()
    {



    }
    private void Start()
    {


        Vector3 orgine = new Vector3(5, 5);
        _Utils.DrawDebugSquer(orgine, 1);
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 5; y++)
            {

            }
        }
    }
    public Vector3 getworldpos(int x, int y, Vector3 org, int cellsize)
    {
        var worldpos = new Vector3(x, y) * cellsize + org;
        return Vector3.zero;
    }
    private void GetGridobj()
    {
        Vector3 s = new Vector3(0, 0);
        Vector3 charpos = new Vector3(27, 4);
        int size = 5;
        int x = Mathf.FloorToInt(charpos.x / 5);
        int y = Mathf.FloorToInt(charpos.y / 5);
        Vector3 t = new Vector3(x * size, y * size);
        print("x " + x + " y " + y + " re " + t);

    }


    void creatobj()
    {
        Vector3 orgin = new Vector3(18, 5);
        float size = 5;
        // I = X
        for (int i = 0; i < 8; i++)
        {
            //  GameObject obj = new GameObject("obj" + i);
            Vector3 objpos = new Vector3(i, 0);
            objpos = new Vector3(objpos.x, objpos.y) * size + orgin;
            //   obj.transform.position = objpos;
            Debug.DrawLine(objpos, new Vector3(objpos.x + size, objpos.y), Color.black, 1000);
            Debug.DrawLine(new Vector3(objpos.x + size, objpos.y), new Vector3(objpos.x + size, objpos.y + size), Color.black, 1000);
            Debug.DrawLine(new Vector3(objpos.x + size, objpos.y + size), new Vector3(objpos.x, objpos.y + size), Color.black, 1000);
            Debug.DrawLine(new Vector3(objpos.x, objpos.y + size), objpos, Color.black, 1000);
        }

    }
    private void OnDrawGizmos()
    {
        var realorg = (OrgineTrans.transform.position) - (OrgineTrans.localScale) / 2;
        _Utils.DrawOrgineLine(realorg);
        GameObject[] waypoints = GameObject.FindGameObjectsWithTag("waypoint");
        foreach (GameObject way in waypoints)
        {
            var pos = new Vector3(Mathf.RoundToInt(way.transform.position.x), Mathf.RoundToInt(way.transform.position.y));
            way.transform.position = pos;

        }
        var charpos = charctor.transform.position;
        var loacalcharpos = ((charpos - charctor.transform.localScale / 2) - (realorg));
        var normalizedlocl = new Vector3(Mathf.FloorToInt(loacalcharpos.x), Mathf.FloorToInt(loacalcharpos.y));
        print("LOCAL" + normalizedlocl);


    }

}
