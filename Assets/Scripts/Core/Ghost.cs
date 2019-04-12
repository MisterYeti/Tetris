using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Shape m_ghostShape = null;
    private bool m_hitBottom = false;
    public Color m_color = new Color(1f, 1f, 1f, 0.2f);


    public void DrawGhost(Shape originalShape, Board gameBoard)
    {
        if (!m_ghostShape)
        {
            m_ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            m_ghostShape.gameObject.name = "GhostShape";

            SpriteRenderer[] allSpriteRenderers = m_ghostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (var renderer in allSpriteRenderers)
            {
                renderer.color = m_color;
            }
        }
        else
        {
            m_ghostShape.transform.position = originalShape.transform.position;
            m_ghostShape.transform.rotation = originalShape.transform.rotation;
            m_ghostShape.transform.localScale = Vector3.one;

        }

        m_hitBottom = false;

        while (!m_hitBottom)
        {
            m_ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition((m_ghostShape)))
            {
                m_ghostShape.MoveUp();
                m_hitBottom = true;
            }
        }
    }



    public void Reset()
    {
        Destroy(m_ghostShape.gameObject);
    }






}
