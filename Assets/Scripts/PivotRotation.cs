using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 各面の中心ピースに付ける面回転制御クラス
/// </summary>
public class PivotRotation : MonoBehaviour
{
    enum RotateState
    {
        None,
        Dragging,               //ユーザー入力による回転中
        AutoAdjusting,          //回転ステップを90°に固定するため、自動回転中
    }
    private RotateState rotateState = RotateState.None;

    //9個のピースを持ち面オブジェクト
    private List<GameObject> activeSide;
    private Vector3 localForward;
    private Vector3 mouseRef;



    //回転感度
    private float sensitivity = 0.4f;
    //回転角度(x,y,z軸)(フレイムステップ)
    private Vector3 rotation;

    //回転ステップを90°固定する場合の目標回転四元数
    private Quaternion targetQuaternion;
    //自動回転速度
    private float speed = 300f;


    //キューブ状態処理クラス
    private ReadCube readCube;
    //所属面状態クラスの参照
    private CubeState cubeState;




    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
    }

    // Late Update is called once per frame after other Update before prerendering
    void LateUpdate()
    {
        switch (rotateState)
        {
            case RotateState.None:
                break;
            case RotateState.Dragging:
                if (!CubeState.autoRotating && CubeState.started)
                {
                    //回転
                    SpinSide(activeSide);
                    if (Input.GetMouseButtonUp(0))
                    {
                        //Auto回転状態に切り替わる
                        rotateState = RotateState.AutoAdjusting;
                        CalculateStepAngle();
                    }

                }
                break;
            case RotateState.AutoAdjusting:
                AutoRotate();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 面を回転させる
    /// </summary>
    /// <param name="side">回転面</param>
    private void SpinSide(List<GameObject> side)
    {
        //reset rotaion
        rotation = Vector3.zero;

        //ターゲット中心ピースのスクリーン座標を取得
        var PivotScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, activeSide[4].transform.position);

        //マウス座標変位
        Vector3 mouseOffset = (Input.mousePosition - mouseRef);

        //ターゲット中心スクリーン座標からマウス座標までの変位
        Vector2 PivotOffset = new Vector2(Input.mousePosition.x - PivotScreenPoint.x, Input.mousePosition.y - PivotScreenPoint.y);
        PivotOffset.Normalize();
        //回転方向multiplier
        float clockdirY = Vector2.Dot(Vector2.right, PivotOffset); //cosθ
        float clockdirX = -Vector2.Dot(Vector2.up, PivotOffset); //sinθ

        if (side == cubeState.up)
        {
            rotation.y = (mouseOffset.x * clockdirX + mouseOffset.y * clockdirY) * -sensitivity;
        }
        if (side == cubeState.down)
        {
            rotation.y = (mouseOffset.x * clockdirX + mouseOffset.y * clockdirY) * sensitivity;
        }
        if (side == cubeState.left)
        {
            rotation.z = (mouseOffset.x * clockdirX + mouseOffset.y * clockdirY) * -sensitivity;
        }
        if (side == cubeState.right)
        {
            rotation.z = (mouseOffset.x * clockdirX + mouseOffset.y * clockdirY) * sensitivity;
        }
        if (side == cubeState.front)
        {
            rotation.x = (mouseOffset.x * clockdirX + mouseOffset.y * clockdirY) * sensitivity;
        }
        if (side == cubeState.back)
        {
            rotation.x = (mouseOffset.x * clockdirX + mouseOffset.y * clockdirY) * -sensitivity;
        }

        //rotate this piece
        transform.Rotate(rotation, Space.Self);

        mouseRef = Input.mousePosition;
    }

    public void ActiveRotate(List<GameObject> side)
    {
        activeSide = side;
        mouseRef = Input.mousePosition;

        rotateState = RotateState.Dragging;

        //create rotate vector
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;

    }





    /// <summary>
    /// 回転ステップを計算する
    /// </summary>
    public void CalculateStepAngle()
    {
        Vector3 vec = transform.localEulerAngles;
        vec.x = Mathf.Round(vec.x / 90) * 90;
        vec.y = Mathf.Round(vec.y / 90) * 90;
        vec.z = Mathf.Round(vec.z / 90) * 90;

        targetQuaternion.eulerAngles = vec;
    }


    public void StartAutoRotate(List<GameObject> side, float angle)
    {
        cubeState.PickUp(side);
        localForward = Vector3.zero - side[4].transform.parent.transform.localPosition;

        targetQuaternion = Quaternion.AngleAxis(angle, localForward) * transform.localRotation;

        activeSide = side;
        rotateState = RotateState.AutoAdjusting;

    }




    private void AutoRotate()
    {
        var step = speed * Time.deltaTime;

        transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetQuaternion, step);

        if (Quaternion.Angle(transform.localRotation, targetQuaternion) <= 1)
        {
            transform.localRotation = targetQuaternion;
            readCube.ReadState();


            cubeState.PutDown(activeSide, transform.parent);


            CubeState.autoRotating = false;
            rotateState = RotateState.None;
        }

    }

}
