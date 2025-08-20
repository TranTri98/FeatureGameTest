using UnityEngine;
using System.Collections;

public class KeyItem : MonoBehaviour
{
    [SerializeField] private GameConfig config;
    [SerializeField] private BoxCollider roomBounds;
    [SerializeField] private PlayerController player;
    [SerializeField] private DoorController door;

    private bool collected = false;
    private bool rerolled = false;

    private void Start()
    {
        // First spawn: far from player's start
        TeleportFarFrom(player.GetStartPosition(), config.keyMinDistanceFromStart);
        // Schedule one-time re-randomization after n seconds
        StartCoroutine(RerollOnceAfter(config.keyRespawnTime));
    }
    
    private IEnumerator RerollOnceAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (!collected && !rerolled)
        {
            rerolled = true;
            TeleportAnywhere();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (other.CompareTag("Player"))
        {
            collected = true;
            gameObject.SetActive(false);
            GameManager.Instance.NotifyKeyCollected();
            door.OpenDoor();

        }
    }

    private void TeleportFarFrom(Vector3 point, float minDistance)
    {
        Bounds b = roomBounds.bounds;
        for (int i = 0; i < 30; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(b.min.x + 2f, b.max.x - 2f),
                b.min.y + 0.25f,
                Random.Range(b.min.z + 2f, b.max.z - 2f)
            );
            if (Vector3.Distance(pos, point) >= minDistance)
            {
                transform.position = pos;
                return;
            }
        }
        // fallback
        transform.position = new Vector3(b.max.x - 1f, b.min.y + 0.25f, b.max.z - 1f);
    }

    private void TeleportAnywhere()
    {
        Bounds b = roomBounds.bounds;
        Vector3 pos = new Vector3(
            Random.Range(b.min.x + 0.5f, b.max.x - 0.5f),
            b.min.y + 0.25f,
            Random.Range(b.min.z + 0.5f, b.max.z - 0.5f)
        );
        transform.position = pos;
    }
}
