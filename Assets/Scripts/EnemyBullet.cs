using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 1.5f;
    public Text bulletText;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * -bulletSpeed * Time.deltaTime;

        if(transform.position.x <= -11.5){
            Destroy(gameObject);
        }
    }

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
}
