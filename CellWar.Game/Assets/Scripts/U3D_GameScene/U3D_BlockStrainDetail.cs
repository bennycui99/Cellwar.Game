using CellWar.GameData;
using UnityEngine;
using UnityEngine.UI;
namespace CellWar.View
{

    public class U3D_BlockStrainDetail : MonoBehaviour {
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            gameObject.GetComponent<Text>().text = MainGameCurrent.GetCurrentBlockStrainDetailInfo();
        }
    }
}