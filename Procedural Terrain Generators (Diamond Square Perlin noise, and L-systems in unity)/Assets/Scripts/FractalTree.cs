using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class FractalTree : MonoBehaviour
{

    public bool debug = true;

    public Dictionary<char, string> rules = new Dictionary<char, string>();



    public int maxIterations;
    public int minIterations;
    private int iterations = 3;
    public string input = "F";
    private string output;

    public string result;


    List<point> points = new List<point>();
    List<GameObject> branches = new List<GameObject>();

    public GameObject cylinder;
    public GameObject leafs;


    // Use this for initialization

    public Material LeafMaterial;
    public Material BranchMaterial;
    void Start()
    {
        iterations = Random.Range(minIterations, maxIterations) + 1;
        GenerateTree();


       // List<MeshFilter> leafs = new List<MeshFilter>();
       // List<MeshFilter> Branch = new List<MeshFilter>();
  

    }
    public GameObject AddTogether(List<MeshFilter> meshFilters, Material m, string Name)
    {
        CombineInstance[] combine = new CombineInstance[meshFilters.Count];
        int i = 0;
        while (i < meshFilters.Count)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
         
            i++;
        }
        GameObject Instance = new GameObject();
        MeshFilter mf =Instance.AddComponent<MeshFilter>();
        MeshRenderer mr=Instance.AddComponent<MeshRenderer>();
        GameObject obInstance =Instantiate(Instance);
         
        
        mf.mesh
             = new Mesh();
        mf.mesh.CombineMeshes(combine);
        mr.sharedMaterial = m;
        obInstance.name = Name;
        return obInstance;
    }


    public void GenerateTree()
    {
        rules.Clear();
        points.Clear();
        foreach (GameObject obj in branches)
        {
            //Object.Destroy(obj);
            Object.DestroyImmediate(obj);
        }
        branches.Clear();

        // Rules
        // Key is replaced by value
        //Set1
        //  rules.Add('F', "F[-F]F[+F][F]");
        //Set1
        //Set2
        // rules.Add('F', "FF-[-F+F+F]+[+F-F-F]");
        //EndSet2
        rules.Add('X', "F[+X]F[-X] + X");
        rules.Add('F', "FF");
        //Set 2.5
        //rules.Add('X', "F[+X][-X]FX");
        //rules.Add('F', "FF");
        //set 2.5
        //--Set3 Cut doen trees
        //rules.Add('X', "F[+X--][-X]FX");
        //rules.Add('F', "F+FX");
        //EndSet3


        // Apply rules for i interations
        output = input;
        for (int i = 0; i < iterations; i++)
        {
            output = applyRules(output);
        }
        result = output;
        determinePoints(output);
        CreateCylinders();
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

    void determinePoints(string p_input)
    {
        Stack<point> returnValues = new Stack<point>();
        point lastPoint = new point(Vector3.zero, Vector3.zero, 1f);
        returnValues.Push(lastPoint);

        foreach (char c in p_input)
        {
            switch (c)
            {
                case 'F': // Draw line of length lastBranchLength, in direction of lastAngle
                    points.Add(lastPoint);

                    point newPoint = new point(lastPoint.Point + new Vector3(0, lastPoint.BranchLength, 0), lastPoint.Angle, 1f);
                    newPoint.BranchLength = lastPoint.BranchLength - 0.02f;
                    if (newPoint.BranchLength <= 0.0f) newPoint.BranchLength = 0.001f;

                    newPoint.Angle.y = lastPoint.Angle.y + UnityEngine.Random.Range(-30, 30);

                    newPoint.Point = pivot(newPoint.Point, lastPoint.Point, new Vector3(newPoint.Angle.x, 0, 0));
                    newPoint.Point = pivot(newPoint.Point, lastPoint.Point, new Vector3(0, newPoint.Angle.y, 0));

                    points.Add(newPoint);
                    lastPoint = newPoint;
                    break;
                case '+': // Rotate +30
                    lastPoint.Angle.x += 30.0f;
                    break;
                case '[': // Save State
                    returnValues.Push(lastPoint);
                    break;
                case '-': // Rotate -30
                    lastPoint.Angle.x += -30.0f;
                    break;
                case ']': // Load Saved State
                    lastPoint = returnValues.Pop();
                    break;
            }
        }
    }

    void CreateCylinders()
    {
        int randomTreeScale = Random.Range(2, 6);
        for (int i = 0; i < points.Count; i += 2)
        {
            CreateCylinder(points[i], points[i + 1], 0.1f, i, randomTreeScale);
        }

    }

    // Pivot point1 around point2 by angles
    Vector3 pivot(Vector3 point1, Vector3 point2, Vector3 angles)
    {
        Vector3 dir = point1 - point2;
        dir = Quaternion.Euler(angles) * dir;
        point1 = dir + point2;
        return point1;
    }

    void CreateCylinder(point point1, point point2, float radius, int i, int treeScale)
    {
        //UnityEngine.Random.Range(0,3);
        GameObject newCylinder = null;
        Vector3 scale = Vector3.zero;
        float length = Vector3.Distance(point2.Point, point1.Point);


        //Adjusting scale and first 1/3 are logs last 2/3 leafs
        bool isLeaf = false;

        if (i < (points.Count / 6))
        {
            newCylinder = (GameObject)Instantiate(cylinder);

            scale = new Vector3((radius + 0.3f) - (0.01f * i), length / 2.0f, (radius + 0.3f) - (0.01f * i));
        }
        else
        {
            isLeaf = true;
            newCylinder = (GameObject)Instantiate(leafs);
    
            scale = new Vector3(radius * treeScale, length/2, radius * treeScale);
        }


        newCylinder.SetActive(true);

        radius = radius * length;

        newCylinder.transform.localScale = scale;

        newCylinder.transform.position = point1.Point;// + new Vector3(0,transform.position.y,0);
        newCylinder.transform.Rotate(point2.Angle);

        newCylinder.transform.SetParent(this.transform, false);

        branches.Add(newCylinder);
    }

    // Update is called once per frame
    void Update()
    {
        if (debug)
        {
            for (int i = 0; i < points.Count; i += 2)
            {
                Debug.DrawLine(points[i].Point, points[i + 1].Point, Color.black);
            }
        }
    }
}
