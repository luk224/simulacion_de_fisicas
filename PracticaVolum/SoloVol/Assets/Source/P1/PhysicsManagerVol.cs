using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManagerVol {
    /// <summary>
    /// Default constructor. Zero all. 
    /// </summary>
    public PhysicsManagerVol()
    {
        Paused = true;
        TimeStep = 0.01f;
        Gravity = new Vector3(0.0f, -9.81f, 0.0f);
        IntegrationMethod = Integration.Explicit;
    }

    /// <summary>
    /// Integration method.
    /// </summary>
    public enum Integration
    {
        Explicit = 0,
        Symplectic = 1,
    };

    #region InEditorVariables

    public bool Paused;
    public float TimeStep;
    public Vector3 Gravity;
    public Integration IntegrationMethod;
    public List<NodeVol> nodes;
    public List<SpringVol> springs;

    #endregion

    #region OtherVariables
    #endregion

    #region MonoBehaviour

    public void Start()
    {
        foreach (NodeVol node in nodes)
        {
            //  node.Manager = this;
        }

        foreach (SpringVol spring in springs)
        {
            //spring.Manager = this;
        }
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
            Paused = !Paused;

    }

    public void FixedUpdate()
    {
        if (Paused)
            return; // Not simulating

        // Select integration method
        switch (IntegrationMethod)
        {
            case Integration.Explicit: stepExplicit(); break;
            case Integration.Symplectic: stepSymplectic(); break;
            default:
                throw new System.Exception("[ERROR] Should never happen!");
        }
    }

    #endregion

    /// <summary>
    /// Performs a simulation step in 1D using Explicit integration.
    /// </summary>
    private void stepExplicit()
    {
        foreach (NodeVol node in nodes)
        {
            node.Force = Vector3.zero;
            node.ComputeForces();
        }

        foreach (SpringVol spring in springs)
        {
            spring.ComputeForces();
        }

        foreach (NodeVol node in nodes)
        {
            if (!node.Fixed)
            {
                node.Pos += node.Vel * TimeStep;
                node.Vel += node.Force * TimeStep / node.Mass;
            }
        }

    }

    /// <summary>
    /// Performs a simulation step in 1D using Symplectic integration.
    /// </summary>
    private void stepSymplectic()
    {
        foreach (NodeVol node in nodes)
        {
            node.Force = Vector3.zero;
            node.ComputeForces();
        }

        foreach (SpringVol spring in springs)
        {
            spring.ComputeForces();
        }

        foreach (NodeVol node in nodes)
        {
            if (!node.Fixed)
            {
                node.Vel += node.Force * TimeStep / node.Mass;
                node.Pos += node.Vel * TimeStep;
            }
        }
    }

}
