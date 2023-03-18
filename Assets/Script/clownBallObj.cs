using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rigidbody2DというコンポーネントをPrefabから消せば重力はなくなります

public class clownBallObj : MonoBehaviour
{
    //生成させるプレファブ
    public GameObject originalBallObj;
    // コピーしたプレファブを入れるリスト
    public List<GameObject> ballObjectList = new List<GameObject>();


    //ポジション
    Vector3 defPosition;
    Vector3 randomPosition;

    //ランダムなポジションの値を入れる変数
    private float randomPositionX;
    private float randomPositionY;

    GameObject parentBallObj;


    void Start()
    {
        //親objの初期ポジション
        defPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //parentBallObjを参照するため
        parentBallObj = GameObject.Find("parentBallObj");

        //生成
        generationBallObj();
    }

    //buttonのOnClickを使って呼んでる
    public void generationBallObj()
    {
        //-2.0から2.0の間でランダムな数値入れてる　X軸
        randomPositionX = Random.Range(-2.0f, 2.0f);
        //Y軸は0固定の値
        randomPositionY = Random.Range(0, 0);

        //親のポジションを基準にランダムに少しずらしてる　ポジション
        randomPosition = new Vector3(transform.position.x + randomPositionX, transform.position.y + randomPositionY, transform.position.z);

        //プレファブの生成
        GameObject generateObjects = GameObject.Instantiate(originalBallObj) as GameObject;

        //生成したプレファブの位置とスケール指定
        generateObjects.transform.SetParent(parentBallObj.transform, false);
        generateObjects.transform.position = randomPosition;
        //generateObjects.transform.localScale = parentBallObj.transform.localScale;

        // リストへ生成したプレファブを追加
        ballObjectList.Add(generateObjects);
    }

}
