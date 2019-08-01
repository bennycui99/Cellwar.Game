using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace CellWar.Editor {
    namespace SixSidesMap {
        [ToolboxItem( true )]
        public class SixSidesControl : Control {
            double G3 = Math.Sin( 60 * Math.PI / 180 );//二分之根号三
            private int m_sideLength = 20;

            public int SideLength {
                get { return m_sideLength; }
                set {
                    m_sideLength = value;
                    Invalidate();
                }
            }


            private float m_lineThickness = 1;

            public float LineThickness {
                get { return m_lineThickness; }
                set {
                    m_lineThickness = value;
                    Invalidate();
                }
            }


            private Color m_lineColor = Color.Black;

            public Color LineColor {
                get { return m_lineColor; }
                set {
                    m_lineColor = value;
                    Invalidate();
                }
            }

            public SixSidesControl() {
                SetStyle( ControlStyles.UserPaint, true );
                SetStyle( ControlStyles.AllPaintingInWmPaint, true );
                SetStyle( ControlStyles.DoubleBuffer, true );
            }

            protected override void OnPaint( PaintEventArgs pe ) {
                //横线，三被的边长
                //纵线，根号三倍的边长
                List<float> xList = new List<float>();
                List<float> yList = new List<float>();

                int maxx = this.Width / ( 3 * m_sideLength );
                int maxy = ( int )( this.Height / ( G3 * m_sideLength ) );

                for( int y = 0; y <= maxy; y++ ) {
                    float curHeight = ( float )( y * G3 * m_sideLength );
                    for( int x = 0; x <= maxx; x++ ) {
                        float curWidth;
                        if( y % 2 == 0 )
                            curWidth = ( float )( x * 3 * m_sideLength );
                        else
                            curWidth = ( float )( ( x * 3 + 1.5 ) * m_sideLength );

                        yList.Add( curHeight );
                        xList.Add( curWidth );
                    }
                }

                OnPaint( pe, xList.ToArray(), yList.ToArray() );

                base.OnPaint( pe );
            }

            private void OnPaint( PaintEventArgs pe, float[] x, float[] y ) {
                pe.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                using( Pen pen = new Pen( new SolidBrush( m_lineColor ), m_lineThickness ) ) {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    for( int i = 0; i < x.Length; i++ ) {
                        //9点方向的点
                        float px1 = ( float )( x[i] - m_sideLength );
                        float py1 = ( float )( y[i] );

                        ////11点方向的点
                        //float px2 = (float)(x[i] - 0.5 * m_sideLength);
                        //float py2 = (float)(y[i] - G3 * m_sideLength);

                        ////1点方向的点
                        //float px3 = (float)(x[i] + 0.5 * m_sideLength);
                        //float py3 = (float)(y[i] - G3 * m_sideLength);

                        //3点方向的点
                        float px4 = ( float )( x[i] + m_sideLength );
                        float py4 = ( float )( y[i] );

                        //5点方向的点
                        float px5 = ( float )( x[i] + 0.5 * m_sideLength );
                        float py5 = ( float )( y[i] + G3 * m_sideLength );

                        //7点方向的点
                        float px6 = ( float )( x[i] - 0.5 * m_sideLength );
                        float py6 = ( float )( y[i] + G3 * m_sideLength );

                        //pe.Graphics.DrawLine(pen, px1, py1, px2, py2);
                        //pe.Graphics.DrawLine(pen, px2, py2, px3, py3);
                        //pe.Graphics.DrawLine(pen, px3, py3, px4, py4);
                        //pe.Graphics.DrawLine(pen, px4, py4, px5, py5);
                        //pe.Graphics.DrawLine(pen, px5, py5, px6, py6);
                        //pe.Graphics.DrawLine(pen, px6, py6, px1, py1);

                        pe.Graphics.DrawLines( pen, new PointF[]
                        {
                        new PointF(px4, py4),
                        new PointF(px5, py5),
                        new PointF(px6, py6),
                        new PointF(px1, py1)
                        } );
                    }
                }
            }

            private void InitializeComponent() {
                this.SuspendLayout();
                this.ResumeLayout(false);
            }
        }
    }
}
