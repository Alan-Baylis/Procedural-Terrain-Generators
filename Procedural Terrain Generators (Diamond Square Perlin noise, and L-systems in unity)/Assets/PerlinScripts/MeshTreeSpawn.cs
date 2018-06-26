using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTreeSpawn : MonoBehaviour {
    bool spawned;

    public GameObject tree;
    private void OnEnable()
    {
       
    }
    public void Generate()
    {
        if (spawned)
            return;
        spawned = true;
        
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        tree = Resources.Load<GameObject>("Tree");
        foreach (var item in vertices)
        {
            
            if (item.y > 10 && item.y < 15)
            {

                if (Random.Range(0, 200) > 190)
                {
                    Instantiate(tree, GetVertexWorldPosition(item, this.transform), Quaternion.identity);
                }
            }
        }
    }
    public Vector3 GetVertexWorldPosition(Vector3 vertex, Transform owner)
    {
        return owner.localToWorldMatrix.MultiplyPoint3x4(vertex);
    }
}
