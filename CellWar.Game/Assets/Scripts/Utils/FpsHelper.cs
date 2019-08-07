using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CellWar.Utils {
    public class FpsHelper : MonoBehaviour {
        //更新的时间间隔
        public float UpdateInterval = 0.3F;
        //最后的时间间隔s
        private float _lastInterval;
        //帧[中间变量 辅助]
        private int _frames = 0;
        //当前的帧率
        private float _fps;
        private void Awake() {

        }
        void Start() {
            UpdateInterval = Time.realtimeSinceStartup;
            _frames = 0;
        }
        void OnGUI() {
            GUI.Label( new Rect( 100, 100, 200, 200 ), "FPS:" + _fps.ToString( "f2" ) );
        }
        void Update() {
            ++_frames;
            if( Time.realtimeSinceStartup > _lastInterval + UpdateInterval ) {
                _fps = _frames / ( Time.realtimeSinceStartup - _lastInterval );
                _frames = 0;
                _lastInterval = Time.realtimeSinceStartup;
            }
        }
    }
}