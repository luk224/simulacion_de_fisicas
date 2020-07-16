using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {

    #region InEditorVariables

    public float Stiffness;
    public Node nodeA;
    public Node nodeB;

    #endregion

    public float Length0;
    public float Length;

    public PhysicsManager Manager;

    // Use this for initialization
    void Start () {
        this.nodeA.Start();
        this.nodeB.Start();
        this.Length0 = this.Length = (this.nodeA.Pos - this.nodeB.Pos).magnitude;

    }

    // Update is called once per frame
    void Update () {

        this.Length = (this.nodeA.Pos - this.nodeB.Pos).magnitude;


        this.transform.position = 0.5f * (this.nodeA.Pos+this.nodeB.Pos);  
        //The default length of a cylinder in Unity is 2.0
        this.transform.localScale = new Vector3(this.transform.localScale.x, this.Length / 2.0f, this.transform.localScale.z);

        //Rotar el muelle:
        Vector3 yaxis = new Vector3(0.0f,1.0f,0.0f);
        Vector3 dir = nodeA.transform.position - nodeB.transform.position;
        dir = dir * (1.0f / dir.magnitude);
        transform.rotation = Quaternion.FromToRotation(yaxis, dir);

	}
    public void addForces()
    {
        Vector3 ForceA = Stiffness * (Length - Length0) * (nodeB.Pos - nodeA.Pos) / Length;
        nodeA.Force += ForceA;
        nodeB.Force -= ForceA;
    }
}
