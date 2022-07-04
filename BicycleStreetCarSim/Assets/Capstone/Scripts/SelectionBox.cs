using UnityEngine;
using UnityEngine.UI;

namespace Capstone
{
    public class SelectionBox : MonoBehaviour
    {
        private Image imageComponent = default;
        private RectTransform rectTransform = default;
        private void OnEnable()
        {
            if(imageComponent == null)
            {
                imageComponent = this.gameObject.GetComponent<Image>();
            }
            if(rectTransform == null)
            {
                rectTransform = this.gameObject.transform.GetComponent<RectTransform>();
            }
        }
        private void OnDisable()
        {
            if (imageComponent != null)
            {
                imageComponent.enabled = false;
            }
            if(rectTransform != null)
            {
                rectTransform.position = Vector3.zero;
                rectTransform.sizeDelta = Vector2.zero;
            }
        }
        public void RenderDragBox(Vector3 startPos, Vector3 endPos)
        {
            if(imageComponent != null && rectTransform != null)
            {
                if(imageComponent.enabled == false)
                {
                    imageComponent.enabled = true;
                }

                if(imageComponent.fillCenter == true)
                {
                    imageComponent.fillCenter = false;
                }

                Debug.Log(startPos + ":" + endPos);
                rectTransform.position = new Vector3((startPos.x + endPos.x) / 2, (startPos.y + endPos.y) / 2, 0.0f);
                rectTransform.sizeDelta = new Vector2(Mathf.Abs(endPos.x - startPos.x), Mathf.Abs(endPos.y - startPos.y));
            }
        }
    }
}

