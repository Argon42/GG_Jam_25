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
            var dir = (worldTarget - hand.PointTransform.position).normalized;
            var dirHand = (hand.EndTransform.position - hand.PointTransform.position).normalized;
            
            Debug.DrawRay(hand.PointTransform.position, dir * 1000, Color.green);
            Debug.DrawRay(hand.PointTransform.position, dirHand * 1000, Color.red);

            float angle = -Vector2.SignedAngle(dir, dirHand);
            hand.transform.rotation = Quaternion.AngleAxis(hand.transform.eulerAngles.z + angle, Vector3.forward);
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