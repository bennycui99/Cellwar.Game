using System.Collections;
using System.Collections.Generic;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CellWar.View {
    public class U3D_AlertPanel : MonoBehaviour, IPointerClickHandler {

        public delegate void BtnEvent();
        public static BtnEvent OkEvent { get; set; } = () => { };
        public static BtnEvent CancelEvent { get; set; } = () => { };

        public static void EmitAlert( string message ) {
            GameObject.Find( "Canvas" ).transform.Find( "UI_AlertPanel" ).gameObject.SetActive( true );
            UIHelper.ChangeText( GameObject.Find( "UI_AlertMsg" ), message );
        }

        public static void EmitAlert( string message, BtnEvent ok, BtnEvent cancel ) {
            GameObject.Find( "Canvas" ).transform.Find( "UI_AlertPanel" ).gameObject.SetActive( true );
            UIHelper.ChangeText( GameObject.Find( "UI_AlertMsg" ), message );

            OkEvent = ok; CancelEvent = cancel;
        }

        public void OnPointerClick( PointerEventData eventData ) {
//            gameObject.SetActive( false );
        }

        public void Ok() {
            OkEvent();
            gameObject.SetActive( false );
            OkEvent = () => { };
        }

        public void Cancel() {
            CancelEvent();
            gameObject.SetActive( false );
            CancelEvent = () => { };
        }
    }
}
