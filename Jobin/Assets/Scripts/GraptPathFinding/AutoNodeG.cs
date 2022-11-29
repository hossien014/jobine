using Abed.Utils;
using System.Collections;
using UnityEngine;
[RequireComponent (typeof(Path))]
public class AutoNodeG : MonoBehaviour
{
    [SerializeField] int spaceBetwineNodes=3;
    [SerializeField] float updateSpped = 0.5f;
    [SerializeField] GameObject node;
    [SerializeField] int diCount;
    Transform nodeparent;

    void Update()
    {
        // BakeNode();
    }

    public void BakeNode(int spaceBetwineNodes,float updateSpped,GameObject node )
    {
            this.spaceBetwineNodes=spaceBetwineNodes;
            this.updateSpped =updateSpped;
            this.node=node;

            DeleteOldNode();
            CreateNodeParentObject();
            StartCoroutine(CreatNodeInPlatformC());

    }
    public void DeleteOldNode()
    {
        GameObject NodeGP = GameObject.Find("NodeGp");
        if(NodeGP==null) return;
        var child =NodeGP.transform.GetChild(1);
        foreach(Transform childObj in NodeGP.transform )
        {
           GameObject.DestroyImmediate(childObj.gameObject);
        }
        GameObject.DestroyImmediate(NodeGP); 
    }

    public void CreateNodeParentObject()
    {
        if (GameObject.Find("NodeGp") == null)
        {
            nodeparent = new GameObject("NodeGp").transform;
        }
        else { nodeparent = GameObject.Find("NodeGp").transform; }
    }
    IEnumerator CreatNodeInPlatformC()
    {
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("ground");
        for (int i = 0; i < grounds.Length; i++)
        {
            Vector3[] topline = _Utils.GetColiderTopLine(grounds[i]);
            int xlenght = Mathf.FloorToInt(grounds[i].transform.localScale.x);
            drawLineInTopOfPlatform(topline, xlenght, spaceBetwineNodes);
            yield return new WaitForSeconds(updateSpped);
        }
    }
    private void CreatNodeInPlatform()
    {
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("ground");
        for (int i = 0; i < grounds.Length; i++)
        {
            Vector3[] topline = _Utils.GetColiderTopLine(grounds[i]);
            int xlenght = Mathf.FloorToInt(grounds[i].transform.localScale.x);
            drawLineInTopOfPlatform(topline, xlenght, spaceBetwineNodes);
        }
    }
    private void drawLineInTopOfPlatform(Vector3[] topline, int xlenght,int space)
    {
        for (int b = 0; b < xlenght + 2; b += space)
        {
          var point =_Utils.LerpByDistance(topline[0], topline[1], b);
            Instantiate(node, point, default, nodeparent);
        }
    }
   
}
