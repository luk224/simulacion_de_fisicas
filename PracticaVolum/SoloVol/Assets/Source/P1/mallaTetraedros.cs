using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class mallaTetraedros  {

    MassSpringVol Manager;
    public Vector4 [] tetraedros;
    public Vector3[] Nodos;
    public float[] vols;
    public float volT;


    String dataNode;
    String dataEle;
    public mallaTetraedros(TextAsset tetraNode, TextAsset tetraEle, MassSpringVol Manager)
    {
        volT = 0;

        this.Manager = Manager;
        dataNode = tetraNode.text;
        dataEle = tetraEle.text;

        String [] datasNode;
        char[] delimiters = new char[] { ' ','\n', '\r',};
        datasNode = dataNode.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
        int nNodos = Int32.Parse(datasNode[0]);
        Nodos = new Vector3[nNodos];//El primer numero es el número de nodos.
        for (int i =4;i<datasNode.Length;i+=4)
        {
            if (datasNode[i].Contains("#")) break;
            Nodos[Int32.Parse(datasNode[i])] = (new Vector3(  float.Parse(datasNode[i+1])*(-1) , float.Parse(datasNode[i + 2]), float.Parse(datasNode[i + 3])));
            //Nodos[Int32.Parse(datasNode[i])].x *= -1;
            Nodos[Int32.Parse(datasNode[i])] = Manager.gameObject.transform.TransformPoint(Nodos[Int32.Parse(datasNode[i])]);
        }


        String[] datasEle;
        delimiters = new char[] { ' ','\n', '\r' };
        datasEle = dataEle.Split(delimiters,StringSplitOptions.RemoveEmptyEntries);
        int nEles = Int32.Parse(datasEle[0]);
        tetraedros = new Vector4[nEles];
        
        for (int i =3; i<datasEle.Length; i+=5)
        {
            if (datasEle[i].Contains("#")) break;
            tetraedros[Int32.Parse(datasEle[i])] = (new Vector4(  float.Parse(datasEle[i+1])      , float.Parse(datasEle[i + 2]), float.Parse(datasEle[i + 3]), float.Parse(datasEle[i + 4])));

        }
        vols = new float[nEles];
        for (int i =0; i<nEles; i++)
        {
            vols[i] = Math.Abs(Vector3.Dot(Nodos[(int)tetraedros[i].y] - Nodos[(int)tetraedros[i].x], Vector3.Cross(Nodos[(int)tetraedros[i].z] - Nodos[(int)tetraedros[i].x], Nodos[(int)tetraedros[i].w] - Nodos[(int)tetraedros[i].x]))) / 6;
            volT += vols[i];
        }
       


    }


	
	
}
