using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;

public class SolveTwoPhase : MonoBehaviour
{
    private ReadCube readCube;
    private CubeState cubeState;

    private bool doOnce = true;


    // Start is called before the first frame update
    void Start()
    {
        cubeState = GetComponent<CubeState>();
        readCube = GetComponent<ReadCube>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CubeState.started && doOnce)
        {
            doOnce = false;
            Solver();
        }
    }


    public void Solver()
    {
        if (CubeState.autoRotating || !CubeState.started)
        {
            return;
        }

        readCube.ReadState();


        //get string state
        string moveString = cubeState.GetStateString();
        print(moveString);
        //solve cube

        string info = "";

        //build lookup table in initial run
        //string solution = SearchRunTime.solution(moveString, out info, buildTables: true);

        //other time
        string solution = Search.solution(moveString, out info);

        //get solve step command string
        List<string> solutionList = StringToList(solution);

        //automate tthe list
        foreach (var command in solutionList)
        {
            Automate.moveQueue.Enqueue(command);
        }

        print(info);
    }


    List<string> StringToList(string solution)
    {
        List<string> solutionList = new List<string>(solution.Split(new string[] { " " }, System.StringSplitOptions.RemoveEmptyEntries));
        return solutionList;
    }

}
