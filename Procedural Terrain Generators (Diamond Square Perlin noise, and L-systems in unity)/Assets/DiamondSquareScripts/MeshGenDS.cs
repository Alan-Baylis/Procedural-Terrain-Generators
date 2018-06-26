using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//another way to create a mesh. This will be used for the diamond square implementation
public class MeshGenDS : MonoBehaviour {

	Vector3[] mVerts;
    Vector2[] mUVs;
    int[] mTriangles;

	void Start () {

        mVerts = new Vector3[4];
        mUVs = new Vector2[4];
        mTriangles = new int[6];

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        mVerts[0] = new Vector3(-1.0f, 0.0f, 1.0f);
        mVerts[1] = new Vector3(1.0f, 0.0f, 1.0f);
        mVerts[2] = new Vector3(-1.0f, 0.0f, -1.0f);
        mVerts[3] = new Vector3(1.0f, 0.0f, -1.0f);

        mUVs[0] = new Vector2(0.0f, 0.0f);
        mUVs[1] = new Vector2(1.0f, 0.0f);
        mUVs[2] = new Vector2(0.0f, 1.0f);
        mUVs[3] = new Vector2(1.0f, 1.0f);

        mTriangles[0] = 0;
        mTriangles[1] = 1;
        mTriangles[2] = 3;

        mTriangles[3] = 0;
        mTriangles[4] = 3;
        mTriangles[5] = 2;

        mesh.vertices = mVerts;
        mesh.uv = mUVs;
        mesh.triangles = mTriangles;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }


}
