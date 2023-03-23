using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rigidbody2D�Ƃ����R���|�[�l���g��Prefab��������Ώd�͂͂Ȃ��Ȃ�܂�

public class clownBallObj : MonoBehaviour
{
    //����������v���t�@�u
    public GameObject originalBallObj;
    // �R�s�[�����v���t�@�u�����郊�X�g
    public List<GameObject> ballObjectList = new List<GameObject>();


    //�|�W�V����
    Vector3 defPosition;
    Vector3 randomPosition;

    //�����_���ȃ|�W�V�����̒l������ϐ�
    private float randomPositionX;
    private float randomPositionY;

    GameObject parentBallObj;


    void Start()
    {
        //�eobj�̏����|�W�V����
        defPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //parentBallObj���Q�Ƃ��邽��
        parentBallObj = GameObject.Find("parentBallObj");

        //����
        generationBallObj();
    }

    //button��OnClick���g���ČĂ�ł�
    public void generationBallObj()
    {
        //-2.0����2.0�̊ԂŃ����_���Ȑ��l����Ă�@X��
        randomPositionX = Random.Range(-2.0f, 2.0f);
        //Y����0�Œ�̒l
        randomPositionY = Random.Range(0, 0);

        //�e�̃|�W�V��������Ƀ����_���ɏ������炵�Ă�@�|�W�V����
        randomPosition = new Vector3(transform.position.x + randomPositionX, transform.position.y + randomPositionY, transform.position.z);

        //�v���t�@�u�̐���
        GameObject generateObjects = GameObject.Instantiate(originalBallObj) as GameObject;

        //���������v���t�@�u�̈ʒu�ƃX�P�[���w��
        generateObjects.transform.SetParent(parentBallObj.transform, false);
        generateObjects.transform.position = randomPosition;
        //generateObjects.transform.localScale = parentBallObj.transform.localScale;

        // ���X�g�֐��������v���t�@�u��ǉ�
        ballObjectList.Add(generateObjects);
    }

}
