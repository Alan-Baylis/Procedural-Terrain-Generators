using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TerrainLsystems : MonoBehaviour
{

    public bool debug = true;

    public Dictionary<char, string> rules = new Dictionary<char, string>();

    public LayerMask mask;

    public int maxIterations;
    public int minIterations;
    private int iterations = 5;
    public string input = "F";
    public string output;

    private string result;
    int incrementer = 2;
    int currentIncrement = 2;

    public List<GameObject> cubes = new List<GameObject>();

    public GameObject cube;

    void Awake()
    {
        output = string.Empty;
        iterations = maxIterations;// Random.Range(minIterations, maxIterations) + 1;
        GenerateCubes();


        // List<MeshFilter> leafs = new List<MeshFilter>();
        // List<MeshFilter> Branch = new List<MeshFilter>();


    }

    public void GenerateCubes()
    {
        rules.Clear();
        
        rules.Add('F', "X+F");
       // rules.Add('X', "FX");
        output = input;
        for (int i = 0; i < iterations; i++)
        {
            output = applyRules(output);
        }
        SpawnCubes(output);
    }



    string applyRules(string p_input)
    {
        StringBuilder sb = new StringBuilder();
        // Loop through characters in the input string
        foreach (char c in p_input)
        {
            // If character matches key in rules, then replace character with rhs of rule
            if (rules.ContainsKey(c))
            {
                sb.Append(rules[c]);
            }
            // If not, keep the character
            else
            {
                sb.Append(c);
            }
        }
        // Return string with rules applied
        return sb.ToString();
    }

    struct point
    {
        public point(Vector3 rP, Vector3 rA, float rL) { Point = rP; Angle = rA; BranchLength = rL; }
        public Vector3 Point;
        public Vector3 Angle;
        public float BranchLength;
    }

    private Vector3[] sides = new Vector3[]
    {
   //  new Vector3(-1,-1,0),
    //    new Vector3(-1,-1,1),
    //    new Vector3(0,-1,1),
     //   new Vector3(1,-1,1),
     //   new Vector3(1,-1,0),
     //   new Vector3(1,-1,-1),
     //   new Vector3(0,-1,-1),
      //  new Vector3(-1,-1,-1),

        new Vector3(-1,0,0),
        new Vector3(-1,0,1),
        new Vector3(0,0,1),
        new Vector3(1,0,1),
        new Vector3(1,0,0),
        new Vector3(1,0,-1),
        new Vector3(0,0,-1),
        new Vector3(-1,0,-1),

           new Vector3(-1,1,0),
        new Vector3(-1,1,1),
        new Vector3(0,1,1),
        new Vector3(1,1,1),
        new Vector3(1,1,0),
        new Vector3(1,1,-1),
        new Vector3(0,1,-1),
        new Vector3(-1,1,-1)
    };

    public int sideRule = 0;
    private List<GameObject> spawnedCubes = new List<GameObject>();
    void SpawnCubes(string p_input)
    {
        Stack<point> returnValues = new Stack<point>();
        point lastPoint = new point(Vector3.zero, Vector3.zero, 1f);
        returnValues.Push(lastPoint);
        Instantiate(cube);
        foreach (char c in p_input)
        {
            switch (c)
            {
                case 'X':
                    for (int i = 0; i < sides.Length; i++)
                    {

                       cube = cubes[Random.Range(0, cubes.Count)];
                        GameObject gameObject = Instantiate(cube, (sides[i] * currentIncrement), Quaternion.identity);
                        spawnedCubes.Add(gameObject);
                        //foreach (Transform gamebjectChild in gameObject.transform)
                        //{
                        //    gamebjectChild.SetParent(null);
                        //    spawnedCubes.Add(gamebjectChild.gameObject);
                        //}
                        //Destroy(gameObject);

                    }                  
                    break;
                case '+':
                    currentIncrement += incrementer;
                    break;

            }
        }
    }

  

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(RunM());
        }
    }

    IEnumerator RunM()
    {
        foreach (var item in spawnedCubes)
        {
            yield return new WaitForSeconds(0.01f);
            RaycastHit ray;
            if (Physics.Raycast(item.transform.position, Vector3.down, out ray, 100f, mask))
            {
                item.transform.position = ray.point + new Vector3(0, 1f, 0);
            }
        }
    }
}
