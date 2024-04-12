using UnityEngine;
using System.Collections;


public class MovingPlatform : MonoBehaviour
{
    #region Platform Fields
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private Vector3 nextPosition;
    #endregion


    #region Player Fields
    public GameObject player1;
    public GameObject player2;
    public float maxDistance = 5.0f;
    #endregion


    #region Lifecycle
    void Start()
    {
        nextPosition = pointB.position;
    }

    void FixedUpdate()
    {
        MovePlatform();
        CheckPlayerDistance();
    }
    #endregion


    #region Platform Movement
    void MovePlatform()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.fixedDeltaTime);
        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == pointA.position) ? pointB.position : pointA.position;
        }
    }
    #endregion


    #region Player Distance
    void CheckPlayerDistance()
    {
        if (player1 != null && player2 != null)
        {
            if (Vector3.Distance(player1.transform.position, player2.transform.position) > maxDistance)
            {
                DetachPlayersIfNeeded(player1);
                DetachPlayersIfNeeded(player2);
            }
        }
    }
    #endregion


    #region Player Movement
    void OnCollisionEnter2D(Collision2D collision)
    {
        AttachPlayer(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        DetachPlayersIfNeeded(collision.gameObject);
    }

    void AttachPlayer(GameObject player)
    {
        if (player == player1 || player == player2)
        {
            player.transform.SetParent(transform);
        }
    }

void DetachPlayersIfNeeded(GameObject player)
{
    if ((player == player1 || player == player2) && player.transform.parent == transform)
    {
        StartCoroutine(DetachPlayerNextFrame(player));
    }
}
    #endregion

    #region Coroutine

    IEnumerator DetachPlayerNextFrame(GameObject player)
{
    yield return null;
    if (player != null && (player == player1 || player == player2) && player.transform.parent == transform)
    {
        player.transform.SetParent(null);
    }
}
#endregion

}
