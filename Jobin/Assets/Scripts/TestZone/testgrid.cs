
////using System.Collections;
////using UnityEngine;
////using Abed.Utils;
////public class testgrid : MonoBehaviour
////{
////    GridA grid;
////    Vector3 orgin;
////    int cellesize=1;
////    [SerializeField] BoxCollider2D target;
////    BoxCollider2D[] walkble; 

////    void Start()
////    {
////       findAndColectGrounds();
////    }
////    private void findAndColectGrounds()
////    {
////        int groundCount = GameObject.FindGameObjectsWithTag("ground").Length;
////        walkble = new BoxCollider2D[groundCount];
////        for (int i = 0; i < groundCount; i++)
////        {
////            walkble[i] = GameObject.FindGameObjectsWithTag("ground")[i].transform.GetComponent<BoxCollider2D>();
////            if (walkble[i] == null) continue;
////            Vector3 orgin= GetColiderOrgin(walkble[i]);
////            int x; int y;
////            calculateXY(walkble[i],out x,out y);
////            new GridA(x, y, cellesize, this.orgin);
////        }
////    }


////    private void calculateXY(BoxCollider2D targetColider,out int x, out int y)
////    {
////        Vector3 scale= targetColider.transform.localScale;
////        x = Mathf.FloorToInt(Mathf.Abs(targetColider.size.x*scale.x / cellesize));
////        y = Mathf.FloorToInt(Mathf.Abs(targetColider.size.y * scale.y / cellesize));
////    }

////    Vector3 GetColiderOrgin(BoxCollider2D targetColider)
////    {
////        Vector3 extend = targetColider.bounds.extents;
////        Vector3 center = targetColider.bounds.center;
////       return orgin = -extend + center;
////    }
////    private void SetNumbertoCell()
////    {
////        if (Input.GetMouseButtonDown(0))
////        {
////            grid.SetValue(Utilis.GetMousePos(), 1);
////        }
////    }
   
////    private void OnDrawGizmos()
////    {
////        Gizmos.color = Color.red;
////        int groundCount = GameObject.FindGameObjectsWithTag("ground").Length;
////        walkble = new BoxCollider2D[groundCount];
////        for (int i = 0; i < groundCount; i++)
////        {
////            walkble[i] = GameObject.FindGameObjectsWithTag("ground")[i].transform.GetComponent<BoxCollider2D>();
////            if (walkble[i] == null) continue;
////            Vector3 extend = walkble[i].bounds.extents;
////            Vector3 center = walkble[i].bounds.center;
////            Gizmos.DrawWireSphere(-extend + center, 1);
////            Gizmos.DrawWireSphere((extend + center), 1);
////        }
////    }


////    IEnumerator randomnumber()
////    {
////        while (true)
////        {
////            grid.SetValue(Random.Range(0, 5), Random.Range(0, 5), Random.Range(0, 100));
////            yield return new WaitForSeconds(0.1f);
////        }
////    }
////}
