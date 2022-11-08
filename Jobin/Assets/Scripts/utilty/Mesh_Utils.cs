using UnityEngine;

namespace Abed.Utils
{
    public class Mesh_Utils
    {
        public static void CreateEmptyMeshArrys(int quadCount, out Vector3[] vertciese, out Vector2[] uv, out int[] triangle)
        {
            vertciese = new Vector3[4 * quadCount];
            uv = new Vector2[4 * quadCount];
            triangle = new int[6 * quadCount];
        }
        #region Setmesharray
        public static void SetMeshArrays(int index, Vector3[] vertciese, Vector2[] uv, int[] triangle, Vector3 size, Vector3 postion, Vector2 uv00, Vector2 uv11)
        {
            int vindex = index * 4;

            int vindex0 = vindex;
            int vindex1 = vindex + 1;
            int vindex2 = vindex + 2;
            int vindex3 = vindex + 3;


            vertciese[vindex0] = postion;
            vertciese[vindex1] = new Vector3(postion.x, postion.y + size.y);
            vertciese[vindex2] = new Vector3(postion.x + size.x, postion.y);
            vertciese[vindex3] = new Vector3(postion.x + size.x, postion.y + size.y);

            uv[vindex0] = new Vector2(uv00.x, uv00.y);
            uv[vindex1] = new Vector2(uv00.x, uv11.y);
            uv[vindex2] = new Vector2(uv11.y, uv00.x);
            uv[vindex3] = new Vector2(uv11.y, uv11.y);

            int tindex = index * 6;
            triangle[tindex + 0] = vindex0;
            triangle[tindex + 1] = vindex1;
            triangle[tindex + 2] = vindex2;

            triangle[tindex + 3] = vindex1;
            triangle[tindex + 4] = vindex3;
            triangle[tindex + 5] = vindex2;
        }
        public static void SetMeshArrays(int index, Vector3[] vertciese, Vector2[] uv, int[] triangle, int size, Vector3 postion)
        {
            Vector3 newSize = new Vector3(size, size);
            Vector2 uv00 = Vector2.zero;
            Vector2 uv11 = Vector2.one;
            SetMeshArrays(index, vertciese, uv, triangle, newSize, postion, uv00, uv11);
        }

        #endregion
        public static Mesh CreateQuaedMesh(Vector3 size, Vector3 position, Vector2 uv00, Vector2 uv11)
        {
            CreateEmptyMeshArrys(1, out Vector3[] vertciese, out Vector2[] uv, out int[] triangle);
            SetMeshArrays(0, vertciese, uv, triangle, size, position, uv00, uv11);
            Mesh mymesh = new Mesh();
            mymesh.vertices = vertciese;
            mymesh.uv = uv;
            mymesh.triangles = triangle;
            return mymesh;
        }
    }

}