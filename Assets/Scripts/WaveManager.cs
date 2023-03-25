using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // 残機
    [SerializeField]
    private int Life;

    // 得点
    [SerializeField]
    private ulong Score;

    // プレイヤーオブジェクト
    public GameObject Player;

    // 残機表示用のアイコンオブジェクト
    public GameObject PlayerIconPrefab;
    public GameObject PlayerIconContainer;
    private List<GameObject> PlayerIcons;

    // エネミーオブジェクト
    public List<EnemyShip> Enemies;

    // 初期化時の処理
    private void Start()
    {
        PlayerIcons = new();
        for(int i = 0; i < Life; ++i)
        {
            GameObject icon = Instantiate(PlayerIconPrefab);
            icon.transform.parent = PlayerIconContainer.transform;
            icon.transform.localPosition = new Vector3(100 * i + 64, 0);
            PlayerIcons.Add(icon);
        }
    }

    // エネミーがヒットした時のイベント
    private void EnemyHit(ulong score)
    {
        Score += score;
    }

    // プレイヤーがヒットした時のイベント
    private void PlayerHit()
    {
        Life -= 1;
        Destroy(PlayerIcons[Life]);
        PlayerIcons.RemoveAt(Life);
    }

    // プレイヤーがエクステンドした時のイベント
    private void PlayerExtend()
    {
        Life += 1;
        GameObject icon = Instantiate(PlayerIconPrefab);
        icon.transform.localPosition = new Vector3(128 * Life + 64, 0);
        PlayerIcons.Add(icon);
    }
}
