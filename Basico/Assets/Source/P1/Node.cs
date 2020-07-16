using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    #region InEditorVariables

    public float Mass;
    public bool Fixed = false;
    #endregion

    public Vector3 Pos;
    public Vector3 Vel;
    public Vector3 Force;


    public PhysicsManager Manager;

    // Use this for initialization
    public void Start () {

        this.Pos = this.transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        this.transform.position = this.Pos;
	}
    public void addForces()
    {
        Force += Mass * Manager.Gravity;

    }
}
