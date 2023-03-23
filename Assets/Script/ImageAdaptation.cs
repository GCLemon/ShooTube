using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class ImageAdaptation : MonoBehaviour
{
    //�摜��\��������image object
    [SerializeField] private RawImage enemyRawImage;

    //GetYutubeComment�N���X�ɂ��郆�[�U�̃A�C�R����url���܂���userIconUrlList�擾���邽��
    //getyutube.userIconUrlList��url�̃��X�g�擾
    //getyutube.liveChatMassegeList�Ń��b�Z�[�W�̃��X�g�擾 �@�@�A�C�R��url�ƃ��b�Z�[�W�̃��X�g�ɓ����Ă郆�[�U�[�̏��Ԃ͑Ή����Ă܂�
    [SerializeField]  private GetYutubeComment getyutube;

    private string imageUrl;
    //�摜��url�̐�
    private int urlCount = 0;

    private Texture2D enemytexture;

    void Start()
    {
        enemytexture = new Texture2D(200, 200);

        //�R�����g���Ȃ��Ƃ��̓G��png�摜�ǂݍ���
        StartCoroutine(LoadPngImage());

        //10�b�����ɍŐV�̃R�����g�̃A�C�R���摜�ɕύX
        StartCoroutine(changeIcon());
    }


    IEnumerator changeIcon()
    {   
        urlCount = getyutube.userIconUrlList.Count;
        //�R�����g�擾�ł��Ă�����ȉ����s
        if (urlCount > 0)
        {
            //��ԐV�����摜��URL�擾
            imageUrl = getyutube.userIconUrlList[urlCount - 1];

            //�摜�����N�G�X�g
            UnityWebRequest requestImage = UnityWebRequestTexture.GetTexture(imageUrl);

            //�摜���擾�ł���܂ő҂�
            yield return requestImage.SendWebRequest();


            if (requestImage.result != UnityWebRequest.Result.Success)
            {
                //�G���[
                Debug.Log("Error: " + requestImage.error);
            }
            else
            {
                //�擾�����摜�̃e�N�X�`����RawImage�̃e�N�X�`���ɒ���t����
                enemyRawImage.texture = ((DownloadHandlerTexture)requestImage.downloadHandler).texture;
                enemyRawImage.SetNativeSize();

            }
        }
        else
        {
            //�R�����g���Ȃ��Ƃ���png�摜�K�� ���u���Ȃ̂ŕς��Ă�����đ��v�ł�
            enemyRawImage.texture = enemytexture;
        }

        //10�b�҂�
        yield return new WaitForSeconds(10.0f);
        //�ēx���s
        yield return changeIcon();

    }

    IEnumerator LoadPngImage()
    {
        byte[] bytes = File.ReadAllBytes("Assets/Textures/enemy.png");
        enemytexture.filterMode = FilterMode.Trilinear;
        enemytexture.LoadImage(bytes);
        yield return null;
    }

}
