using UnityEngine;
using CellWar.Model.Map;
using System.Collections.Generic;

public class BlockLogic : MonoBehaviour
{
    List<GameObject> neighbour = new List<GameObject>();

    Block mBlock = new Block();
    // Start is called before the first frame update
    void Start()
    {
        mBlock.BlockType = Block.Type.Normal;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
