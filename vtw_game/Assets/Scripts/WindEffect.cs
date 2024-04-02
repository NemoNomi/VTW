using System.Collections;
using UnityEngine;

public class WindEffect : MonoBehaviour
{
    public float minForce = 1f;
    public float maxForce = 5f;
    private float currentForce;
    public Vector2 windDirection = Vector2.left; // Wind weht nach links
    public LayerMask playerLayer;
    public float changeInterval = 2f; // Intervall, nach dem die Windstärke geändert wird

    private void Start()
    {
        StartCoroutine(ChangeWindForceRoutine());
    }

    IEnumerator ChangeWindForceRoutine()
    {
        while (true)
        {
            currentForce = Random.Range(minForce, maxForce);
            yield return new WaitForSeconds(changeInterval);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(windDirection * currentForce, ForceMode2D.Force);
            }
        }
    }
}
