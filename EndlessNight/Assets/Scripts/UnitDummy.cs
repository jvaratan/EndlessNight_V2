using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDummy : MonoBehaviour {

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    public void Moving(float speed)
    {
        transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
    }
}
