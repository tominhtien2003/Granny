using UnityEngine;

public class CellsVisualizer : MonoBehaviour
{
    public Vector3Int m_cellAmount;
    public Vector3 m_cellSize;
    public bool DrawGizmos = false;
    private void OnDrawGizmosSelected()
    {
        if (!DrawGizmos) return;
        Gizmos.color = Color.red;
        for (int i = 0; i < m_cellAmount.x; i++)
        {
            for (int j = 0; j < m_cellAmount.y; j++)
            {
                for (int k = 0; k < m_cellAmount.z; k++)
                {
                    Vector3 begin = transform.position + new Vector3(i * m_cellSize.x, j * m_cellSize.y, k * m_cellSize.z);
                    Vector3 center = begin + m_cellSize * .5f;
                    Gizmos.DrawWireCube(center, m_cellSize);
                }
            }
        }
    }
}
