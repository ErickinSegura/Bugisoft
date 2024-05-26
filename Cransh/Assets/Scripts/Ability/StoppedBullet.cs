using UnityEngine;
using System.Collections;

public class StoppedBullet : MonoBehaviour
{
    private Vector3 direction;
    private Vector3 hitPoint;
    private LineRenderer lineRenderer;
    private Transform bulletTransform;
    private Collider bulletCollider; // Referencia al collider de la bala
    private Rigidbody bulletRigidbody; // Referencia al Rigidbody de la bala

    public int weaponDamage = 10; // Daño de la bala

    public void Initialize(Vector3 bulletDirection, Vector3 impactPoint)
    {
        direction = bulletDirection;
        hitPoint = impactPoint;
        DrawLine();
        OrientBullet();
        DisableCollider();
    }

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.startColor = Color.red;
            lineRenderer.endColor = Color.red;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        }

        bulletTransform = transform.GetChild(0); // Asegúrate de que la cápsula sea el primer hijo del objeto
        if (bulletTransform == null)
        {
            Debug.LogWarning("No child found for the bullet object.");
        }

        bulletCollider = GetComponentInChildren<Collider>(); // Obtener el collider del objeto hijo (la cápsula)
        if (bulletCollider == null)
        {
            Debug.LogWarning("No Collider found on the bullet object.");
        }

        bulletRigidbody = GetComponent<Rigidbody>(); // Obtener el Rigidbody
        if (bulletRigidbody == null)
        {
            bulletRigidbody = gameObject.AddComponent<Rigidbody>();
        }

        // Configurar el Rigidbody
        bulletRigidbody.useGravity = false;
        bulletRigidbody.isKinematic = true; // Inicialmente cinemático
        bulletRigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        bulletRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    private void DrawLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitPoint);
        }
    }

    private void OrientBullet()
    {
        if (bulletTransform != null)
        {
            // Calcular la rotación basada en la dirección de la bala
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            bulletTransform.rotation = lookRotation;
        }
    }

    private void DisableCollider()
    {
        if (bulletCollider != null)
        {
            bulletCollider.enabled = false; // Desactivar el collider
        }
    }

    private void EnableCollider()
    {
        if (bulletCollider != null)
        {
            bulletCollider.enabled = true; // Activar el collider
        }
    }

    public void MoveBulletAlongLine(float duration)
    {
        StartCoroutine(MoveBulletCoroutine(duration));
    }

    private IEnumerator MoveBulletCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        EnableCollider(); // Activar el collider al inicio del movimiento
        bulletRigidbody.isKinematic = true; // Asegurarse de que el Rigidbody sea cinemático durante el movimiento
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = Vector3.Lerp(startPosition, hitPoint, elapsedTime / duration);
            yield return null;
        }
        bulletRigidbody.isKinematic = false; // Devolver el Rigidbody a su estado no cinemático
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("CHOQUE");
            Destroy(other.gameObject); // Destruye el enemigo
            Destroy(gameObject); // Destruye la bala
        }
    }
}
