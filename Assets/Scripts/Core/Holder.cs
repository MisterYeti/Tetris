using UnityEngine;

public class Holder : MonoBehaviour
{

    public Transform m_holderXForm;
    public Shape m_heldShape = null;
    private float m_scale = 0.5f;
    public bool m_canRelease = false;

    public void Catch(Shape shape)
    {

        if (m_heldShape)
        {
            Debug.LogWarning("Holder Warning : Release a shape before trying to hold");
            return;
        }

        if (!shape)
        {
            Debug.LogWarning("Holder warning : Invalid shape");
        }

        if (m_holderXForm)
        {
            shape.transform.position = m_holderXForm.position + shape.m_queueOffSet;
            shape.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            m_heldShape = shape;
            shape.transform.rotation = Quaternion.identity;
        }
        else
        {
            Debug.LogWarning("Holder warning : Holder has no transform assigned");
        }
    }

    public Shape Release()
    {
        m_heldShape.transform.localScale = Vector3.one;

        Shape shape = m_heldShape;

        m_heldShape = null;

        m_canRelease = false;
        return shape;

    }


}

