using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerSub : MonoBehaviour {


    public GameObject cube;
    public float Iterations;

    public float rotateBy;
	// Use this for initialization
	void Start () {


        float numOfCubes =Mathf.Pow( ((2f * Iterations) + 1f),2f);

      Transform lastTransform = this.transform;
        Vector3 rotation = new Vector3(0, 0, 0);
        for (int i = 0; i <  numOfCubes; i++)
        {
           GameObject obj = Instantiate(cube, lastTransform.position, Quaternion.identity);
            rotation = new Vector3(0, (rotation.y + rotateBy)%360,0);
            obj.transform.localEulerAngles =(rotation);
            obj.transform.Translate(obj.transform.forward * 20);
            lastTransform = obj.transform;
        }    

    }
	
	
}
