using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SaveGroundSaver : MonoBehaviour
{
    [SerializeField] private float saveFrequenzy = 3;
    public Vector2 SaveGroundLocation {  get; private set; } = Vector2.zero;
    private GroundCheck groundCheck;
    private Coroutine safeGroundCoroutine;

    private void Start()
    {
        safeGroundCoroutine = StartCoroutine(SaveGroundLocations());
        groundCheck = GetComponent<GroundCheck>();
        SaveGroundLocation = transform.position;
    }
    IEnumerator SaveGroundLocations()
    {
        float elapesdTime = 0;
        while (elapesdTime < saveFrequenzy)
        {
            elapesdTime += Time.deltaTime;
            yield return null;
        }
        if(groundCheck.IsGrounded())
        {
            SaveGroundLocation = transform.position;
        }

        safeGroundCoroutine = StartCoroutine(SaveGroundLocations());
    }

    public void WarpPlayerToSaveGround()
    {
        transform.position = SaveGroundLocation;
    }
}
