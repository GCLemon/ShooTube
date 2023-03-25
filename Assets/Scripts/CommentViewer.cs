using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class CommentViewer : MonoBehaviour
{
    [SerializeField] private CommentItem _CommentItemPrefab;

    [SerializeField] private Texture2D _DefaultTexture;

    private List<CommentItem> _Comments;

    private void Start()
    {
        _Comments = new();
    }

    // コメントの追加処理
    public void AppendComment(string authorName, string authorIconURL, string commentContent)
    {
        // コメントオブジェクトの生成
        CommentItem comment = Instantiate(_CommentItemPrefab);
        _Comments.Add(comment);

        // アイコンの設定
        Image image = comment.AuthorIcon.GetComponent<Image>();
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(authorIconURL);
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += (operation) =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 32, 32), Vector2.zero);
                image.sprite = sprite;
            }
            else
            {
                Sprite sprite = Sprite.Create(_DefaultTexture, new Rect(0, 0, 32, 32), Vector2.zero);
                image.sprite = sprite;
            }
        };

        // ユーザー名の設定
        TextMeshProUGUI name = comment.AuthorName.GetComponent<TextMeshProUGUI>();
        name.text = authorName;

        // コメントの設定
        TextMeshProUGUI content = comment.Content.GetComponent<TextMeshProUGUI>();
        content.text = commentContent;

        // 描画位置の計算
        float height = content.preferredHeight;
        comment.transform.localPosition = new Vector3(0, height + 50.0f);

        // すでに描画されたオブジェクトの更新
        foreach(CommentItem c in _Comments)
        {
            c.transform.localPosition += new Vector3(0, height + 50.0f);
            float itemYPosition = c.transform.localPosition.y;
            float itemHeight = c.Content.GetComponent<TextMeshProUGUI>().preferredHeight;
            float windowHeight = Screen.height;
            if(itemYPosition > itemHeight + windowHeight) { Destroy(c); }
        }
        _Comments = _Comments.Where(item => item != null).ToList();
    }
}
