using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

 public class PlayerControllerR : MonoBehaviour
 {
     public float speed;
     public Transform bulletPos;
     public GameObject beamPrefab;

     // Start is called before the first frame update
     void Start()
     {

     }

     private void OnTriggerEnter2D(Collider2D col)
     {
         // 敵だったら
         if (col.gameObject.tag == "Enemy")
         {
             // ぶつかった相手を破壊
             Destroy(col.gameObject);

             // 弾を破壊
             Destroy(gameObject);
         }
     }

     // Update is called once per frame
     void Update()
     {
        Move();
        Shot();
     }

     void Move()
    {
        //横軸の値を返す
        float x = Input.GetAxisRaw("Horizontal");

        //縦軸の値を返す
        float y = Input.GetAxisRaw("Vertical");

        //移動制御＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊＊今回追加
        Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * 4f;
        //移動できる範囲をMathf.Clampで範囲指定して制御
        nextPosition = new Vector3(
            Mathf.Clamp(nextPosition.x,-10f,10f),
            Mathf.Clamp(nextPosition.y, -4.5f, 4.5f),
            nextPosition.z
            );
        //現在位置にnextPositionを＋
        transform.position = nextPosition;
    }

    void Shot()
    {
        if (Input.GetKeyDown("z"))
        {
            Instantiate(beamPrefab, bulletPos.position, bulletPos.rotation);
        }
    }
 }