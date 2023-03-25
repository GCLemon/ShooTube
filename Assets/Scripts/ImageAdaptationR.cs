using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class ImageAdaptationR : MonoBehaviour
{
    //�摜��\��������image object
    [SerializeField] private RawImage enemyRawImage;

    //GetYutubeComment�N���X�ɂ��郆�[�U�̃A�C�R����url���܂���userIconUrlList�擾���邽��
    //getyutube.userIconUrlList��url�̃��X�g�擾
    //getyutube.liveChatMassegeList�Ń��b�Z�[�W�̃��X�g�擾 �@�@�A�C�R��url�ƃ��b�Z�[�W�̃��X�g�ɓ����Ă郆�[�U�[�̏��Ԃ͑Ή����Ă܂�
    [SerializeField]  private EnemyGenerator getyutube;

    private string imageUrl;
    //�摜��url�̐�
    private int urlCount = 0;

    private Texture2D enemytexture;

    void Start()
    {
        getyutube = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();

        enemytexture = new Texture2D(5, 5);

        LoadPngImage();

        StartCoroutine(ChangeIcon());
    }


    IEnumerator ChangeIcon()
    {   
        imageUrl = getyutube.userIconUrl;
        if (imageUrl != "NonePng")
        {
            UnityWebRequest requestImage = UnityWebRequestTexture.GetTexture(imageUrl);

            yield return requestImage.SendWebRequest();

            if (requestImage.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + requestImage.error);
            }
            else
            {
                enemyRawImage.texture = ((DownloadHandlerTexture)requestImage.downloadHandler).texture;
                enemyRawImage.SetNativeSize();
            }
        }
        else
        {
            enemyRawImage.texture = enemytexture;
        }
    }

    private void LoadPngImage()
    {
        byte[] bytes = File.ReadAllBytes("Assets/Textures/enemy.png");
        enemytexture.filterMode = FilterMode.Trilinear;
        enemytexture.LoadImage(bytes);
    }

}
