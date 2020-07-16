using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodoVisible  {

    
    public MassSpringVol Manager;

    public Vector3 Pos;
    public Vector3 Vel;
    public Vector3 Force;
    private int id;
    public Vector3 [] tetraedro;
    public float[] w;
    public Vector4 tetra; //Almacena el índice de los vertices del tetraedro en el que está contenido.


    public NodoVisible(Vector3 pos,Vector3 [] verticesTetra, Vector4[] tetras )
    {
        tetraedro = new Vector3[4];
        tetra = new Vector4();
        w = new float[4];
        this.Pos = pos;
        for(int i =0; i<tetras.Length; i++)
        {
            if(PointInTetrahedron(verticesTetra[(int)tetras[i].x], verticesTetra[(int)tetras[i].y], verticesTetra[(int)tetras[i].z], verticesTetra[(int)tetras[i].w], pos)){
                tetra = tetras[i];
            }

        }
        
        float Vol3 = Math.Abs( Vector3.Dot(verticesTetra[(int)tetra.y] - verticesTetra[(int)tetra.x],  Vector3.Cross(verticesTetra[(int)tetra.z] - verticesTetra[(int)tetra.x], pos- verticesTetra[(int)tetra.x]) ) )/ 6;
        float Vol2 = Math.Abs(Vector3.Dot(verticesTetra[(int)tetra.y] - verticesTetra[(int)tetra.x], Vector3.Cross(pos - verticesTetra[(int)tetra.x], verticesTetra[(int)tetra.w] - verticesTetra[(int)tetra.x]))) / 6;
        float Vol1 = Math.Abs(Vector3.Dot(pos - verticesTetra[(int)tetra.x], Vector3.Cross(verticesTetra[(int)tetra.z] - verticesTetra[(int)tetra.x], verticesTetra[(int)tetra.w] - verticesTetra[(int)tetra.x]))) / 6;
        float Vol0 = Math.Abs(Vector3.Dot(verticesTetra[(int)tetra.y] - pos, Vector3.Cross(verticesTetra[(int)tetra.z] - pos, verticesTetra[(int)tetra.w] - pos))) / 6;
        float VolT = Math.Abs(Vector3.Dot(verticesTetra[(int)tetra.y] - verticesTetra[(int)tetra.x], Vector3.Cross(verticesTetra[(int)tetra.z] - verticesTetra[(int)tetra.x], verticesTetra[(int)tetra.w] - verticesTetra[(int)tetra.x]))) / 6;
        w[0] = Vol0 / VolT;
        w[1] = Vol1 / VolT;
        w[2] = Vol2 / VolT;
        w[3] = Vol3 / VolT;

    }


    public Vector3 updatePos(List<NodeVol> vertex)
    {
        Pos = Vector3.zero;
        for(int i =0;i<4; i++)
        {
            Pos += w[i] * vertex[(int)tetra[i]].Pos;
        }
        return Pos;

    }


    public Vector3 getPos()
    {
        return Pos;
    }


    //https://stackoverflow.com/questions/25179693/how-to-check-whether-the-point-is-in-the-tetrahedron-or-not
    bool SameSide(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 p)
    {
        Vector3 normal = Vector3.Cross(v2 - v1, v3 - v1);
        float dotV4 = Vector3.Dot(normal, v4 - v1);
        float dotP = Vector3.Dot(normal, p - v1);
        return (dotP*dotV4 >=0);
    }
    bool PointInTetrahedron(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, Vector3 p)
    {
        return SameSide(v1, v2, v3, v4, p) &&
               SameSide(v2, v3, v4, v1, p) &&
               SameSide(v3, v4, v1, v2, p) &&
               SameSide(v4, v1, v2, v3, p);
    }


}
