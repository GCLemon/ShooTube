using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
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

    // エネミーオブジェクト
    [SerializeField] private EnemyShip _Enemy;

    // 初期化時の処理
    private void Start()
    {
        _PlayerIcons = new();
        for(int i = 0; i < _Life; ++i)
        {
            GameObject icon = Instantiate(_PlayerIconPrefab);
            icon.transform.parent = _PlayerIconContainer.transform;
            icon.transform.localPosition = new Vector3(100 * i + 64, 0);
            _PlayerIcons.Add(icon);
        }

        _ScoreText = _ScoreTextObject.GetComponent<TextMeshProUGUI>();

        InvokeRepeating(nameof(SpawnEnemy), 2f, 2f);
    }

    // エネミーを配置する
    private void SpawnEnemy()
    {
        EnemyShip enemy = Instantiate(_Enemy);
        float position = Random.Range(-4.0f, 4.0f);
        enemy.transform.position = new Vector3(9.6f, position, 0);
    }

    // 更新時の処理
    private void Update()
    {
        _ScoreText.text = string.Format("{0:D10}", _Score);
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
        Destroy(_PlayerIcons[_Life]);
        _PlayerIcons.RemoveAt(_Life);
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
