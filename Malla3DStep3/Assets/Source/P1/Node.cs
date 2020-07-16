using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node  {

    #region InEditorVariables



    #endregion

    public float Mass;
    public bool Fixed;


    public MassSpringCloth Manager;

    public Vector3 Pos;
    public Vector3 Vel;
    public Vector3 Force;
    private int id;


    public Node (Vector3 pos, float mass, int id)
    {
        this.id = id;
        this.Pos = pos;
        this.Mass = mass;
    }

    public void ComputeForces ()
    {
        Force += Mass * Manager.Gravity;
    }
    public int getID()
    {
        return this.id;
    }
}
