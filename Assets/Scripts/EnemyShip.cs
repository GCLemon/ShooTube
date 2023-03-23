using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーと球(プレイヤー)にColliderをつける

public class EnemyShip : MonoBehaviour
{
    public Transform bulletPos;
    public GameObject enemyBulletPrefab;
    public GameObject explosion;
    public string textString;
    public Queue<string> letterQueue = new Queue<string>();
    public Queue<string> letterQueue2 = new Queue<string>();

    float offset;

    void Start()
    {
        //生成時のコメントを取得し、Queueで1文字ずつ管理
        textString = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>().textString;
        for(int i = 0; i < textString.Length; i++){
            string s = textString.Substring(i, 1);
            letterQueue.Enqueue(s);
        }

        offset = Random.Range(0, 2f * Mathf.PI);

        //1s待ってから0.5s間隔で弾を打つ
        InvokeRepeating("Shooting", 1f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //ゆらゆら移動する
        transform.position -= new Vector3(
            Time.deltaTime,
            Mathf.Cos(Time.frameCount * 0.005f + offset) * 0.002f,
            0
            );
    }

    //Colliderをつけ、全てのisTrigerにチェックをつける
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")==true){
            Instantiate(explosion, collision.transform.position, collision.transform.rotation);
        }
        if(collision.CompareTag("Enemy")==false){
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    private void Shooting(){
        //Queueに要素があれば1文字ずつ発射、なければ打つ前のQueueを打った後のQueueに更新
        if(letterQueue.TryDequeue(out string s)){
            letterQueue2.Enqueue(s);
            enemyBulletPrefab.GetComponent<EnemyBullet>().bulletText.text = s;
            Instantiate(enemyBulletPrefab,
            bulletPos.position,
            Quaternion.Euler(0f,0f,0f));
        }else
        {
            letterQueue = letterQueue2;
            letterQueue2 = new Queue<string>();
        }
    }
}
