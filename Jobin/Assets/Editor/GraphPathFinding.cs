using UnityEngine;
using UnityEditor;

public class GraphPathFinding : EditorWindow {

     GameObject NodeObj;
     int spaceBetwineNodes=0;
     float updateSpeed;
     AutoNodeG autoNode;
    [MenuItem("AbedTools/GraphPathFinding")]
    private static void ShowWindow() {
        
        var window = GetWindow<GraphPathFinding>();
        window.titleContent = new GUIContent("GraphPathFinding");
        window.Show();
    }
    private void OnGUI()
     {
        GUILayout.Label("Generate Node",EditorStyles.boldLabel);
        
        spaceBetwineNodes =EditorGUILayout.IntField("space between node ",spaceBetwineNodes);
        NodeObj = EditorGUILayout.ObjectField( "node" , NodeObj , typeof(GameObject),true ) as GameObject;
        updateSpeed = EditorGUILayout.FloatField("update speed " ,updateSpeed);
        autoNode =FindObjectOfType<AutoNodeG>();
        if(GUILayout.Button("Bake")) autoNode.BakeNode(spaceBetwineNodes, updateSpeed, NodeObj);
        if(GUILayout.Button("delete nodes")) autoNode.DeleteOldNode();
     }
    

}
