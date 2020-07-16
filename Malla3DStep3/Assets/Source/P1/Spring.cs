using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring {

    #region InEditorVariables

   
    public Node nodeA;
    public Node nodeB;

    #endregion

    public MassSpringCloth Manager;

    public float Length0;
    public float Length;
    public Vector3 Pos;
    public float Stiffness;

    public Spring(Node a, Node b)
    {
        Stiffness = 10;
        nodeA = a;
        nodeB = b;
        Length0 = Length = (nodeA.Pos - nodeB.Pos).magnitude;
        Pos = 0.5f * (nodeA.Pos + nodeB.Pos);

    }

    // Use this for initialization
    void Start () {
       // nodeA.Start();
        //nodeB.Start();
        Length0 = Length = (nodeA.Pos - nodeB.Pos).magnitude;
        Pos = 0.5f * (nodeA.Pos + nodeB.Pos);
    }

    // Update is called once per frame
    void Update () {

        Vector3 yaxis = new Vector3(0.0f, 1.0f, 0.0f);
       

       // transform.position = Pos;
        //The default length of a cylinder in Unity is 2.0
     //   transform.localScale = new Vector3(transform.localScale.x, Length / 2.0f, transform.localScale.z);
       // transform.rotation = Quaternion.FromToRotation(yaxis, dir);
    }

    public void ComputeForces()
    {
      
        Pos = 0.5f * (nodeA.Pos + nodeB.Pos);

        Vector3 dir = nodeA.Pos - nodeB.Pos;
        Length = dir.magnitude;
        dir = dir * (1.0f / Length);
        Vector3 Force = -Stiffness * (Length - Length0) * dir;
        nodeA.Force += Force;
        nodeB.Force -= Force;
    }

}
