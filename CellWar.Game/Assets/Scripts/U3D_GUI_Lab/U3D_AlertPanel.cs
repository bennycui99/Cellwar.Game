using System.Collections;
using System.Collections.Generic;
using CellWar.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using static CellWar.Utils.LambdaHelper;

namespace CellWar.View {
    public class U3D_AlertPanel : MonoBehaviour, IPointerClickHandler {

        public static BaseEvent OkEvent { get; set; } = () => { };
        public static BaseEvent CancelEvent { get; set; } = () => { };

        public static void EmitAlert( string message ) {
            GameObject.Find( "BlockingCanvas" ).transform.Find( "UI_AlertPanel" ).gameObject.SetActive( true );
            GameObject.Find( "BlockingCanvas" ).transform.Find( "UI_AlertPanel" ).gameObject.transform.SetSiblingIndex( 999 );
            UIHelper.ChangeText( GameObject.Find( "UI_AlertMsg" ), message );
        }

        public static void EmitAlert( string message, BaseEvent ok, BaseEvent cancel ) {
            EmitAlert( message );
            OkEvent = ok; CancelEvent = cancel;
        }

        public void OnPointerClick( PointerEventData eventData ) {
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
