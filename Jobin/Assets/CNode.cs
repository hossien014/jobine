using Abed.Utils;
using UnityEngine;
[ExecuteInEditMode]
public class CNode : MonoBehaviour
{
    public Vector3 pos;
    public Vector2 Key;
    public testnod exploredFrome;
    public bool singelNode;
    void Update()
    {
        var postion = transform.position;
        int x = Mathf.FloorToInt(postion.x);
        int y = Mathf.FloorToInt(postion.y);
        transform.position = new Vector3(Mathf.FloorToInt(postion.x), Mathf.FloorToInt(postion.y));
        transform.name = (x + "/" + y);
        Key = new Vector2(x, y);
        pos = new Vector2(x, y);
    }
    void FindConections()
    {
        var dirctions = _Utils.DirctionArray8();
        foreach (var dir in dirctions)
        {
            var neighbor = pos + dir;

        }
    }
}
