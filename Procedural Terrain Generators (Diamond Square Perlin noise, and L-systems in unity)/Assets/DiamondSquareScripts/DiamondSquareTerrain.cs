using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSquareTerrain : MonoBehaviour {

    public int mDivisions;
    public float mSize;
    public float mHeight;
    public Material m;
    public TextureData data;

    Vector3[] mVerts;
    int mVertCount;

	// Use this for initialization
	void Start () {

        data.OnValuesUpdated += UpdateMat;
        CreateTerrain();
	}

    void UpdateMat()
    {
        data.ApplyToMaterial(m);
    }
	
    
    void CreateTerrain()
    {
        mVertCount = (mDivisions + 1) * (mDivisions + 1);
        mVerts = new Vector3[mVertCount];
        Vector2[] uvs = new Vector2[mVertCount];
        int[] tris = new int[mDivisions * mDivisions * 6];

        float halfSize = mSize * 0.5f;
        float divisionSize = mSize / mDivisions;

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int triOffset = 0;

        for (int i = 0; i <= mDivisions; i++)
        {
            for (int j = 0; j <= mDivisions; j++)
            {
                mVerts[i * (mDivisions + 1) + j] = new Vector3(-halfSize + j * divisionSize,0.0f, halfSize-i*divisionSize);
                uvs[i * (mDivisions + 1) + j] = new Vector2((float)i / mDivisions, (float)j / mDivisions);

                if (i < mDivisions && j < mDivisions)
                {
                    int topLeft = i * (mDivisions + 1) + j;
                    int botLeft = (i + 1) * (mDivisions + 1) + j;

                    tris[triOffset] = topLeft;
                    tris[triOffset + 1] = topLeft + 1;
                    tris[triOffset + 2] = botLeft + 1;

                    tris[triOffset + 3] = topLeft;
                    tris[triOffset + 4] = botLeft + 1;
                    tris[triOffset + 5] = botLeft;

                    triOffset += 6;
                }
            }
        }

        //giving random height points to the 4 vertices of our mesh
        mVerts[0].y = Random.Range(-mHeight, mHeight);
        mVerts[mDivisions].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1].y = Random.Range(-mHeight, mHeight);
        mVerts[mVerts.Length - 1 - mDivisions].y = Random.Range(-mHeight, mHeight);

        int iterations = (int)Mathf.Log(mDivisions, 2);
        int numSquares = 1;
        int squareSize = mDivisions;
        for (int i = 0; i < iterations; i++)
        {
            int row = 0;
            for (int j = 0; j < numSquares; j++)
            {
                int col = 0;
                for (int k = 0; k < numSquares; k++)
                {
                    //calling our diamond square algorithm method.
                    DiamondSquareAlgorithm(row, col, squareSize, mHeight);
                    col += squareSize;
                }
                row += squareSize;
            }

            numSquares *= 2;
            squareSize /= 2;
            mHeight *= 0.5f;
        }


        mesh.vertices = mVerts;
        mesh.uv = uvs;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
   
        data.UpdateMeshHeights(m,0,0.6f);
    }

    //this method is following the steps to execute a diamond square algorithm to generate the terrain.
    void DiamondSquareAlgorithm(int row, int col, int size, float offset)
    {
        //getting half the size of the suqare
        int halfSize = (int)(size * 0.5f);
        //getting the top left vertex of the square
        int topLeft = row * (mDivisions + 1) + col;
        //getting the bottom left vertex of the square
        int botLeft = (row + size) * (mDivisions + 1) + col;

        //middle vertex of the square
        int mid = (int)(row + halfSize) * (mDivisions + 1) + (int)(col + halfSize);
        mVerts[mid].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[botLeft].y + mVerts[botLeft + size].y)*0.25f + Random.Range(-offset, offset); // or we can divide by 4 but multiplication is faster than division.

        mVerts[topLeft + halfSize].y = (mVerts[topLeft].y + mVerts[topLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid - halfSize].y = (mVerts[topLeft].y + mVerts[botLeft].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[mid + halfSize].y = (mVerts[topLeft + size].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
        mVerts[botLeft + halfSize].y = (mVerts[botLeft].y + mVerts[botLeft + size].y + mVerts[mid].y) / 3 + Random.Range(-offset, offset);
    }
}
