using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject enemyBulletPrefab;

    public List<string> liveChatMassegeList = new List<string>();
    public string textString;

    // Start is called before the first frame update
    void Start()
    {
        //コメントを取得するオブジェクトの場所を見つける
        //GetYutubeComment YouTubeComment = GameObject.Find("YutubeApiAttacheObject").GetComponent<GetYutubeComment>;
        
        //2s毎に繰り返しEnemyを生成
        InvokeRepeating("Spawn", 2f, 2f);
    }

    //enemyをランダムに生成
    void Spawn()
    {
        //コメントリストの最初の要素を取り出し
        if(liveChatMassegeList.Count != 0){
            textString = liveChatMassegeList[0];
            liveChatMassegeList.RemoveAt(0);
        }else{
            //コメントがなければ”コメントがありません”を表示
            textString = "コメントがありません";
        }
        //生成位置のy座標はRandom.Range()の中で指定
        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            Random.Range(-2.0f,2.0f),
            transform.position.z
        );
        
        //Enemyを生成
        Instantiate(enemyPrefab,spawnPosition,transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        //コメントリストの更新
        //liveChatMassegeList = YouTubeComment.liveChatMassegeList;
    }
}
