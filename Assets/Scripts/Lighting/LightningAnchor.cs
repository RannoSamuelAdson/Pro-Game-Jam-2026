using UnityEngine;

public class LightningAnchor : MonoBehaviour
{
    public LightningAnchor nextGoalPoint;
    void OnDrawGizmos()
    {
        if (nextGoalPoint != null)
        {
            // Set the color of the line
            Gizmos.color = Color.green;

            // Draw a line from this anchor to the next
            Gizmos.DrawLine(transform.position, nextGoalPoint.transform.position);
        }
    }
}
