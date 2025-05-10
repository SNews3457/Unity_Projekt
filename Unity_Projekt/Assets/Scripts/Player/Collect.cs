//op dagobert
using UnityEngine;

public class Collect :  MonoBehaviour
{

    public LevelUpManager levelUpManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //op Punkte werden gez�hlt (wenn eingesammelt)
        if (collision.CompareTag("LevelPoint"))
        {
            levelUpManager.LevelPoints++;
            Destroy(collision.gameObject);
        }
    }
}
