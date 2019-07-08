using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HexAutotile.UI;
using UnityEngine;
using System.Collections;

namespace HexAutotile
{
    public class HexCellClicker : MonoBehaviour
    {
        BoxCollider2D _collider;

        IsoHexCell hexCell;

        SpriteRenderer editorSprite;
        private IHexCellClickObserver _clickObserver;

        public IsoHexCell HexCell
        {
            get
            {
                return hexCell;
            }
        }

        public IHexCellClickObserver ClickObserver
        {
            get
            {
                return _clickObserver;
            }

            set
            {
                _clickObserver = value;
            }
        }

        void Awake()
        {

        }

        void Start()
        {
            hexCell = GetComponent<IsoHexCell>();

            _collider = this.gameObject.AddComponent<BoxCollider2D>();
            _collider.offset = new Vector2(0f, -0.13f);
            _collider.size = new Vector2(0.7f, 0.3f);

            GameObject go = new GameObject("editor sprite");
            go.transform.parent = transform;
            go.transform.localPosition = Vector3.zero;
            editorSprite = go.AddComponent<SpriteRenderer>();
            //            editorSprite.sortingLayerName = "UI";
            editorSprite.sprite = hexCell.HexTerrain.editorOverSprite;
            editorSprite.enabled = false;
            editorSprite.sortingOrder = 1;

            hexCell.hexCellClicker = this;

            editorSpriteTimer.Deactivate();
        }

        void OnMouseEnter()
        {
            if (ClickObserver != null)
                ClickObserver.OnMouseEnter(this);
        }

        void OnMouseExit()
        {
            if (ClickObserver != null)
                ClickObserver.OnMouseExit(this);
        }

        void OnMouseOver()
        {
            if (ClickObserver != null)
                ClickObserver.OnMouseOver(this);
        }

        public SystemTimer editorSpriteTimer = SystemTimer.CreateLifePeriod(140);

        public void OnBrushOver()
        {
            editorSpriteTimer.Restart();
        }

        public static bool displayAlways = false;

        void Update()
        {
            if (editorSprite.enabled != editorSpriteTimer.IsActive)
                editorSprite.enabled = editorSpriteTimer.IsActive;

            if (displayAlways)
            {
                editorSprite.enabled = true;
            }
        }
    }

    public interface IHexCellClickObserver
    {
        void OnMouseEnter(HexCellClicker hexCellClicker);
        void OnMouseExit(HexCellClicker hexCellClicker);
        void OnMouseOver(HexCellClicker hexCellClicker);
    }
}
