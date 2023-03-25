using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 残機
    [SerializeField] private int _Life;

    // 得点
    [SerializeField] private ulong _Score;

    // プレイヤーオブジェクト
    [SerializeField] private GameObject _Player;

    // エネミーオブジェクト
    [SerializeField] private EnemyShip _Enemy;

    // 残機表示用のアイコンオブジェクト
    [SerializeField] private GameObject _PlayerIconPrefab;
    [SerializeField] private GameObject _PlayerIconContainer;
    private List<GameObject> _PlayerIcons;

    // 得点表示用のオブジェクト
    [SerializeField] private GameObject _ScoreTextObject;
    private TMPro.TextMeshPro _ScoreText;

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

        _ScoreText = _ScoreTextObject.GetComponent<TMPro.TextMeshPro>();
    }

    // 更新時の処理
    private void Update()
    {
        _ScoreText.text = _Score.ToString();
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
