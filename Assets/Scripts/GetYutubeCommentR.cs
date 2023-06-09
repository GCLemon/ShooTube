using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class VideoResponse{
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
    public string displayName;
}



public class GetYutubeCommentR : MonoBehaviour
{
    [SerializeField] private string apiKey;

    [SerializeField] private string liveStreamId;


    [SerializeField] private bool isGetComment=false;

    //���C�u�R�����g����擾�����R�����g���X�g
    public Queue<string> liveChatMassegeQueue = new Queue<string>();
    //���[�U�[�̃A�C�R���摜��URL�@liveChatMassegeList�@�Ɓ@userIconUrlList�@�̃��[�U�[���͑Ή����Ă܂�
    public Queue<string> userIconUrlQueue = new Queue<string>();
    public Queue<string> userNameQueue = new  Queue<string>();

    string liveChatId;
    TimeZoneInfo timeZoneJst = TimeZoneInfo.Local;
    DateTime lastCommentTime ;

    public event Action<string,string,string> RecieveEvent
    {
        add => _RecieveEvent += value;
        remove => _RecieveEvent -= value;
    }
    private Action<string, string, string> _RecieveEvent;

    void Start()
    {
        lastCommentTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneJst);
        if (isGetComment == true)
        {
            StartCoroutine(GetLiveChatId());
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
                Debug.Log("Requesting");
                break;
            case UnityWebRequest.Result.Success:
                //����
                //Debug.Log(LiveChatData.downloadHandler.text);
                Debug.Log("Succeeded");
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
                LiveChatItems item = liveChatResponse.items[i];

                liveChatMassegeQueue.Enqueue(item.snippet.displayMessage);
                userIconUrlQueue.Enqueue(item.authorDetails.profileImageUrl);
                userNameQueue.Enqueue(item.authorDetails.displayName);

                Debug.Log(item.snippet.displayMessage);
                _RecieveEvent(item.authorDetails.displayName, item.authorDetails.profileImageUrl, item.snippet.displayMessage);

                // Debug.Log(userNameQueue.Dequeue());
                //Debug.Log(userIconUrlList[i]);
            }

            if (i== liveChatResponse.items.Length - 1)
            {
                lastCommentTime = DateTime.Parse(liveChatResponse.items[i].snippet.publishedAt);
               // Debug.Log(lastCommentTime);
            }
        }
        //��ԍŐV�̃R�����g�\��
        //Debug.Log(liveChatMassegeQueue[liveChatMassegeQueue.Count - 1]);

        yield return new WaitForSeconds(5.0f);
        yield return GetLiveChatComment();
    }

}