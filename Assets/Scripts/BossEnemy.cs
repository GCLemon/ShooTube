using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour
{
    public float bossHp;
    public float bossPosition;
    public float bossSpeed;
    public Transform bulletPos;
    public GameObject bulletPrefab;
    public string bulletText;
    public Queue<string> letterQueue = new Queue<string>();
    public Queue<string> letterQueue2 = new Queue<string>();

    GameObject playerShip;
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        playerShip = GameObject.Find("PlayerShip");
        //生成時のコメントを取得し、Queueで1文字ずつ管理
        bulletText = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>().textString;
        for(int i = 0; i < bulletText.Length; i++){
            string s = bulletText.Substring(i, 1);
            letterQueue.Enqueue(s);
        }

        StartCoroutine(CPU());
    }

    void Shot(float angle, float speed)
    {
        if(letterQueue.TryDequeue(out string s)){
            letterQueue2.Enqueue(s);
            bulletPrefab.GetComponent<EnemyBullet>().bulletText.text = s;
            Instantiate(bulletPrefab,
            bulletPos.position,
            Quaternion.Euler(0f,0f,angle));
        }else
        {
            letterQueue = letterQueue2;
            letterQueue2 = new Queue<string>();
        }
    }

    IEnumerator CPU()
    {
        // 特定の位置より上だったら
        while (transform.position.x > bossPosition)
        {
            transform.position += transform.right * -bossSpeed * Time.deltaTime;
            yield return null; //1フレーム(0.02秒)待つ 
        }

        // while(カッコの中がtrueの間繰り返し処理をする)
        while (true)
        {
            yield return WaveNShotM(4, 8);
            yield return new WaitForSeconds(1f);
            yield return WaveNShotMCurve(4, 16);
            yield return new WaitForSeconds(1f);
            yield return WaveNPlayerAimShot(4, 6);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator WaveNShotM(int n, int m)
    {
        // 4回8方向に撃ちたい
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(0.3f);
            ShotN(m, 2);
        }
    }
    IEnumerator WaveNShotMCurve(int n, int m)
    {
        // 4回8方向に撃ちたい
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(0.3f);
            yield return ShotNCurve(m, 2);
        }
    }
    IEnumerator WaveNPlayerAimShot(int n, int m)
    {
        // 4回8方向に撃ちたい
        for (int w = 0; w < n; w++)
        {
            yield return new WaitForSeconds(1f);
            PlayerAimShot(m, 3);
        }
    }

    void ShotN(int count, float speed)
    {
        int bulletCount = count;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (2 * Mathf.PI / bulletCount); // 2PI：360
            Shot(angle, speed);
        }
    }

    IEnumerator ShotNCurve(int count, float speed)
    {
        int bulletCount = count;
        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * (2 * Mathf.PI / bulletCount); // 2PI：360
            Shot(angle - Mathf.PI / 2f, speed);
            Shot(-angle - Mathf.PI / 2f, speed);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Playerを狙う
    // ・Playerの位置取得
    // ・どの角度に撃てばいいのかを計算
    void PlayerAimShot(int count, float speed)
    {
        //この弾幕前にplayerが倒されていたら何もしない
        if(playerShip != null)
        {
            // 自分からみたPlayerの位置を計算する
            Vector3 diffPosition = playerShip.transform.position - transform.position;
            // 自分から見たPlayerの角度を出す：傾きから角度を出す：アークタンジェントを使う
            float angleP = Mathf.Atan2(diffPosition.y, diffPosition.x);

            int bulletCount = count;
            for (int i = 0; i < bulletCount; i++)
            {
                float angle = (i - bulletCount / 2f) * ((Mathf.PI / 2f) / bulletCount); // PI/2f：90


                Shot(angleP + angle, speed);
            }
        }
        
    }

    //****************************
    //Bossの当たり判定
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //playerとBossが接触した時
        if (collision.CompareTag("Player") == true)
        {
            //破壊する時に爆破エフェクト生成（生成したいもの、場所、回転）
            Instantiate(explosion, collision.transform.position, transform.rotation);
            //collisionはぶつかった相手の情報が入っている。
            Destroy(collision.gameObject);

            //gameController.GameOver();
        }
        //BulletとBossが接触した時
        else if (collision.CompareTag("Player") == true)
        {
            //BossのHp
            bossHp--;

            //collisionはぶつかった相手の情報が入っている。この場合は弾
            Destroy(collision.gameObject);

            if(bossHp <= 0)
            {
                //enemyの機体を破壊
                Destroy(gameObject);
                //破壊する時に爆破エフェクト生成（生成したいもの、場所、回転）
                Instantiate(explosion, transform.position, transform.rotation);
            }
            
        }

    }
}
