using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameConfig config;
    [SerializeField] private AudioClip spawnAudio;

    private Transform player;
    private Rigidbody rb;
    private bool active;

    private void Start()
    {
        SoundManager.Instance.PlaySFX(spawnAudio, 0.2f);
    }

    public void Setup(Transform playerRef, GameConfig cfg)
    {
        player = playerRef;
        config = cfg;
        rb = GetComponent<Rigidbody>();

        // Rigidbody setup
        rb.isKinematic = false;  
        rb.useGravity = false;  
        rb.constraints = RigidbodyConstraints.FreezeRotation; 

        active = true;
    }

    public void Deactivate()
    {
        active = false;
        gameObject.SetActive(false);
    }

    private void FixedUpdate() 
    {
        if (!active || player == null) return;
        if (GameManager.Instance.State != GameManager.GameState.Playing) return;

        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f;

        Vector3 dir = toPlayer.normalized;
        Vector3 targetPos = transform.position + dir * config.enemySpeed * Time.fixedDeltaTime;

        rb.MovePosition(targetPos);

        if (dir.sqrMagnitude > 0.0001f)
            rb.MoveRotation(Quaternion.LookRotation(dir, Vector3.up));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameManager.Instance.NotifyPlayerDied();
        }
    }
}
