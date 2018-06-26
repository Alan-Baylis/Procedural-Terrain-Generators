using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LolScript : MonoBehaviour {
    public LayerMask mask;
    public GameObject cube;
    public int amount;
    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(cube, transform.position, Quaternion.identity);
            RaycastHit ray;
            if (Physics.Raycast(obj.transform.position, Vector3.down, out ray, 100f, mask))
            {
                obj.transform.position = ray.point + new Vector3(0, 1f, 0);
            }
        }
     
    }
}
