using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CellWar.View {
    public class U3D_AlertPanel : MonoBehaviour, IPointerClickHandler {
        public void OnPointerClick( PointerEventData eventData ) {
            gameObject.SetActive( false );
        }
    }
}
