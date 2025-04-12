using System;
using UnityEngine;

namespace ZeroStats.Game
{
    public class HandView : MonoBehaviour
    {
        [SerializeField] private RectTransform pointPivot = default!;
        [SerializeField] private RectTransform endPivot = default!;
        [SerializeField] private RectTransform shadow = default!;
        [SerializeField] private RectTransform hand = default!;
        [SerializeField] private float shadowOffset = 100;

        public RectTransform PointTransform => pointPivot;
        public RectTransform EndTransform => endPivot;

        private void Update()
        {
            shadow.position = pointPivot.position - Vector3.down * shadowOffset;
        }
    }
}