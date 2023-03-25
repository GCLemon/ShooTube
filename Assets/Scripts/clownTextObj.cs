using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Rigidbody2DというコンポーネントをPrefabから消せば重力はなくなります

public class clownTextObj : MonoBehaviour
{
    //生成させるプレファブ
    public GameObject originalTextObj;
    // コピーしたプレファブを入れるリスト
    public List<GameObject> textObjectList = new List<GameObject>();

    
    //ポジション
    Vector3 defPosition;
    Vector3 randomPosition;

    //ランダムなポジションの値を入れる変数
    private float randomPositionX;
    private float randomPositionY;

    GameObject parentTextObj;


    void Start()
    {
        //親objの初期ポジション
        defPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //parentTextObjを参照するため
        parentTextObj = GameObject.Find("parentTextObj");

        //生成
        generationTextObj();
    }

    //buttonのOnClickを使って呼んでる
    public void generationTextObj()
    {
        //-2.0から2.0の間でランダムな数値入れてる　X軸
        randomPositionX = Random.Range(-2.0f, 2.0f);
        //Y軸は0固定の値
        randomPositionY = Random.Range(0, 0);

        //親のポジションを基準にランダムに少しずらしてる　ポジション
        randomPosition = new Vector3(transform.position.x + randomPositionX, transform.position.y + randomPositionY, transform.position.z);

        //プレファブの生成
        GameObject generateObjects = GameObject.Instantiate(originalTextObj) as GameObject;

        //生成したプレファブの位置とスケール指定
        generateObjects.transform.SetParent(parentTextObj.transform, false);
        generateObjects.transform.position = randomPosition;
        generateObjects.transform.localScale = new Vector2(1, 1);

        // リストへ生成したプレファブを追加
        textObjectList.Add(generateObjects);  

        //生成したプレファブのtextの文字を変えるため参照
        Text myText = textObjectList[textObjectList.Count - 1].GetComponent<Text>();
        myText.text = Random.Range(1,10).ToString();
    }


}
