using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleLogic : MonoBehaviour
{
    [SerializeField]
    public int m_maxarc = 60;

    [SerializeField]
    public int m_arc = 0;

    //[SerializeField]
    //public bool m_ischosing = false;

    [SerializeField]
    GameObject m_bench;

    [SerializeField]
    Code.Type m_type;

    public Code m_code;

    //BenchLogic m_benchLogic;

    //Start is called before the first frame update
    void Start()
    {
        m_code = new Code(m_type);
        //GetComponent<SpriteRenderer>().color = Color.blue;
        //m_benchLogic = m_bench.GetComponent<BenchLogic>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (m_ischosing)
    //    {
    //        Vector3 mousePositionInScreenSpace = Input.mousePosition;
    //        Vector3 PositionInWorldSpace = m_bench.transform.position;
    //        Vector3 PositionInScreenSpace = Camera.main.WorldToScreenPoint(PositionInWorldSpace);
    //        Vector3 diretionInScreenSpace = mousePositionInScreenSpace - PositionInScreenSpace;

    //        float t_angle = Mathf.Floor(Mathf.Atan2(diretionInScreenSpace.y, diretionInScreenSpace.x) * Mathf.Rad2Deg) - 90;
    //        if(t_angle > 0)
    //        {
    //            t_angle -= 360;
    //        }
    //        Debug.Log(t_angle);
    //        if(t_angle > m_arc || t_angle < m_arc - m_maxarc)
    //        {
    //            m_arc = m_benchLogic.Find(t_angle);
    //            if(m_benchLogic.Remap(m_arc, m_maxarc))
    //            {
    //                Rotate(0f);
    //            }
    //            else
    //            {
    //                Debug.Log("too long");
    //            }
    //        }
    //        if (Input.GetButton("Fire1"))
    //        {
    //            if(m_benchLogic.Insert(this, m_arc, m_maxarc))
    //            {
    //                m_benchLogic.m_ischosing = false;
    //                m_ischosing = false;
    //            }
    //            else
    //            {
    //                Debug.Log("too long");
    //            }
    //        }

    //        //Vector3 mousePositionInWorldSpace = Camera.main.ScreenToWorldPoint(mousePositionInScreenSpace);
    //        //mousePositionInWorldSpace.z = 0;
    //        //transform.position = mousePositionInWorldSpace;
    //    }
    //}
    //private void Update()
    //{
    //}

    public void Rotate(float i)
    {
        transform.localEulerAngles = new Vector3(0, 0, m_arc - i);
        if (m_bench)
        {
            m_bench.transform.localEulerAngles = new Vector3(0, 0, i - m_arc);
        }
    }
}
