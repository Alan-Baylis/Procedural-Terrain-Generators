using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneratorPre : MonoBehaviour
{

    public static int width = 128;
    public static int height = 128;
    public static int depth = 128;
    public float detailScale = 25.0f;
    public int heightOffset = 100;
    public int heightScale = 20;
    public bool spawnTrees = true;
    public bool spawnWall = true;

    public GameObject TopPrefab;
    public int topLvl = 50;

    public GameObject Midprefab;
    public int midLvl = 20;

    public GameObject LowPrefab;
    public int LowLvl = 12;

    public GameObject VeryLowPrefab;
    public int veryLowLvl = 9;


    public GameObject Tree;
    public GameObject Water;


    public int waterLvl = 9;

    public int seed = 123456;//(int)Network.time * 10; // this could be anything, it is set to teh current delta time so that the terrain will always be different upon runtime.
 
    void Start()
    {
        GameObject curtile = null;
        Vector3 tr = Vector3.zero;
        Vector3 blockPos = Vector3.zero;
        //these floats are created to resize the positioning of the cube according to the size of the cube itself
        float divideX = (1 / TopPrefab.transform.localScale.x);
        float divideY = (1 / TopPrefab.transform.localScale.y);
        float divideZ = (1 / TopPrefab.transform.localScale.z);

        GameObject obj = Instantiate(Water, new Vector3(width / 2, waterLvl, height / 2),Water.transform.localRotation);
        // obj.transform.localScale = new Vector3((128/2))
        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {

                int y = (int)(Mathf.PerlinNoise((x + seed) / detailScale, (z + seed) / detailScale) * heightScale) + heightOffset;



                tr = new Vector3(x, y, z);
                blockPos = new Vector3(x / divideX, y / divideY, z / divideZ);
                curtile = CreateBlock(y, blockPos, true);
                if ((blockPos.x <= 0.1f || blockPos.z <= 0.1f || blockPos.x >= (width - 1.2f) || blockPos.z >= (depth - 1.2f)) && spawnWall == true)
                {
                    if (curtile != null)
                    {
                        curtile.transform.localScale = new Vector3(curtile.transform.localScale.x, 100, curtile.transform.localScale.z);
                        curtile.GetComponent<MeshRenderer>().enabled = false;
                    }
                }
            }
        }

    }
    GameObject CreateBlock(float y, Vector3 blockPos, bool create)
    {
        y = blockPos.y;

        if (!((blockPos.x <= 0.1f || blockPos.z <= 0.1f || blockPos.x >= (width - 1.2f) || blockPos.z >= (depth - 1.2f)) && spawnWall == true))
        {
            if (blockPos.y > waterLvl && y < LowLvl)
            {
                if (Random.Range(0, 200) > 197 && spawnTrees == true)
                {
                    Instantiate(Tree, blockPos, Quaternion.identity);
                }
            }
        }
        if (y < veryLowLvl)
        {
            return Instantiate(VeryLowPrefab, blockPos, Quaternion.identity);
        }
        else if (y < LowLvl)
        {
            return Instantiate(LowPrefab, blockPos, Quaternion.identity);
        }
        else if(y <midLvl)
        {
            return Instantiate(Midprefab, blockPos, Quaternion.identity);
        }
   
        else if (y < topLvl)
        {
            return Instantiate(TopPrefab, blockPos, Quaternion.identity);
        }

        return null;
    }
}
