using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// キューブの各面の状態を取得し、UIに映るシステムの制御クラス
/// </summary>
public class CubeMap : MonoBehaviour
{
    //UIから(2D側)参照できるキューブ面状態
    private CubeState cubeState;

    //各面を表すUIの位置Parent
    public Transform up;
    public Transform down;
    public Transform left;
    public Transform right;
    public Transform front;
    public Transform back;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 取得された面の情報に応じて、UIに色を更新する
    /// </summary>
    /// <param name="state">面の情報</param>
    public void Set(CubeState state)
    {
        cubeState = state;
        UpdateMap(cubeState.front, front);
        UpdateMap(cubeState.back, back);
        UpdateMap(cubeState.left, left);
        UpdateMap(cubeState.right, right);
        UpdateMap(cubeState.up, up);
        UpdateMap(cubeState.down, down);
    }

    /// <summary>
    /// 3Dオブジェクトと2Dオブジェクトの色情報を統一させる
    /// </summary>
    /// <param name="face3d">3D面オブジェクト</param>
    /// <param name="side2D">UISideオブジェクト</param>
    void UpdateMap(List<GameObject> face3d, Transform side2D)
    {

        //2重ロープ避ける
        for (int i = 0; i < side2D.childCount; i++)
        {
            if (i >= face3d.Count)
            {
                break;
            }
            Transform piece2D = side2D.GetChild(i);

            //面の名前は正面(Front)の場合
            if (face3d[i].name[0] == 'F')
            {
                piece2D.GetComponent<Image>().color = Color.green;
            }
            //反面(Back)の場合
            if (face3d[i].name[0] == 'B')
            {
                piece2D.GetComponent<Image>().color = Color.blue;
            }
            //左(Left)の場合
            if (face3d[i].name[0] == 'L')
            {
                piece2D.GetComponent<Image>().color = new Color(1, 0.5f, 0, 1);//Orange
            }
            //右(Right)の場合
            if (face3d[i].name[0] == 'R')
            {
                piece2D.GetComponent<Image>().color = Color.red;
            }
            //上(Up)の場合
            if (face3d[i].name[0] == 'U')
            {
                piece2D.GetComponent<Image>().color = Color.white;
            }
            //下(Down)の場合
            if (face3d[i].name[0] == 'D')
            {
                piece2D.GetComponent<Image>().color = Color.yellow;
            }
        }
    }

}
