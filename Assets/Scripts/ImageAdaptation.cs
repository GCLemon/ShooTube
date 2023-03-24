using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;

public class ImageAdaptation : MonoBehaviour
{
    //画像を表示させるimage object
    [SerializeField] private RawImage enemyRawImage;

    //GetYutubeCommentクラスにあるユーザのアイコンのurlがつまったuserIconUrlQueue取得するため
    //getyutube.userIconUrlQueueでurlのリスト取得
    //getyutube.liveChatMassegeQueueでメッセージのリスト取得 　　アイコンurlとメッセージのリストに入ってるユーザーの順番は対応してます
    [SerializeField]  private GetYutubeComment getyutube;

    private string imageUrl;
    //画像のurlの数
    private int urlCount = 0;

    private Texture2D enemytexture;

    void Start()
    {
        enemytexture = new Texture2D(200, 200);

        //コメントがないときの敵のpng画像読み込み
        StartCoroutine(LoadPngImage());

        //10秒おきに最新のコメントのアイコン画像に変更
        StartCoroutine(changeIcon());
    }


    IEnumerator changeIcon()
    {   
        urlCount = getyutube.userIconUrlQueue.Count;
        //コメント取得できていたら以下実行
        if (urlCount > 0)
        {
            //要素を削除しないで取り出して一番初めの要素にある画像のURL取得
            imageUrl = getyutube.userIconUrlQueue.Peek();

            //画像をリクエスト
            UnityWebRequest requestImage = UnityWebRequestTexture.GetTexture(imageUrl);

            //画像を取得できるまで待つ
            yield return requestImage.SendWebRequest();


            if (requestImage.result != UnityWebRequest.Result.Success)
            {
                //エラー
                Debug.Log("Error: " + requestImage.error);
            }
            else
            {
                //取得した画像のテクスチャをRawImageのテクスチャに張り付ける
                enemyRawImage.texture = ((DownloadHandlerTexture)requestImage.downloadHandler).texture;
                enemyRawImage.SetNativeSize();

            }
        }
        else
        {
            //コメントがないときのpng画像適応 仮置きなので変えてもらって大丈夫です
            enemyRawImage.texture = enemytexture;
        }

        //10秒待つ
        yield return new WaitForSeconds(10.0f);
        //再度実行
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
