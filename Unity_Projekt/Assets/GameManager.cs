using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CharacterController2D player;
    [SerializeField] private LevelUpManager level;
    public bool DeleteData = false;

    private void Start()
    {
        if (DeleteData)
        {
            ResetPlayerData();
        }
        else
        {
            LoadPlayer();
        }

        StartCoroutine(AutoSave());
    }

    public void SavePlayer()
    {
        SaveSytem.SavePlayer(player, level);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSytem.LoadPlayer();
        if (data == null) return;

        player.lives = data.health;
        level.LevelPoints = data.LevelPoints;
        level.Level = data.Level;

        Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
        player.transform.position = position;
    }

    public void ResetPlayerData()
    {
        player.lives = 5;
        level.LevelPoints = 0;
        level.Level = 0;
        player.transform.position = new Vector3(-120, 36, 0);
    }

    private IEnumerator AutoSave()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            SavePlayer();
        }
    }
}
