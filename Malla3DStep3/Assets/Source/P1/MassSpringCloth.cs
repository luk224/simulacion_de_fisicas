using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpringCloth : MonoBehaviour
{

    /// <summary>
    /// Default constructor. Zero all. 
    /// </summary>
    public MassSpringCloth()
    {
        Paused = false;
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
    public float mass;
    public bool Paused;
    public float TimeStep;
    public Vector3 Gravity;
    public Integration IntegrationMethod;
    public List<Fixer> fixers;
    public float flex = 1f;
    public float trac = 10f ;


    #endregion

    #region OtherVariables
    public MeshFilter malla;
    public List<Node> nodes;
    public List<Spring> springs;
    public MeshFilter meshFil;

    public static Dictionary<String, Node> dict = new Dictionary<String, Node>();
  //  public static Dictionary<String, Node> dict2 = new Dictionary<String, Node>();

    Mesh mesh;
    #endregion

    #region MonoBehaviour

    public void Start()


    {
        springs = new List<Spring>();
        nodes = new List<Node>();
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        meshFil = GetComponent<MeshFilter>();
        mesh = meshFil.mesh;
        int[] triangs = mesh.triangles;

        Vector3[] vertices = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            vertices[i] = transform.TransformPoint(mesh.vertices[i]);//Se guarda en coordenadas G
        }


        for (int i = 0; i < vertices.Length; i++)
        {

            Node nodoNuevo = new Node(vertices[i], mass / vertices.Length, i);
            nodoNuevo.Manager = this;
            foreach (Fixer fixer in fixers)
            {
                if (fixer.contain(vertices[i]))
                {
                    nodoNuevo.Fixed = true;
                }
                
            }
            nodes.Add(nodoNuevo);

        }

        for (int i = 0; i < triangs.Length; i += 3)//Se añaden 3 muellles por triangulo.
        {
            int a = i;
            int b = i + 1;
            int c = i + 2;
            String clave1;
            String clave2;
            String clave3;
            int ida = triangs[a];
            int idb = triangs[b];
            int idc = triangs[c];
            if(ida<idb) clave1 = ida + "," + idb; else clave1 = idb + "," + ida;
            if(idb<idc) clave2 = idb + "," + idc; else clave2 = idc + "," + idb;
            if(idc<ida) clave3 = idc + "," + ida; else clave3 = ida + "," + idc;
          
            if (!dict.ContainsKey(clave1))
            {
                
                dict.Add(clave1, nodes[idc]);
                springs.Add(new Spring(nodes[ida], nodes[idb]));
            }
            else
            {
                Spring s = new Spring(nodes[triangs[c]],dict[clave1]);
                s.Stiffness = flex;
                springs.Add(s);
            }
            if (!dict.ContainsKey(clave2))
            {
               
                dict.Add(clave2, nodes[ida]);
                springs.Add(new Spring(nodes[idb], nodes[idc]));
            }
            else
            {
                Spring s = new Spring(nodes[triangs[a]], dict[clave2]);
                s.Stiffness = flex;
                springs.Add(s);
            }
            if (!dict.ContainsKey(clave3))
            {

                dict.Add(clave3, nodes[idb]);
                springs.Add(new Spring(nodes[idc], nodes[ida]));
            }
            else
            {
                Spring s = new Spring(nodes[triangs[b]], dict[clave3]);
                s.Stiffness = flex;
                springs.Add(s);
            }


        }

    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Spring spring in springs)
        {
            Gizmos.DrawSphere(spring.Pos, 0.05f);
            Gizmos.DrawLine(spring.nodeA.Pos, spring.nodeB.Pos);
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

        updateVertex();
    }

    private void updateVertex()
    {
        Vector3[] aux = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            aux[i] = transform.InverseTransformPoint(nodes[i].Pos);
        }
        meshFil.mesh.vertices = aux;
    }

    #endregion

    /// <summary>
    /// Performs a simulation step in 1D using Explicit integration.
    /// </summary>
    private void stepExplicit()
    {
        foreach (Node node in nodes)
        {
            node.Force = Vector3.zero;
            node.ComputeForces();
        }

        foreach (Spring spring in springs)
        {
            spring.ComputeForces();
        }

        foreach (Node node in nodes)
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
        foreach (Node node in nodes)
        {
            node.Force = Vector3.zero;
            node.ComputeForces();
        }

        foreach (Spring spring in springs)
        {
            spring.ComputeForces();
        }

        foreach (Node node in nodes)
        {
            if (!node.Fixed)
            {
                node.Vel += node.Force * TimeStep / node.Mass;
                node.Pos += node.Vel * TimeStep;
            }
        }
    }

}
