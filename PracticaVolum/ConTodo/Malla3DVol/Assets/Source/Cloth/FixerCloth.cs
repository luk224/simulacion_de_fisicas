using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixerCloth : MonoBehaviour {

    Bounds b;
	// Use this for initialization
	void Start () {
       b= gameObject.GetComponent<Renderer>().bounds;
	}

    public bool contain(Vector3 v)
    {
        if (b.Contains(v)) return true; else return false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
