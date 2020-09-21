using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キューブ各面の色情報を取得するクラス
/// </summary>
public class ReadCube : MonoBehaviour
{
    //Ray発射元位置
    public Transform tUp;
    public Transform tDown;
    public Transform tFront;
    public Transform tBack;
    public Transform tLeft;
    public Transform tRight;

    //各面のRay Origin Object
    [HideInInspector]
    public List<GameObject> upRays = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> downRays = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> frontRays = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> backRays = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> leftRays = new List<GameObject>();
    [HideInInspector]
    public List<GameObject> rightRays = new List<GameObject>();


    //ルービックキューブの各面の Layer
    private int layerMask = 1 << 8;

    //3D側で参照できるキューブ状態クラス
    [SerializeField]
    private CubeState cubeState;

    [SerializeField]
    private CubeMap cubeMap;

    //ray発射元ダミーオブジェクト
    public GameObject emptyGO;

    // Start is called before the first frame update
    void Start()
    {
        if (!cubeState)
        {
            cubeState = GetComponent<CubeState>();
        }
        if (!cubeMap)
        {
            cubeMap = FindObjectOfType<CubeMap>();
        }

        SetRayTransform();
        ReadState();

        CubeState.started = true;
    }

    // Update is called once per frame
    void Update()
    {
        //ReadState();
    }

    /// <summary>
    /// 各面の色を取得し、2DUIにセットする
    /// </summary>
    public void ReadState()
    {
        cubeState.up = ReadFace(upRays, tUp);
        cubeState.down = ReadFace(downRays, tDown);
        cubeState.left = ReadFace(leftRays, tLeft);
        cubeState.right = ReadFace(rightRays, tRight);
        cubeState.front = ReadFace(frontRays, tFront);
        cubeState.back = ReadFace(backRays, tBack);

        //2DUIにセットする
        cubeMap.Set(cubeState);
    }

    /// <summary> 
    /// 各面のrayオブジェクトを生成します
    /// </summary>
    void SetRayTransform()
    {
        upRays = BuildRays(tUp, new Vector3(90, 90, 0));
        downRays = BuildRays(tDown, new Vector3(270, 90, 0));
        leftRays = BuildRays(tLeft, new Vector3(0, 180, 0));
        rightRays = BuildRays(tRight, new Vector3(0, 0, 0));
        frontRays = BuildRays(tFront, new Vector3(0, 90, 0));
        backRays = BuildRays(tBack, new Vector3(0, 270, 0));

    }

    /// <summary>
    /// 9個のray発射元を作り、発射元に番号をつけ、2DmapUIの順番と参照する、発射元はキューブと共に回転する、これで面の回転軸を固定される
    /// </summary>
    /// <param name="rayTransform">ray発射元のparent(発射方向を付ける、且つ発射面の位置情報を提供しています)</param>
    /// <param name="direction">発射方向</param>
    /// <returns></returns>
    List<GameObject> BuildRays(Transform rayTransform, Vector3 direction)
    {
        //CountはFaceの並び順を決まる
        int rayCount = 0;
        List<GameObject> rays = new List<GameObject>();
        //並び番号(2DUIと一致):
        //  x-1 x x+1
        //  |0|1|2| y+1
        //  |3|4|5| y
        //  |6|7|8| y-1

        //0から始めたいから y は 1 => -1, x は -1 => 1の順番で処理します
        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                Vector3 startPos = new Vector3(rayTransform.localPosition.x + x,
                                                rayTransform.localPosition.y + y,
                                                rayTransform.localPosition.z);
                GameObject rayStart = Instantiate(emptyGO, startPos, Quaternion.identity, rayTransform);
                //名前を付ける
                rayStart.name = rayCount.ToString();
                rays.Add(rayStart);
                rayCount++;
            }
        }

        //発射方向を付ける
        rayTransform.localRotation = Quaternion.Euler(direction);
        return rays;
    }

    /// <summary>
    /// ray飛ばして、hitされた9個のpieceの面を順番に取得
    /// </summary>
    /// <param name="rayStarts">ray発射元</param>
    /// <param name="rayTransform">ray発射元のparent(発射方向を決める)</param>
    /// <returns></returns>
    public List<GameObject> ReadFace(List<GameObject> rayStarts, Transform rayTransform)
    {
        List<GameObject> hitfaces = new List<GameObject>();


        foreach (GameObject rayStart in rayStarts)
        {
            Vector3 rayOrigin = rayStart.transform.position;
            RaycastHit hit;

            //ray飛ばして、そしてHitされたのは面のcolliderだから、pieceじゃなくて面を確保できる
            if (Physics.Raycast(rayOrigin, rayTransform.forward, out hit, 1000f, layerMask))
            {
#if UNITY_EDITOR
                Debug.DrawRay(rayOrigin, rayTransform.forward * hit.distance, Color.yellow);
                //Debug.Log(hit.collider.gameObject.name);
#endif
                //hitされた面を取得
                //代わりに、hit.collider.gameObject.parentはpieceです
                hitfaces.Add(hit.collider.gameObject);
            }
#if UNITY_EDITOR
            else
            {
                Debug.DrawRay(rayOrigin, rayTransform.forward * 1000f, Color.green);
            }
#endif
        }
        return hitfaces;
    }

}
