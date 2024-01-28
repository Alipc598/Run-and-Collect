using UnityEngine;

public class enemyChase : MonoBehaviour
{
    // Static event for all zombies to listen to
    public static event System.Action OnPlayerHit;

    public Transform player;
    public float detectionRange = 10.0f;
    public float moveSpeed = 2.0f;
    public float fallSpeed = 10.0f; // Separate falling speed
    private Animator anim;
    private Vector3 spawnPosition;
    private bool isGrounded = true;
    private float groundCheckDistance = 1.0f;
    private float idleTimeAfterRespawn = 5.0f; // Time for which the zombie stays idle after respawn
    private float respawnTime; // Time when the zombie was last respawned

    void Start()
    {
        anim = GetComponent<Animator>();
        spawnPosition = transform.position;
        OnPlayerHit += ResetToSpawn; // Subscribe to the static event
        respawnTime = -idleTimeAfterRespawn; // Initialize to allow immediate chasing
    }

    void OnDestroy()
    {
        OnPlayerHit -= ResetToSpawn; // Unsubscribe to prevent memory leaks
    }

    void Update()
    {
        if (Time.time - respawnTime < idleTimeAfterRespawn)
        {
            // Stay idle for a certain duration after respawning
            anim.SetBool("isRunning", false);
            return; // Skip the rest of the Update method
        }

        float distance = Vector3.Distance(transform.position, player.position);
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);

        if (distance < detectionRange && isGrounded)
        {
            // Run towards the player
            anim.SetBool("isRunning", true);
            Vector3 targetDirection = (player.position - transform.position).normalized;
            targetDirection.y = 0; // Ignore y-axis for horizontal movement
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * 5f);
            transform.position += targetDirection * moveSpeed * Time.deltaTime;
        }
        else if (!isGrounded)
        {
            // If there's no ground, the zombie should fall faster
            anim.SetBool("isRunning", false);
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
        else
        {
            // If the player is out of detection range, reset position to spawn and stop running
            anim.SetBool("isRunning", false);
            transform.position = spawnPosition; // Teleport back to spawn position
        }
    }

    private void ResetToSpawn()
    {
        anim.SetBool("isRunning", false);
        transform.position = spawnPosition; // Teleport back to spawn position
        respawnTime = Time.time; // Record the time of respawn
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            TriggerPlayerHitEvent(); // Call the static method to reset all zombies
            collider.gameObject.GetComponent<player>().HitByZombie();
        }
        else if (collider.gameObject.CompareTag("deathZone"))
        {
            ResetToSpawn();
        }
    }

    public static void TriggerPlayerHitEvent()
    {
        OnPlayerHit?.Invoke();
    }
}
