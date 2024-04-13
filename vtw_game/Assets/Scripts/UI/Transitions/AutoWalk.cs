using UnityEngine;

public class AutoWalk : MonoBehaviour
{
    public float speed = 5f; // Geschwindigkeit, mit der sich der Charakter 

    private Animator animator; // Referenz auf den Animator des Charakters

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("IsWalking", true); // Aktiviert die Walking Animation
        }
    }

    void Update()
    {
        // Bewegt das GameObject jede Sekunde um `speed` Einheiten nach rechts
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
