using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// キューブ各面の状態を格納するクラス
/// </summary>
public class CubeState : MonoBehaviour
{
    //各面のピースの参照
    public List<GameObject> up = new List<GameObject>();
    public List<GameObject> down = new List<GameObject>();
    public List<GameObject> front = new List<GameObject>();
    public List<GameObject> back = new List<GameObject>();
    public List<GameObject> left = new List<GameObject>();
    public List<GameObject> right = new List<GameObject>();

    public static bool autoRotating = false;
    public static bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ピックアップしたい面のすべてのピースを中心ピースの子供オブジェクトとして格納する
    /// </summary>
    /// <param name="cubeSide">ピックアップしたい面</param>
    public void PickUp(List<GameObject> cubeSide)
    {
        foreach (GameObject face in cubeSide)
        {
            //面リストの4番目は中心ピース
            if (face != cubeSide[4])
            {
                //小さい面      ピース                                           中心ピース
                //face         piece                      center face           piece
                face.transform.parent.transform.parent = cubeSide[4].transform.parent;
            }

        }
       
    }


    public void PutDown(List<GameObject> cubepieces,Transform pivot)
    {
        foreach (GameObject cubepiece in cubepieces)
        {
            if (cubepiece != cubepieces[4])
            {
                cubepiece.transform.parent.transform.parent = pivot;
            }
        }
    }


    string GetSideString(List<GameObject> side)
    {
        string sideString = "";

        foreach (GameObject face in side)
        {
            sideString += face.name[0];
        }
        return sideString;
    }


    public string GetStateString()
    {
        string stateString = "";

        stateString += GetSideString(up);
        stateString += GetSideString(right);
        stateString += GetSideString(front);
        stateString += GetSideString(down);
        stateString += GetSideString(left);
        stateString += GetSideString(back);

        return stateString;
    }
}
