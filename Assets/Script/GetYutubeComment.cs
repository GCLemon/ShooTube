using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class VideoResponse
{
    public VideoItems[] items;
}

[System.Serializable]
public class VideoItems
{
    public LiveStreamingDetails liveStreamingDetails;
}

[System.Serializable]
public class LiveStreamingDetails
{
     public string activeLiveChatId;
}




[System.Serializable]
public class LiveChatResponse
{
    public LiveChatItems[] items;
}

[System.Serializable]
public class LiveChatItems
{
    public Snippet snippet;
    public AuthorDetails authorDetails;
}

[System.Serializable]
public class Snippet
{
    public string displayMessage;
    public string publishedAt;
}

[System.Serializable]
public class AuthorDetails
{
    public string profileImageUrl;
}



public class GetYutubeComment : MonoBehaviour
{
    [SerializeField] private string apiKey;

    [SerializeField] private string liveStreamId;


    [SerializeField] private bool isGetComment=false;

    //ライブコメントから取得したコメントリスト
    public Queue<string> liveChatMassegeQueue = new Queue<string>();
    //ユーザーのアイコン画像のURL　liveChatMassegeQueue　と　userIconUrlQueue　のユーザー情報は対応してます
    public Queue<string> userIconUrlQueue = new Queue<string>();


    string liveChatId;
    DateTime lastCommentTime = DateTime.Parse("2023 - 03 - 17T04:16:22.484251+00:00");

    void Start()
    {
        if (isGetComment == true)
        {
            StartCoroutine(GetLiveChatId());
            Debug.Log("yutubeAPI使用中です");
        }
    }

    IEnumerator GetLiveChatId()
    {
        string url = "https://www.googleapis.com/youtube/v3/videos" +
            "?id=" + liveStreamId +
            "&part=liveStreamingDetails" +
            "&key=" + apiKey;

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error: " + www.error);
            }
            else
            {
                VideoResponse videoResponse = JsonUtility.FromJson<VideoResponse>(www.downloadHandler.text);
                liveChatId = videoResponse.items[0].liveStreamingDetails.activeLiveChatId.ToString();
                liveChatId = liveChatId.Replace("\"", "");
                yield return GetLiveChatComment();
            }
        }
    }

    IEnumerator GetLiveChatComment()
    {
        string url = "https://www.googleapis.com/youtube/v3/liveChat/messages?" +
            "liveChatId=" + liveChatId + "&" +
            "part=snippet,authorDetails" + "&" +
            "key=" + apiKey;


        UnityWebRequest LiveChatData = UnityWebRequest.Get(url);
        yield return LiveChatData.SendWebRequest();

        switch (LiveChatData.result)
        {
            case UnityWebRequest.Result.InProgress:
                Debug.Log("リクエスト中");
                break;
            default:
                Debug.Log("erro");
                break;
        }

        LiveChatResponse liveChatResponse = JsonUtility.FromJson<LiveChatResponse>(LiveChatData.downloadHandler.text);

        for (int i = 0; i < liveChatResponse.items.Length; i++)
        {

            if(lastCommentTime< DateTime.Parse(liveChatResponse.items[i].snippet.publishedAt))
            {
                liveChatMassegeQueue.Enqueue(liveChatResponse.items[i].snippet.displayMessage);
                userIconUrlQueue.Enqueue(liveChatResponse.items[i].authorDetails.profileImageUrl);
            }

            if (i== liveChatResponse.items.Length - 1)
            {
                lastCommentTime = DateTime.Parse(liveChatResponse.items[i].snippet.publishedAt);
            }
        }

        yield return new WaitForSeconds(10.0f);
        yield return GetLiveChatComment();
    }

}