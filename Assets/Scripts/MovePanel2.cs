using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.TouchControl
{
    public class MovePanel2 : MonoBehaviour
                                   , IPointerDownHandler, IPointerUpHandler
    {
        private        RectTransform _RectTr;
        private        RectTransform _ParentCanvas;

        private        Vector2       _Offset = Vector2.zero;
        private        bool          _IsDrag = false;
        private static bool          _isBusy = false;
        private        float         _Angle;
        private Camera _Camera;
        private void Start()
        {
            _RectTr       = (RectTransform) transform;
            _ParentCanvas = (RectTransform) gameObject.GetComponentInParent<Canvas>().transform;
            _Camera = Camera.main;
        }

        private void Update()
        {
            if (!_isBusy || !_IsDrag) return;
#if !UNITY_EDITOR
            if (Input.touchCount == 1)
            {
                var p = Input.touches[0].position;
#else
            {
                Vector2 p = Input.mousePosition;
#endif
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_ParentCanvas,
                    p, _Camera, out p);
                Move(p + _Offset);
            }
            if (Input.touchCount == 2)
            {
                if (Input.touches[0].phase == TouchPhase.Began || Input.touches[1].phase == TouchPhase.Began)
                {
                    var touch1 = Input.touches[0].position;
                    var touch2 = Input.touches[1].position;
                    _Angle = Vector2.SignedAngle(Vector2.up, touch2 - touch1);
                }
                else
                {
                    var touch1 = Input.touches[0].position;
                    var touch2 = Input.touches[1].position;
                    var angle  = Vector2.SignedAngle(Vector2.up, touch2 - touch1);
                    //var rotate = _RectTr.rotation.eulerAngles;
                    //rotate.z         = angle - _Angle;
                    //_RectTr.rotation = Quaternion.Euler(rotate);
                }
            }
        }

        private void Move(Vector2 position)
        {
            var rect                                                             = _RectTr.rect;
            if (rect.xMin  + position.x < 0) position.x                          = -rect.xMin;
            if (rect.xMax  + position.x > _ParentCanvas.sizeDelta.x) position.x  = _ParentCanvas.sizeDelta.x - rect.xMax;
            if (position.y + rect.yMax  > 0) position.y                          = -(rect.yMax);
            if (position.y + rect.yMin  < -_ParentCanvas.sizeDelta.y) position.y = -(_ParentCanvas.sizeDelta.y + rect.yMin);
            _RectTr.anchoredPosition = position;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isBusy) return;
            _isBusy = true;
            _IsDrag = true;

            var p = eventData.position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_ParentCanvas,
                p, _Camera, out p);
            _Offset =  _RectTr.anchoredPosition - p;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isBusy && _IsDrag)
                _isBusy = false;
            _IsDrag = false;
        }
    }
}
