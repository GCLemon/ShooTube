using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public float spawnTime;

    public GameObject enemyPrefab;
    public GameObject subBossPrefab;
    public GameObject bossPrefab;
    public GetYutubeCommentR youTubeComment;
    public Queue<string> liveChatMassegeQueue = new Queue<string>();
    public Queue<string> userIconUrlQueue = new Queue<string>();
    public List<string> liveChatMassegeLog = new List<string>();
    public List<string> userIconUrlLog = new List<string>();
    public string textString;
    public string userIconUrl;

    // Start is called before the first frame update
    void Start()
    {
        //コメントを取得するオブジェクトの場所を見つける
        youTubeComment = GameObject.Find("YutubeApiAttacheObject").GetComponent<GetYutubeCommentR>();
        
        //2s毎に繰り返しEnemyを生成
        InvokeRepeating("Spawn", 2f, spawnTime);
    }

    //enemyをランダムに生成
    void Spawn()
    {
        float num = UnityEngine.Random.Range(0.0f,1.0f);

        //コメントリストの最初の要素を取り出し
        if(liveChatMassegeQueue.TryDequeue(out textString)){
            liveChatMassegeLog.Add(textString);
            userIconUrl = userIconUrlQueue.Dequeue();
            userIconUrlLog.Add(userIconUrl);
        }else{
            int listCount = liveChatMassegeLog.Count;
            if(listCount > 0){
                System.Random random = new System.Random();
                int rnd = random.Next(listCount);
                textString = liveChatMassegeLog[rnd];
                userIconUrl = userIconUrlLog[rnd];
            }else{
                textString = "コメントがありません";
                userIconUrl = "NonePng";
            }
        }
        //生成位置のy座標はRandom.Range()の中で指定
        Vector3 spawnPosition = new Vector3(
            transform.position.x,
            UnityEngine.Random.Range(-4.0f,4.0f),
            transform.position.z
        );
        //Enemyを生成
        if(num >= 0.95f){
            Instantiate(bossPrefab,spawnPosition,transform.rotation);
        }else if(num >= 0.65f){
            Instantiate(subBossPrefab,spawnPosition,transform.rotation);
        }else{
            Instantiate(enemyPrefab,spawnPosition,transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //コメントリストの更新
        liveChatMassegeQueue = youTubeComment.liveChatMassegeQueue;
        userIconUrlQueue = youTubeComment.userIconUrlQueue;
    }
}
