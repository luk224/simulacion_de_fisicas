using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MassSpringVol : MonoBehaviour
{

    /// <summary>
    /// Default constructor. Zero all. 
    /// </summary>
    public MassSpringVol()
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
    public float densidad = 2;

    public bool Paused;
    public bool verGizmos = false;
    public float TimeStep;
    public Vector3 Gravity;
    public Integration IntegrationMethod;
    public List<Fixer> fixers;
    public float trac = 10f;


    #endregion

    #region OtherVariables
    public List<NodeVol> nodes;
    public List<SpringVol> springs;
    public float Vol;

    private Dictionary<String, NodeVol> dict = new Dictionary<String, NodeVol>();

    //  public static Dictionary<String, Node> dict2 = new Dictionary<String, Node>();
  
    private bool vertexCreados = false;
    public GameObject objetoVisible;
    public TextAsset tetraNode;
    public TextAsset tetraEle;
    Mesh mesh;
    NodoVisible[] nodosVisibles;
    mallaTetraedros mallaTetras;

    #endregion
    float vol;
    #region MonoBehaviour

    public void Start()
    {
        
        springs = new List<SpringVol>();
        nodes = new List<NodeVol>();

        mesh = objetoVisible.GetComponent<MeshFilter>().mesh;//meshFil.mesh;
        nodosVisibles = new NodoVisible[mesh.vertexCount];
        mallaTetras = new mallaTetraedros(tetraNode, tetraEle, this);


        for (int i = 0; i < mallaTetras.Nodos.Length; i++)
        {
            NodeVol nodoNuevo = new NodeVol(mallaTetras.Nodos[i], densidad * mallaTetras.volT / mallaTetras.Nodos.Length, i);
            nodoNuevo.Manager = this;
            foreach (Fixer f in fixers)
            {
                if (f.contain(mallaTetras.Nodos[i]))
                {
                    nodoNuevo.Fixed = true;
                }
            }

            nodes.Add(nodoNuevo);
        }


        vertexCreados = true;

        /* for (int i = 0; i < 4; i++)        {
            vertices[i] = transform.TransformPoint(mesh.vertices[i]);//Se guarda en coordenadas G
         }*/
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            nodosVisibles[i] = new NodoVisible(transform.TransformPoint(mesh.vertices[i]), mallaTetras.Nodos, mallaTetras.tetraedros);
        }


        for (int i = 0; i < mallaTetras.tetraedros.Length; i++)//Se añaden 6 muellles por tetraedro. (4 Nodos)
        {
            int ida = (int)mallaTetras.tetraedros[i].x;
            int idb = (int)mallaTetras.tetraedros[i].y;
            int idc = (int)mallaTetras.tetraedros[i].z;
            int idd = (int)mallaTetras.tetraedros[i].w;



            String clave1;
            String clave2;
            String clave3;
            String clave4;
            String clave5;
            String clave6;


            if (ida < idb) clave1 = ida + "," + idb; else clave1 = idb + "," + ida;
            if (idb < idc) clave2 = idb + "," + idc; else clave2 = idc + "," + idb;
            if (idc < idd) clave3 = idc + "," + idd; else clave3 = idd + "," + idc;
            if (idd < ida) clave4 = idd + "," + ida; else clave4 = ida + "," + idd;
            if (ida < idc) clave5 = ida + "," + idc; else clave5 = idc + "," + ida;
            if (idb < idd) clave6 = idb + "," + idd; else clave6 = idd + "," + idb;

            if (!dict.ContainsKey(clave1)) //A-B
            {
                dict.Add(clave1, nodes[idc]);
                springs.Add(new SpringVol(nodes[ida], nodes[idb], mallaTetras.volT / (mallaTetras.tetraedros.Length / 6), trac));
            }
            else
            {

            }
            if (!dict.ContainsKey(clave2)) //B-C
            {

                dict.Add(clave2, nodes[ida]);
                springs.Add(new SpringVol(nodes[idb], nodes[idc], mallaTetras.volT / (mallaTetras.tetraedros.Length / 6), trac));
            }
            else
            {

            }
            if (!dict.ContainsKey(clave3))//C-D
            {

                dict.Add(clave3, nodes[idb]);
                springs.Add(new SpringVol(nodes[idc], nodes[idd], mallaTetras.volT / (mallaTetras.tetraedros.Length / 6), trac));
            }
            else
            {

            }
            if (!dict.ContainsKey(clave4))//D-A
            {

                dict.Add(clave4, nodes[idd]);
                springs.Add(new SpringVol(nodes[idd], nodes[ida], mallaTetras.volT / (mallaTetras.tetraedros.Length / 6), trac));
            }
            else
            {

            }
            if (!dict.ContainsKey(clave5))//A-C
            {

                dict.Add(clave5, nodes[ida]);
                springs.Add(new SpringVol(nodes[ida], nodes[idc], mallaTetras.volT / (mallaTetras.tetraedros.Length / 6), trac));
            }
            else
            {

            }
            if (!dict.ContainsKey(clave6))//B-D
            {

                dict.Add(clave6, nodes[idb]);
                springs.Add(new SpringVol(nodes[idb], nodes[idd], mallaTetras.volT / (mallaTetras.tetraedros.Length / 6), trac));
            }
            else
            {

            }


        }

    }
    public void OnDrawGizmos()
    {
        if (vertexCreados && verGizmos)
        {
            foreach (NodeVol vert in nodes)
            {
                Gizmos.DrawSphere(vert.Pos, 0.5f);
            }
            Gizmos.color = Color.red;
            foreach (SpringVol spring in springs)
            {
                Gizmos.DrawLine(spring.nodeA.Pos, spring.nodeB.Pos);
            }
            Gizmos.color = Color.green;
            foreach (NodoVisible v in nodosVisibles)
            {
                Gizmos.DrawSphere((v.getPos()), 0.5f);
            }
            Gizmos.color = Color.blue;
            foreach (NodeVol p in nodes)
            {
                Gizmos.DrawSphere(p.Pos, 2.0f);
            }
        }

        /*
        foreach (Spring spring in springs)
        {
            Gizmos.DrawSphere(spring.Pos, 0.05f);
        }*/
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
        switch (IntegrationMethod)
        {
            case Integration.Explicit: stepExplicit(); break;
            case Integration.Symplectic: stepSymplectic(); break;
            default:
                throw new System.Exception("[ERROR] Should never happen!");
        }
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
        /*
        Vector3[] aux = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            aux[i] = nodes[i].Pos;//transform.InverseTransformPoint(nodes[i].Pos);
        }
        vertices = aux;*/
        Vector3[] aux2 = new Vector3[nodosVisibles.Length];
        for (int i = 0; i < nodosVisibles.Length; i++)
        {
            aux2[i] = transform.InverseTransformPoint(nodosVisibles[i].updatePos(nodes));
        }
        objetoVisible.GetComponent<MeshFilter>().mesh.vertices = aux2;


        /*
        Vector3[] aux = new Vector3[mesh.vertices.Length];
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            aux[i] = transform.InverseTransformPoint(nodes[i].Pos);
        }
        meshFil.mesh.vertices = aux;*/
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