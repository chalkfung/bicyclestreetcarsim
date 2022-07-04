using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Capstone
{
    public class SelectionPanel : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event EventHandler<SelectionPanelEventArgs> SelectionEndDrag;
        public SelectionBox selectionBox = default;
        private Vector3 startPosition = default;
        private Vector3 endPosition = default;
        private void OnEnable()
        {
            startPosition = Vector3.zero;
            endPosition = Vector3.zero;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (selectionBox != null)
            {
                selectionBox.gameObject.SetActive(true);
                startPosition = eventData.position;
                endPosition = eventData.position;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (selectionBox != null)
            {
                endPosition = eventData.position;
                selectionBox.RenderDragBox(startPosition, endPosition);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(selectionBox != null)
            {
                selectionBox.gameObject.SetActive(false);
                OnSelectionEnd();
                
            }
        }

        protected virtual void OnSelectionEnd()
        {
            SelectionEndDrag?.Invoke(this, new SelectionPanelEventArgs { startPos = startPosition, endPos = endPosition, cameraYAngle = Camera.main.transform.rotation.eulerAngles.y });
        }

        public class SelectionPanelEventArgs : EventArgs
        {
            public Vector3 startPos;
            public Vector3 endPos;
            public float cameraYAngle;
        }
    }
}

