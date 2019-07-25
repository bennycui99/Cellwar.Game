using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchLogic : MonoBehaviour {
    public bool m_ischosing = false;

    [SerializeField]
    List<CircleLogic> m_cirvles;

    CircleLogic m_chose;

    int m_long;
    // Start is called before the first frame update
    void Start() {
        Map0();
    }

    // Update is called once per frame
    void Update() {
        if( m_ischosing ) {
            Vector3 mousePositionInScreenSpace = Input.mousePosition;
            Vector3 PositionInWorldSpace = transform.position;
            Vector3 PositionInScreenSpace = Camera.main.WorldToScreenPoint( PositionInWorldSpace );
            Vector3 diretionInScreenSpace = mousePositionInScreenSpace - PositionInScreenSpace;

            float t_angle = Mathf.Floor( Mathf.Atan2( diretionInScreenSpace.y, diretionInScreenSpace.x ) * Mathf.Rad2Deg ) - 90;
            if( t_angle > 0 ) {
                t_angle -= 360;
            }

            if( t_angle > m_chose.m_arc || t_angle < m_chose.m_arc - m_chose.m_maxarc ) {
                m_chose.m_arc = Find( t_angle );
                if( Remap( m_chose.m_arc, m_chose.m_maxarc ) ) {
                    m_chose.Rotate( 0f );
                } else {
                    Debug.Log( "too long" );
                }
            }
            if( Input.GetButtonUp( "Fire1" ) ) {
                if( Mathf.Sqrt( diretionInScreenSpace.x * diretionInScreenSpace.x + diretionInScreenSpace.y * diretionInScreenSpace.y ) < 100 ) {
                    Destroy( m_chose.gameObject );
                    m_ischosing = false;
                    m_chose = null;
                    Map0();
                } else {
                    if( Insert( m_chose, m_chose.m_arc, m_chose.m_maxarc ) ) {
                        m_ischosing = false;
                        m_chose = null;
                        Map();
                    } else {
                        Debug.Log( "too long" );
                    }
                }
            }
        } else {
            if( Input.GetButtonDown( "Fire1" ) ) {
                Vector3 mousePositionInScreenSpace = Input.mousePosition;
                Vector3 PositionInWorldSpace = transform.position;
                Vector3 PositionInScreenSpace = Camera.main.WorldToScreenPoint( PositionInWorldSpace );
                Vector3 diretionInScreenSpace = mousePositionInScreenSpace - PositionInScreenSpace;


                if( Mathf.Sqrt( diretionInScreenSpace.x * diretionInScreenSpace.x + diretionInScreenSpace.y * diretionInScreenSpace.y ) < 430 ) {
                    float t_angle = Mathf.Atan2( diretionInScreenSpace.y, diretionInScreenSpace.x ) * Mathf.Rad2Deg - 90;
                    if( t_angle > 0 ) {
                        t_angle -= 360;
                    }

                    m_chose = m_cirvles[0];
                    foreach( CircleLogic i in m_cirvles ) {
                        if( i.m_arc <= t_angle ) {
                            m_ischosing = true;
                            m_cirvles.Remove( m_chose );
                            Map();
                            return;
                        }
                        m_chose = i;
                    }
                }
            }
        }
    }

    //排列
    public void Map() {
        int t = 0;
        foreach( CircleLogic i in m_cirvles ) {
            i.m_arc = t;
            t -= i.m_maxarc;
        }
        m_long = t;
    }

    //初始化排列
    public void Map0() {
        int t = 0;
        foreach( CircleLogic i in m_cirvles ) {
            i.m_arc = t;
            t -= i.m_maxarc;
            i.Rotate( 0f );
        }
        m_long = t;
    }


    //临时排列
    public bool Remap( int arc, int maxarc ) {
        if( m_long + maxarc > 360 ) {
            return false;
        }
        foreach( CircleLogic i in m_cirvles ) {
            if( i.m_arc <= arc ) {
                i.Rotate( maxarc );
            } else {
                i.Rotate( 0f );
            }
        }
        return true;
    }

    //插入
    public bool Insert( CircleLogic c, int arc, int maxarc ) {
        if( m_long + maxarc > 360 || arc + maxarc > 360 ) {
            return false;
        }
        foreach( CircleLogic i in m_cirvles ) {
            if( i.m_arc <= arc ) {
                m_cirvles.Insert( m_cirvles.IndexOf( i ), c );
                Map();
                return true;
            }
        }
        return false;
    }

    //查找当前角度对应位置
    int Find( float arc ) {
        int t = 360;
        foreach( CircleLogic i in m_cirvles ) {
            if( i.m_arc > arc ) {
                t = i.m_arc;
            } else {
                return t;
            }
        }
        return t;
    }

    //编译当前基因组
    public void Make() {
        List<Code> t = new List<Code>();
        for( int i = 0; i < m_cirvles.Count; i++ ) {
            //if (m_cirvles[i].m_code.c_type == Code.Type.G1)
            //{
            //    t.Add(m_cirvles[i].m_code);
            //    m_cirvles[i].m_code.c_nexts = m_cirvles[++i].m_code;
            //}
            //else
            //{
            //    t.Add(m_cirvles[i].m_code);
            //}
            t.Add( m_cirvles[i].m_code );
        }
        //Code t = m_cirvles[0].m_code;
        foreach( Code i in t ) {
            i.c_F();
        }
        //return t;
    }
}







//转码
public class Code {
    public Type c_type;
    public Code c_nexts;
    public Function c_F;

    public delegate void Function();

    public enum Type {
        G1,
        G2,
        G3,
        G4,
        G5,
        end
    }

    public Code( Type type ) {
        c_type = type;
        switch( type ) {
            case Type.G1:
                c_F = new Function( this.G1 );
                break;
            case Type.G2:
                c_F = new Function( this.G2 );
                break;
            case Type.G3:
                c_F = new Function( this.G3 );
                break;
            case Type.G4:
                c_F = new Function( this.G4 );
                break;
            case Type.G5:
                c_F = new Function( this.G5 );
                break;
            case Type.end:
                c_F = new Function( this.End );
                break;
        }
    }

    public void G1() {
        Debug.Log( "G1" );
        //c_nexts.c_F();
    }
    public void G2() {
        Debug.Log( "G2" );
    }
    public void G3() {
        Debug.Log( "G3" );
    }
    public void G4() {
        Debug.Log( "G4" );
    }
    public void G5() {
        Debug.Log( "G5" );
    }


    public void End() {
        return;
    }
}
