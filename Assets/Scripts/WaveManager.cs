using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    // 現在のWave
    private int _WaveIndex;

    // エネミーが発射されてからの時間
    private float _IntervalTime;
    private float _BossIntervalTime;

    // 現在発射されたエネミーの数
    private int _EnemyCount;

    // 現在がボスモードか？
    private bool _IsBossMode;

    // 残機
    [SerializeField] private int _Life;

    // 残機表示用のアイコンオブジェクト
    [SerializeField] private GameObject _PlayerIconPrefab;
    [SerializeField] private GameObject _PlayerIconContainer;
    private List<GameObject> _PlayerIcons;

    // 得点
    [SerializeField] private ulong _Score;

    // 得点表示用のオブジェクト
    [SerializeField] private GameObject _ScoreTextObject;
    private TextMeshProUGUI _ScoreText;

    // YouTubeコメント取得オブジェクト
    [SerializeField] private GetYutubeCommentR _YouTube;

    // プレイヤーオブジェクト
    [SerializeField] private PlayerControllerR _Player;
    private PlayerControllerR _SpawnedPlayer;

    // エネミーオブジェクト
    [SerializeField] private EnemyShip _Enemy;
    private List<EnemyShip> _SpawnedEnemy = new();

    [SerializeField] private SubBossEnemy _SubBossEnemy;
    private List<SubBossEnemy> _SpawnedSubBossEnemy = new();

    [SerializeField] private BossEnemy _BossEnemy;
    private BossEnemy _SpawnedBossEnemy;

    // メッセージ
    public string Message
    {
        get
        {
            if (_YouTube.liveChatMassegeQueue.TryDequeue(out string message))
            {
                _MessageList.Add(message);
                return message;
            }
            else if (_MessageList.Count > 0)
            {
                int index = Random.Range(0, _MessageList.Count);
                return _MessageList[index];
            }
            else
            {
                return "コメントがありません。";
            }
        }
    }
    private readonly List<string> _MessageList = new();

    // 投稿者のアイコン
    public string AuthorIcon
    {
        get
        {
            if (_YouTube.userIconUrlQueue.TryDequeue(out string iconURL))
            {
                _AuthorIconList.Add(iconURL);
                return iconURL;
            }
            else if (_AuthorIconList.Count > 0)
            {
                int index = Random.Range(0, _AuthorIconList.Count);
                return _AuthorIconList[index];
            }
            else
            {
                return "NonePng";
            }
        }
    }
    private readonly List<string> _AuthorIconList = new();

    // 投稿者のユーザーネーム
    public string AuthorName
    {
        get
        {
            if (_YouTube.userNameQueue.TryDequeue(out string name))
            {
                _AuthorIconList.Add(name);
                return name;
            }
            else if (_AuthorNameList.Count > 0)
            {
                int index = Random.Range(0, _AuthorNameList.Count);
                return _AuthorNameList[index];
            }
            else
            {
                return "ANONYMOUS";
            }
        }
    }
    private readonly List<string> _AuthorNameList = new();

    // 初期化時の処理
    private void Start()
    {
        _WaveIndex = 0;
        _IntervalTime = 0.0f;
        _BossIntervalTime = 0.0f;
        _IsBossMode = false;

        _SpawnedPlayer = Instantiate(_Player);
        _SpawnedPlayer.transform.localPosition = new Vector3(-7, 0);

        _PlayerIcons = new();
        for(int i = 0; i < _Life; ++i)
        {
            GameObject icon = Instantiate(_PlayerIconPrefab);
            icon.transform.parent = _PlayerIconContainer.transform;
            icon.transform.localPosition = new Vector3(100 * i + 64, 0);
            _PlayerIcons.Add(icon);
        }

        _ScoreText = _ScoreTextObject.GetComponent<TextMeshProUGUI>();
    }

    // 更新時の処理
    private void Update()
    {
        // スコア表示の更新
        _ScoreText.text = string.Format("{0:D10}", _Score);

        // ボスモードの場合
        if(_IsBossMode)
        {
            // ボスが消えたらモードを変更する
            if(!_SpawnedBossEnemy)
            {
                EnemyHit(5000);
                _IsBossMode = false;
                _WaveIndex += 1;
                _IntervalTime = 0.0f;
                _BossIntervalTime = 0.0f;
            }
        }

        // それ以外の場合
        else
        {
            // タイムを更新する
            _IntervalTime += Time.deltaTime;
            _BossIntervalTime += Time.deltaTime;

            // 2分経過したらボスモードに移る
            if(_BossIntervalTime >= 120.0f)
            {
                _SpawnedBossEnemy = Instantiate(_BossEnemy);
                _SpawnedBossEnemy.EnemyHit += EnemyHit;
                _BossEnemy.transform.position = new Vector3(9.6f, 0, 0);
            }

            // そうでない場合はザコを・30回に1回は中ボスを吐き出す
            else if(_IntervalTime >= 2.0f / (_WaveIndex * 0.1f + 1.0f))
            {
                if((++_EnemyCount) % 30 == 0)
                {
                    SubBossEnemy enemy = Instantiate(_SubBossEnemy);
                    float position = Random.Range(-4.0f, 4.0f);
                    enemy.transform.position = new Vector3(9.6f, position, 0);
                    enemy.EnemyHit += EnemyHit;
                }
                else
                {
                    EnemyShip enemy = Instantiate(_Enemy);
                    float position = Random.Range(-4.0f, 4.0f);
                    enemy.transform.position = new Vector3(9.6f, position, 0);
                    enemy.EnemyHit += EnemyHit;
                }

                _IntervalTime = 0.0f;
            }
        }

        Debug.Log(_SpawnedEnemy.Count);

        // エネミーがいなくなった時の処理
        foreach(EnemyShip enemy in _SpawnedEnemy) { if(!enemy) { EnemyHit(1000); } }
        _SpawnedEnemy = _SpawnedEnemy.Where(enemy => enemy).ToList();

        foreach(SubBossEnemy enemy in _SpawnedSubBossEnemy) { if(!enemy) { EnemyHit(3000); } }
        _SpawnedSubBossEnemy = _SpawnedSubBossEnemy.Where(enemy => enemy).ToList();

        // プレイヤーがいなくなった時の処理
        if(!_SpawnedPlayer) { PlayerHit(); }
    }

    // エネミーを配置する
    private void SpawnEnemy()
    {
        EnemyShip enemy = Instantiate(_Enemy);
        float position = Random.Range(-4.0f, 4.0f);
        enemy.transform.position = new Vector3(9.6f, position, 0);
    }

    // エネミーがヒットした時のイベント
    private void EnemyHit(ulong score)
    {
        _Score += score;
    }

    // プレイヤーがヒットした時のイベント
    private void PlayerHit()
    {
        _Life -= 1;
        if(_Life > 0)
        {
            Destroy(_PlayerIcons[_Life]);
            _PlayerIcons.RemoveAt(_Life);
            _SpawnedPlayer = Instantiate(_Player);
        }
    }

    // プレイヤーがエクステンドした時のイベント
    private void PlayerExtend()
    {
        _Life += 1;
        GameObject icon = Instantiate(_PlayerIconPrefab);
        icon.transform.localPosition = new Vector3(128 * _Life + 64, 0);
        _PlayerIcons.Add(icon);
    }
}
