using UnityEngine;
using System.Collections;

namespace HexAutotile
{
    public class PCCameraControl : MonoBehaviour
    {
        public float panSpeed = 5;
        public bool enableMousePanning = false;
        public bool enableKeyPanning = true;
        public int mousePanningZoneWidth = 10;

        public float minPosX = -10;
        public float maxPosX = 10;
        public float minPosY = -10;
        public float maxPosY = 10;

        public bool DrawGizmos = true;

        void LateUpdate()
        {
            if (enableKeyPanning)
            {
                UpdateKeyboardPan();
            }
            if (enableMousePanning)
            {
                UpdateMousePan();
            }

            float x = Mathf.Clamp(transform.position.x, minPosX, maxPosX);
            float y = Mathf.Clamp(transform.position.y, minPosY, maxPosY);

            transform.position = new Vector3(x, y, transform.position.z);
        }

        void UpdateKeyboardPan ()
        {
            Quaternion direction = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            float deltaTime = Time.smoothDeltaTime;

            if (Input.GetButton("Horizontal"))
            {
                Vector3 rightDir = transform.InverseTransformDirection(direction * Vector3.right);
                transform.Translate(rightDir * panSpeed * deltaTime * Input.GetAxisRaw("Horizontal"));
            }

            if (Input.GetButton("Vertical"))
            {
                Vector3 downDir = transform.InverseTransformDirection(direction * Vector3.up);
                transform.Translate(downDir * panSpeed * deltaTime * Input.GetAxisRaw("Vertical"));
            }
        }

        void UpdateMousePan ()
        {
            Quaternion direction = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            float deltaTime = Time.deltaTime;

            Vector3 mousePos = Input.mousePosition;
            Vector3 dirHor = transform.InverseTransformDirection(direction * Vector3.right);
            if (mousePos.x <= 0)
                transform.Translate(dirHor * panSpeed * deltaTime * -3);
            else if (mousePos.x <= mousePanningZoneWidth)
                transform.Translate(dirHor * panSpeed * deltaTime * -1);
            else if (mousePos.x >= Screen.width)
                transform.Translate(dirHor * panSpeed * deltaTime * 3);
            else if (mousePos.x > Screen.width - mousePanningZoneWidth)
                transform.Translate(dirHor * panSpeed * deltaTime * 1);

            Vector3 dirVer = transform.InverseTransformDirection(direction * Vector3.up);
            if (mousePos.y <= 0)
                transform.Translate(dirVer * panSpeed * deltaTime * -3);
            else if (mousePos.y <= mousePanningZoneWidth)
                transform.Translate(dirVer * panSpeed * deltaTime * -1);
            else if (mousePos.y >= Screen.height)
                transform.Translate(dirVer * panSpeed * deltaTime * 3);
            else if (mousePos.y > Screen.height - mousePanningZoneWidth)
                transform.Translate(dirVer * panSpeed * deltaTime * 1);
        }

        void OnDrawGizmos()
        {
            if (DrawGizmos)
            {
                Vector3 p1 = new Vector3(minPosX, maxPosY, transform.position.z);
                Vector3 p2 = new Vector3(maxPosX, maxPosY,  transform.position.z);
                Vector3 p3 = new Vector3(maxPosX, minPosY,  transform.position.z);
                Vector3 p4 = new Vector3(minPosX, minPosY,  transform.position.z);

                Gizmos.color = Color.white;

                Gizmos.DrawLine(p1, p2);
                Gizmos.DrawLine(p2, p3);
                Gizmos.DrawLine(p3, p4);
                Gizmos.DrawLine(p4, p1);
            }
        }
    }
}