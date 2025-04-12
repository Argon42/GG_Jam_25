using UnityEngine;

namespace ZeroStats.Game
{
    public class MouseFinder : MonoBehaviour
    {
        [SerializeField] private RectTransform area = default!;
        [SerializeField] private HandView handLeft = default!;
        [SerializeField] private HandView handRight = default!;
        [SerializeField] private RectTransform pivotHandLeft = default!;
        [SerializeField] private RectTransform pivotHandRight = default!;

        public Vector2 GetMousePosition() => Input.mousePosition;

        private void Awake()
        {
            handLeft.gameObject.SetActive(false);
            handRight.gameObject.SetActive(true);
        }

        private void Update()
        {
            handRight.transform.localPosition =
                Vector3.Lerp(handRight.transform.localPosition, GetLocalMousePosition(area), 0.6f);

            AlignHandToPoint(handRight, pivotHandRight.position);
        }

        private void AlignHandToPoint(HandView hand, Vector3 worldTarget)
        {
            Vector3 worldA = hand.PointTransform.position;
            Vector3 worldC = hand.EndTransform.position;

            Vector2 dirAC = (worldC - worldA).normalized;
            Vector2 dirAB = (worldTarget - worldA).normalized;

            float angle = Vector2.SignedAngle(dirAC, dirAB);
            hand.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Vector3 GetLocalMousePosition(RectTransform rectTransform)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                Input.mousePosition,
                null,
                out var localPoint
            );
            return localPoint;
        }
    }
}