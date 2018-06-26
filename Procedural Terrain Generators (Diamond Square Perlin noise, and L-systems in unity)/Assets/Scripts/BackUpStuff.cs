using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BackUpStuff : MonoBehaviour
{

    public Dictionary<char, string> rules = new Dictionary<char, string>();
    public int maxIterations;
    public int minIterations;
    public int iterations = 3;
    public string input = "A";
    private string output;
    public string result;

    public GameObject cube;

    private void Awake()
    {
        GenerateTree();
    }


    public void GenerateTree()
    {
        rules.Clear();

        rules.Add('A', "AABAA");
        

        output = input;
        for (int i = 0; i < iterations; i++)
        {
            output = applyRules(output);
        }
        result = output;
        GenerateTerrain(result);
    }
    public void GenerateTerrain(string formula)
    {
       Vector3 LastPos = Vector3.zero;
        Vector3 increment = Vector3.left;
        bool GoUp = false;
        foreach (char c in formula)
        {
            switch (c)
            {
                case 'A':
                    LastPos = LastPos + increment;
                    for (int i = 1; i <= 100; i++)
                    {
                        Instantiate(cube, LastPos+ new Vector3(0,0,i), Quaternion.identity);
                    }
                
                    break;

                case 'B':
                    GoUp = !GoUp;

                    if(GoUp)
                    {
                        LastPos += Vector3.up;
                    }
                    else
                    {
                        LastPos += Vector3.down;
                    }
             
                    break;

                default:
                    break;
            }
        }
      
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
}
