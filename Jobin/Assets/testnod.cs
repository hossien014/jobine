using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class testnod : MonoBehaviour
{
    public Vector3 pos;
    public Vector2 Key;
    public testnod exploredFrome;
    public List<testnod> ConectedList;

    private void Start()
    {
        FindConected();
        var postion = transform.position;
        int x = Mathf.FloorToInt(postion.x);
        int y = Mathf.FloorToInt(postion.y);
        transform.position = new Vector3(Mathf.FloorToInt(postion.x), Mathf.FloorToInt(postion.y));
        transform.name = (x + "/" + y);
        Key = new Vector2(x, y);
        pos = new Vector2(x, y);
    }
    private void LateUpdate()
    {
        var postion = transform.position;
        int x = Mathf.FloorToInt(postion.x);
        int y = Mathf.FloorToInt(postion.y);
        transform.position = new Vector3(Mathf.FloorToInt(postion.x), Mathf.FloorToInt(postion.y));
        transform.name = (x + "/" + y);
        Key = new Vector2(x, y);
        pos = new Vector2(x, y);

        FindConected();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (testnod Cnod in ConectedList)
        {

            // Gizmos.DrawLine(this.pos, Cnod.pos);
        }
    }
    void FindConected()
    {
        ConectNode Conectnod = FindObjectOfType<ConectNode>();
        ConectedList = Conectnod.GetInRangeNodeList(this.pos, 5, false);
    }
}
