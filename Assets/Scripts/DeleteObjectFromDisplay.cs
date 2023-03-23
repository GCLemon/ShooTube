using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectFromDisplay : MonoBehaviour
{
    [SerializeField] private bool isDisplayDelete = false;

    //delete�^�O���������̂ƐڐG�������ʂɕ\������Ȃ����Ă�
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "delete" && isDisplayDelete == true)
        {
            this.gameObject.SetActive(false);
        }
    }

}
