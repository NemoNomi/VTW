using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeRenderer : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public GameObject[] ropeSegments;
    private LineRenderer lineRenderer;
    public float zOffset = 0.1f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        lineRenderer.positionCount = ropeSegments.Length + 2;
        lineRenderer.SetPosition(0, new Vector3(player1.position.x, player1.position.y, zOffset));
        lineRenderer.SetPosition(ropeSegments.Length + 1, new Vector3(player2.position.x, player2.position.y, zOffset));
        for (int i = 0; i < ropeSegments.Length; i++)
        {
            lineRenderer.SetPosition(i + 1, new Vector3(ropeSegments[i].transform.position.x, ropeSegments[i].transform.position.y, zOffset));
        }
    }
}