using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
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
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        transform.position += new Vector3(x, y, 0) * Time.deltaTime * speed;

        if (Input.GetKeyDown("z"))
        {
            Instantiate(beamPrefab, transform.position, Quaternion.identity);
        }


    }
}
