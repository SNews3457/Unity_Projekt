using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Level;
    public int LevelPoints;
    public int health;
    public float[] position;

    public PlayerData(CharacterController2D player, LevelUpManager level)
    {
        Level = level.Level;
        LevelPoints = level.LevelPoints;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;

        health = player.lives;
    }
}
