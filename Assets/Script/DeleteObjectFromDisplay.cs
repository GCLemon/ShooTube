using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectFromDisplay : MonoBehaviour
{
    [SerializeField] private bool isDisplayDelete = false;

    //deleteタグがついた物体と接触したら画面に表示されなくしてる
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "delete" && isDisplayDelete == true)
        {
            this.gameObject.SetActive(false);
        }
    }

}
