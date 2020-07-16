﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Basic physics manager capable of simulating a given ISimulable
/// implementation using diverse integration methods: explicit,
/// implicit, Verlet and semi-implicit.
/// </summary>
public class PhysicsManager : MonoBehaviour 
{
	/// <summary>
	/// Default constructor. Zero all. 
	/// </summary>
	public PhysicsManager()
	{
		this.Paused = true;
		this.TimeStep = 0.01f;
		this.Gravity = new Vector3 (0.0f, -9.81f, 0.0f);
		this.IntegrationMethod = Integration.Explicit;
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
    public Node nodeHigh;
    public Node nodeLow;
    public Spring spring;
    public List<Node> nodes;
    public List<Spring> springs;

    #endregion

    #region OtherVariables
    #endregion

    #region MonoBehaviour

    public void Start()
    {
        foreach(Node node in nodes)
        {
            node.Manager = this;
        }
        foreach(Spring spring in springs)
        {
            spring.Manager = this;
        }

  
    }

	public void Update()
	{
		if (Input.GetKeyUp (KeyCode.P))
			this.Paused = !this.Paused;

    }

    public void FixedUpdate()
    {
        if (this.Paused)
            return; // Not simulating

        // Select integration method
        switch (this.IntegrationMethod)
        {
            case Integration.Explicit: this.stepExplicit(); break;
            case Integration.Symplectic: this.stepSymplectic(); break;
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

        foreach(Node node in nodes)
        {
            node.Force = Vector3.zero;
            
        }
        foreach(Node node in nodes)
        {
            node.addForces();
        }
        foreach (Spring p in springs)
        {
            p.addForces();
        }
        foreach (Node node in nodes)
        {
            node.Pos+= TimeStep * nodeLow.Vel;
            node.Vel += TimeStep * nodeLow.Force / nodeLow.Mass;
        }
       
    }

	/// <summary>
	/// Performs a simulation step in 1D using Symplectic integration.
	/// </summary>
	private void stepSymplectic()
	{


        foreach (Node node in nodes)
        {
            node.Force = Vector3.zero;

        }
        foreach (Node node in nodes)
        {
            node.addForces();
        }
        foreach (Spring p in springs)
        {
            p.addForces();
        }
        foreach (Node node in nodes)
        {
            if (!node.Fixed){
                node.Vel += TimeStep * nodeLow.Force / nodeLow.Mass;
                node.Pos += TimeStep * nodeLow.Vel;
            }
          
        }


    }


}
