//op Zitrone
using UnityEngine;

public class MovingSpikes : MonoBehaviour
{
    /*Das Script ist für eine Zacke, die sich von oben (der Decke) bis zur ersten Kolision bewegt.
     Nach der Kolision bewegt sie sich wieder nach oben.
     Die Zacke bleibt durchgehend mit der Decke verbunden. 
     Das Script wird auf die Zacke angelegt (Das Dreieck/der Spitze part) */

    [SerializeField] Transform yBottonMiddlePart; // Das ist der Wert wo die Zacke (Das Dreieck/der Spitze part) mit dem Mittelteil verbunden wird
    [SerializeField] Transform spikeConection; // Das ist der Wert von der Zacke, wo sie an den Mittel teil anheften muss    Die Zacke ist ein child Objekt, damit sie sich mit bewegt
    [SerializeField] Transform spikeYBottom; // Ist die kleinste Y Position der Zacke
    [SerializeField] Transform ground; // Wenn spikeYBottom kleiner ist wechselt die Zacke die Richtung
    // die Oberen Transforms sind nur Game Objekte, die eine Position angeben (die haben nichts den Objekten zutun die sich bewegen)

    [SerializeField] GameObject middlePart; // Das ist der Mitlere Teil, der größer/kleiner wird
    [SerializeField] float MovingSpeed;
    int direction = 1; // Wird bei richtungs änderung auf - 1 gesetzt
    void Start()
    {
        
    }
    void Update()
    {
        if (spikeYBottom.position.y <= ground.position.y)
        {
            direction = direction * -1;
        }
        if (middlePart.transform.localScale.y <= 0.1f)
        {
            direction = direction * -1;
        }

        float newYScale = middlePart.transform.localScale.y + 1 * direction * MovingSpeed * Time.deltaTime;

        middlePart.transform.localScale = new Vector3(middlePart.transform.localScale.x, newYScale, middlePart.transform.localScale.z);

        spikeConection.position = yBottonMiddlePart.position;

    }
}
