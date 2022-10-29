using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testpolygon : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices=new Vector3[4];
        Vector2[] uv=new Vector2[4];
        int[] triangles= new int[6];

        vertices[0] = new Vector3(   0,0);
        vertices[1] = new Vector3(0, 100);
        vertices[2] = new Vector3(100, 100); 
        vertices[3] = new Vector3(100, 0);

        uv[0] = new Vector3(0, 0);
        uv[1] = new Vector3(0, 1);
        uv[2] = new Vector3(1, 1);
        uv[3] = new Vector3(1, 0);
    //   
        //frist triangle
        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
       // seconed triangle
        triangles[3] = 2;
        triangles[4] = 3;
        triangles[5] =0;

        mesh.vertices =vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        
        transform.GetComponent<MeshFilter>().mesh=mesh;
    }

    void Update()
    {
        
    }
}
