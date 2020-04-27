using UnityEngine;
using UnityEngine.EventSystems;

namespace LiveWallpaperCore {
    public class HookedInputModule : StandaloneInputModule {
#if UNITY_STANDALONE_WIN
        private readonly MouseState mouseState = new MouseState();

        private void Update() { ProcessMouseEvent(); }

        public override bool ShouldActivateModule() { return LiveWallpaper.Main.IsCurrentlyInWallpaperMode; }

        protected override MouseState GetMousePointerEventData(int id) {
            // Populate the left button...
            PointerEventData leftData;
            var created = GetPointerData(kMouseLeftId, out leftData, true);

            leftData.Reset();

            if(created)
                leftData.position = Input.mousePosition;

            Vector2 pos = Input.mousePosition;
            leftData.delta = pos - leftData.position;
            leftData.position = pos;
            leftData.scrollDelta = Input.mouseScrollDelta;
            leftData.button = PointerEventData.InputButton.Left;
            eventSystem.RaycastAll(leftData, m_RaycastResultCache);

            var raycast = FindFirstRaycast(m_RaycastResultCache);
            leftData.pointerCurrentRaycast = raycast;
            m_RaycastResultCache.Clear();

            // copy the apropriate data into right and middle slots
            PointerEventData rightData;
            GetPointerData(kMouseRightId, out rightData, true);
            CopyFromTo(leftData, rightData);
            rightData.button = PointerEventData.InputButton.Right;

            PointerEventData middleData;
            GetPointerData(kMouseMiddleId, out middleData, true);
            CopyFromTo(leftData, middleData);
            middleData.button = PointerEventData.InputButton.Middle;

            mouseState.SetButtonState(PointerEventData.InputButton.Left, GetStateForButton(0), leftData);
            mouseState.SetButtonState(PointerEventData.InputButton.Right, GetStateForButton(1), rightData);
            mouseState.SetButtonState(PointerEventData.InputButton.Middle, GetStateForButton(2), middleData);

            return mouseState;
        }

        private PointerEventData.FramePressState GetStateForButton(int id) {
            var mouseDown = HookedInput.GetMouseButtonDown(id);
            var mouseUp = HookedInput.GetMouseButtonUp(id);

            if(mouseDown && mouseUp)
                return PointerEventData.FramePressState.PressedAndReleased;
            else if(mouseDown)
                return PointerEventData.FramePressState.Pressed;
            else if(mouseUp)
                return PointerEventData.FramePressState.Released;
            else
                return PointerEventData.FramePressState.NotChanged;
        }
#endif
    }
}