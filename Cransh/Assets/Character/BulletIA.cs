using UnityEngine;

public class BulletIA : MonoBehaviour
{
    public int damage = 100; // Daño que la bala inflige al jugador

    private void Start()
    {
        // Ignorar colisiones con enemigos
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Collider enemyCollider = enemy.GetComponent<Collider>();
            if (enemyCollider != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), enemyCollider);
            }
        }

        // Ignorar colisiones con paredes
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            Collider wallCollider = wall.GetComponent<Collider>();
            if (wallCollider != null)
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), wallCollider);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour player = collision.gameObject.GetComponent<PlayerBehaviour>();
            Debug.Log("Si paso");
            if (player != null)
            {
                player.TakenDamage(damage);
            }

            // Destruir la bala después de colisionar con el jugador
            Destroy(gameObject);
        }
    }


}
