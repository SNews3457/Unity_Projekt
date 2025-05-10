using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public static Checkpoint Instance;
    private bool checkpointActive = false;

    private void Awake()
    {
        // Singleton-Instanz für globalen Zugriff
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            checkpointActive = true;
            Debug.Log("Checkpoint aktiviert!");
        }
    }

    public bool IsCheckpointActive()
    {
        return checkpointActive;
    }

    public Vector3 GetCheckpointPosition()
    {
        return transform.position;
    }
}

