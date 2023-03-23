using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectFromDisplay : MonoBehaviour
{
    [SerializeField] private bool isDisplayDelete = false;

    //deleteƒ^ƒO‚ª‚Â‚¢‚½•¨‘Ì‚ÆÚG‚µ‚½‚ç‰æ–Ê‚É•\¦‚³‚ê‚È‚­‚µ‚Ä‚é
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "delete" && isDisplayDelete == true)
        {
            this.gameObject.SetActive(false);
        }
    }

}
