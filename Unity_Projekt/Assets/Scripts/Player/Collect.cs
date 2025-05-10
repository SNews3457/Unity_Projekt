//op dagobert
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Collect :  MonoBehaviour
{

    public LevelUpManager levelUpManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //op Punkte werden gezählt (wenn eingesammelt)
        if (collision.CompareTag("LevelPoint"))
        {
            levelUpManager.LevelPoints++;
            Destroy(collision.gameObject);
        }
    }
}
