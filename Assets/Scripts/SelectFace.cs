using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 面を
/// </summary>
public class SelectFace : MonoBehaviour
{
    //スクリーン側で参照できるキューブ面状態クラス
    [SerializeField]
    private CubeState cubeState;

    //面情報処理クラス
    [SerializeField]
    private ReadCube readCube;

    private int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        if (!readCube)
        {
            readCube = GetComponent<ReadCube>();
        }
        if (!cubeState)
        {
            cubeState = GetComponent<CubeState>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !CubeState.autoRotating && CubeState.started)
        {
            //今の面状態を取得
            readCube.ReadState();

            //スクリーン座標からrayを飛ばして、hitした面を確認
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
            {
                //faceはpieceオブジェクトの子供オブジェクト
                GameObject face = hit.collider.gameObject;
                //すべての面を一つのリストに格納
                List<List<GameObject>> cubeSides = new List<List<GameObject>>()
                {
                    cubeState.up,
                    cubeState.down,
                    cubeState.left,
                    cubeState.right,
                    cubeState.front,
                    cubeState.back,
                };
                //
                foreach (List<GameObject> cubeSide in cubeSides)
                {
                    //hitされたfaceはどの面に所属を確認
                    if (cubeSide.Contains(face))
                    {
                        //その面を選択する
                        cubeState.PickUp(cubeSide);
                        //中心ピースの回転クラスを回転可能メッセージを渡す
                        cubeSide[4].transform.parent.GetComponent<PivotRotation>().ActiveRotate(cubeSide);
                    }
                }
            }
        }
    }
}
