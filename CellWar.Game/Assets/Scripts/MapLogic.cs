///<summary>
/// 游戏场景的结构必须是一个 Map空对象，子对象为Block
/// 
/// 形如： 
///
/// Map -
///     | -Block1
///     | -Block2
///     
/// </summary>
using System.Collections;
using System.Collections.Generic;
using CellWar.Model.Map;
using UnityEngine;

public class MapLogic : MonoBehaviour {

    public static Map basicSceneMap { get; set; } = new Map();

    // Start is called before the first frame update
    void Start() {
        basicSceneMap.LoadAllBlockUnityObjectFromTransform( transform );
    }

    // Update is called once per frame
    void Update() {

    }
}
