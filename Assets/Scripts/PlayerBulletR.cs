using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletR : MonoBehaviour
{
    public float bulletSpeed;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(bulletSpeed,0,0) * Time.deltaTime;

        if(transform.position.x > 11){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Enemy")==true){
            Instantiate(explosion, collision.transform.position, collision.transform.rotation);
        }
        if(collision.CompareTag("Player")==false){
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
