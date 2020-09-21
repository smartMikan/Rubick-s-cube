using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Automate : MonoBehaviour
{
    public static Queue<string> moveQueue = new Queue<string>();

    private readonly List<string> allMoveCommands = new List<string>()
    {
        "U","D","F","B","L","R",
        "U'","D'","F'","B'","L'","R'",
        "U2","D2","F2","B2","L2","R2",
    };

    private CubeState cubeState;
    private ReadCube readCube;

    // Start is called before the first frame update
    void Start()
    {
        cubeState = GetComponent<CubeState>();
        readCube = GetComponent<ReadCube>();


        //moveList.Enqueue("U");

    }

    // Update is called once per frame
    void Update()
    {
        if (moveQueue.Count > 0 && !CubeState.autoRotating && CubeState.started)
        {
            DoMove(moveQueue.Peek());
            moveQueue.Dequeue();
            Debug.Log(moveQueue);
        }

    }

    public void Shuffle()
    {
        if (moveQueue.Count <= 0)
        {
            int shuffleLength = Random.Range(10, 43);

            for (int i = 0; i < shuffleLength; i++)
            {
                moveQueue.Enqueue(allMoveCommands[Random.Range(0, 18)]);
            }
        }
        
    }


    void DoMove(string moveSymbol)
    {
        readCube.ReadState();
        CubeState.autoRotating = true;
        if (moveSymbol == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        if (moveSymbol == "U'")
        {
            RotateSide(cubeState.up, 90);
        }
        if (moveSymbol == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        if (moveSymbol == "D")
        {
            RotateSide(cubeState.down, -90);
        }
        if (moveSymbol == "D'")
        {
            RotateSide(cubeState.down, 90);
        }
        if (moveSymbol == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        if (moveSymbol == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        if (moveSymbol == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        if (moveSymbol == "L2")
        {
            RotateSide(cubeState.left, -180);
        }

        if (moveSymbol == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        if (moveSymbol == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        if (moveSymbol == "R2")
        {
            RotateSide(cubeState.right, -180);
        }

        if (moveSymbol == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        if (moveSymbol == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        if (moveSymbol == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        if (moveSymbol == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        if (moveSymbol == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        if (moveSymbol == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
    }

    void RotateSide(List<GameObject> side, float angle)
    {
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();

        pr.StartAutoRotate(side, angle);
    }
}
