using Abed.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove_PathFinding : MonoBehaviour
{
    List<Vector3> path;
    Vector3 sizeVector;
    TestPathfinding_PathFinding TestP;
    [SerializeField] int speed = 1;
    int i;
    public void setStart(Vector3 startpos)
    {
        // transform.position = startpos;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            path = null;
        }
    }
    public void Go(List<Vector3> path, Vector3 sizeVector)
    {
        foreach (Vector3 pathnod in path)
        {
            print(pathnod);
        }
        this.path = null;
        this.path = path;
        i = 1;
        this.sizeVector = sizeVector;
        StartCoroutine(move());
    }
    IEnumerator move()
    {

        while (true)
        {
            if (path != null && path.Count > 1 && path.Count > i)
            {
                print("dir  = " + _Utils.GetDirction8(transform.position, path[i]));
                print("i=  " + i);
                var dir = _Utils.GetDirction8(transform.position, path[i]);
                transform.position += dir * speed * Time.deltaTime;
                float distance = Vector3.Distance(transform.position, path[i]);
                if (distance < 2) i++;
            }
            yield return new WaitForFixedUpdate();
        }

    }
    public void StopMove()
    {
        path = null;
    }
}

