using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キューブ全体の回転の制御クラス
/// </summary>
public class RotateWholeCube : MonoBehaviour
{
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    Vector3 previousMousePos;
    Vector3 mouseDelta;

    //回転中心オブジェクト
    public GameObject cubeCenter;

    [SerializeField]
    float speed = 200f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Swipe();

        Drag();


    }

    void Drag()
    {

        if (Input.GetMouseButton(1))
        {

            //自分を少しだけ回転して、移動先の方向を示す
            mouseDelta = Input.mousePosition - previousMousePos;

            //移動量
            mouseDelta *= 0.1f;
            //Y軸回転
            Quaternion YQua = Quaternion.Euler(0, -mouseDelta.x, 0);

            //X or Z軸回転
            float length = mouseDelta.magnitude;
            //左上or右下ならX軸回転
            float Xmultiplier = mouseDelta.x * mouseDelta.y < 0 ? 1 : 0;
            //右上or左下ならZ軸回転
            float Zmultiplier = mouseDelta.x * mouseDelta.y > 0 ? 1 : 0;
            //回転方向
            Xmultiplier *= mouseDelta.x > 0 ? -1 : 1;
            Zmultiplier *= mouseDelta.x > 0 ? -1 : 1;

            Quaternion XZQua = Quaternion.Euler(Xmultiplier * length, 0, Zmultiplier * length);

            //30°以下なら左右回転
            float tanMousDelta =Mathf.Abs(mouseDelta.y) / Mathf.Abs(mouseDelta.x);
            if (tanMousDelta < 0.577f)
            {
                transform.rotation = YQua * transform.rotation;
            }
            //60°以上なら回転しない
            else if (tanMousDelta > 1.732f)
            {
                //
            }
            //30°から60°まで上下回転
            else
            {
                transform.rotation = XZQua * transform.rotation;
            }

        }
        else
        {
            //ターゲットまで回転させる
            if (transform.rotation != cubeCenter.transform.rotation)
            {
                var step = speed * Time.deltaTime;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, cubeCenter.transform.rotation, step);
            }

        }

        previousMousePos = Input.mousePosition;
    }



    void Swipe()
    {
        if (Input.GetMouseButtonDown(1))
        {
            //今のマウス座標を取る
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(1))
        {
            //次のマウス座標を取る
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //移動方向を計算する
            currentSwipe = secondPressPos - firstPressPos;
            currentSwipe.Normalize();


            //回転ターゲットを設定する
            if (LeftSwipe(currentSwipe))
            {
                cubeCenter.transform.Rotate(0, 90, 0, Space.World);
            }
            else if (RightSwipe(currentSwipe))
            {
                cubeCenter.transform.Rotate(0, -90, 0, Space.World);
            }

            else if (UpLeftSwipe(currentSwipe))
            {
                cubeCenter.transform.Rotate(90, 0, 0, Space.World);
            }
            else if (DownLeftSwipe(currentSwipe))
            {
                cubeCenter.transform.Rotate(0, 0, 90, Space.World);
            }
            else if (UpRightSwipe(currentSwipe))
            {
                cubeCenter.transform.Rotate(0, 0, -90, Space.World);
            }
            else if (DownRightSwipe(currentSwipe))
            {
                cubeCenter.transform.Rotate(-90, 0, 0, Space.World);
            }


        }
    }

    bool LeftSwipe(Vector2 swipe)
    {
        return swipe.x < 0 && swipe.y > -0.5f && swipe.y < 0.5f;
    }

    bool RightSwipe(Vector2 swipe)
    {
        return swipe.x > 0 && swipe.y > -0.5f && swipe.y < 0.5f;
    }


    bool UpLeftSwipe(Vector2 swipe)
    {
        return swipe.y > 0 && swipe.x < 0;
    }
    bool DownLeftSwipe(Vector2 swipe)
    {
        return swipe.y < 0 && swipe.x < 0;
    }

    bool UpRightSwipe(Vector2 swipe)
    {
        return swipe.y > 0 && swipe.x > 0;
    }

    bool DownRightSwipe(Vector2 swipe)
    {
        return swipe.y < 0 && swipe.x > 0;
    }
}
