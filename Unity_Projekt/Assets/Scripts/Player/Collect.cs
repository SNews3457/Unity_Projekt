//op dagobert
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Collect :  MonoBehaviour
{

    public LevelUpManager levelUpManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //op Punkte werden gezählt (wenn eingesammelt)
        if (collision.gameObject.CompareTag("LevelPoint"))
        {
            levelUpManager.LevelPoints++;
            Destroy(collision.gameObject);
        }
    }
}
