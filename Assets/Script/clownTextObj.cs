using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Rigidbody2D�Ƃ����R���|�[�l���g��Prefab��������Ώd�͂͂Ȃ��Ȃ�܂�

public class clownTextObj : MonoBehaviour
{
    //����������v���t�@�u
    public GameObject originalTextObj;
    // �R�s�[�����v���t�@�u�����郊�X�g
    public List<GameObject> textObjectList = new List<GameObject>();

    
    //�|�W�V����
    Vector3 defPosition;
    Vector3 randomPosition;

    //�����_���ȃ|�W�V�����̒l������ϐ�
    private float randomPositionX;
    private float randomPositionY;

    GameObject parentTextObj;


    void Start()
    {
        //�eobj�̏����|�W�V����
        defPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        //parentTextObj���Q�Ƃ��邽��
        parentTextObj = GameObject.Find("parentTextObj");

        //����
        generationTextObj();
    }

    //button��OnClick���g���ČĂ�ł�
    public void generationTextObj()
    {
        //-2.0����2.0�̊ԂŃ����_���Ȑ��l����Ă�@X��
        randomPositionX = Random.Range(-2.0f, 2.0f);
        //Y����0�Œ�̒l
        randomPositionY = Random.Range(0, 0);

        //�e�̃|�W�V��������Ƀ����_���ɏ������炵�Ă�@�|�W�V����
        randomPosition = new Vector3(transform.position.x + randomPositionX, transform.position.y + randomPositionY, transform.position.z);

        //�v���t�@�u�̐���
        GameObject generateObjects = GameObject.Instantiate(originalTextObj) as GameObject;

        //���������v���t�@�u�̈ʒu�ƃX�P�[���w��
        generateObjects.transform.SetParent(parentTextObj.transform, false);
        generateObjects.transform.position = randomPosition;
        generateObjects.transform.localScale = new Vector2(1, 1);

        // ���X�g�֐��������v���t�@�u��ǉ�
        textObjectList.Add(generateObjects);  

        //���������v���t�@�u��text�̕�����ς��邽�ߎQ��
        Text myText = textObjectList[textObjectList.Count - 1].GetComponent<Text>();
        myText.text = Random.Range(1,10).ToString();
    }


}
