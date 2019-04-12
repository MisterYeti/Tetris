using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Shape[] m_Shapes;
    public Transform[] m_queuedXForms = new Transform[3];

    Shape[] m_queuedShapes = new Shape[3];

    private float m_queueScale = .5f;

    public ParticlePlayer m_spawnFx;

    private void Awake()
    {
        InitQueue();
    }

    private Shape GetRandomShape()
    {
        var i = Random.Range(0, m_Shapes.Length);
        if (m_Shapes[i])
            return m_Shapes[i];
        else
        {
            Debug.LogWarning("Warning! Invalid shape in spawner");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        //shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        shape = GetQueuedShape();
        shape.transform.position = transform.position;

        StartCoroutine(GrowShape(shape, transform.position, 0.25f));

        if (m_spawnFx)
            m_spawnFx.Play();

        if (shape)
            return shape;
        else
        {
            Debug.LogWarning("Warning! Invalid shape in spawner");
            return null;
        }
    }


    Shape GetQueuedShape()
    {
        Shape firstShape = null;


        if (m_queuedShapes[0])
        {
            firstShape = m_queuedShapes[0];
        }

        for (int i = 1; i < m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i - 1] = m_queuedShapes[i];
            m_queuedShapes[i - 1].transform.position = m_queuedXForms[i - 1].position + m_queuedShapes[i].m_queueOffSet;
        }

        m_queuedShapes[m_queuedShapes.Length - 1] = null;

        FillQueue();

        return firstShape;










    }

    public void InitQueue()
    {
        for (int i = 0; i < m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i] = null;
        }
        FillQueue();

    }

    public void FillQueue()
    {
        for (int i = 0; i < m_queuedShapes.Length; i++)
        {
            if (!m_queuedShapes[i])
            {
                m_queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                m_queuedShapes[i].transform.position = m_queuedXForms[i].position + m_queuedShapes[i].m_queueOffSet;
                m_queuedShapes[i].transform.localScale = new Vector3(m_queueScale, m_queueScale, m_queueScale);
            }
        }
    }


    IEnumerator GrowShape(Shape shape, Vector3 position, float growTime = 0.5f)
    {
        float size = 0f;
        growTime = Mathf.Clamp(growTime, 0.1f, 2f);

        float sizeDelta = Time.deltaTime / growTime;

        while (size < 1f)
        {
            shape.transform.localScale = new Vector3(size, size, size);
            size += sizeDelta;
            shape.transform.position = position;
            yield return null;
        }

        shape.transform.localScale = Vector3.one;

    }
}
