using Abed.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Abed.GraphPathFinding
{
    [ExecuteInEditMode]
    public class NodeG : MonoBehaviour
    {
        public Vector3 pos;
        public Vector2 Key;
        public List<NodeG> ConectedList;
        public NodeG exploredFrome;
        public NodeType Type = NodeType.Normal;
        public Vector3 Dir;

        private void Start()
        {
            PrimitiveSetup();
            DebugVistual();
            FindConected();
        }
        private void LateUpdate()
        {
            PrimitiveSetup();
            DebugVistual();
            FindConected();
        }

        private void DebugVistual()
        {
            var SR = GetComponent<SpriteRenderer>();
            if (Dir == Vector3.left) SR.flipX = true;
            if (Dir == Vector3.right) SR.flipX = false;

            switch (Type)
            {
                case NodeType.Normal:
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case NodeType.Jump:
                    transform.rotation = Quaternion.Euler(0, 0, +90);
                    break;
                case NodeType.fall:
                    transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
        }

        private void PrimitiveSetup()
        {
            Vector3 RoundPos = transform.position.FloorVector();
            transform.position = RoundPos;
            transform.name = ((RoundPos.x) + "/" + (RoundPos.y));
            Key = new Vector2(RoundPos.x, RoundPos.y);
            pos = new Vector2(RoundPos.x, RoundPos.y);
        }
        void FindConected()
        {
            Path Conectnod = FindObjectOfType<Path>();
            ConectedList = Conectnod.GetInRangeNodeList(pos, 5, false);
        }

        //private void OnDrawGizmos()
        //{
        //    foreach(NodeG nod in ConectedList)
        //    {
        //        Gizmos.color=Color.black;
        //        Gizmos.DrawLine(pos, nod.pos);
        //    }
        //}
    }
}
