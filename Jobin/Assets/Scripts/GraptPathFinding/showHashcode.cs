using System.Collections;
using System.Collections.Generic;
using Abed.Utils;
using UnityEngine;

public class showHashcode : MonoBehaviour
{
private void OnDrawGizmos() {
transform.name=transform.GetHashCode().ToString();
}
}
