﻿using System;
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
        [SerializeField] private float angleOfClick = 0.3f;
        private Vector3 _startHandPosition;

        public RectTransform PointTransform => pointPivot;
        public RectTransform EndTransform => endPivot;


        private void Update()
        {
            shadow.position = pointPivot.position - Vector3.down * shadowOffset;
            if (Input.GetMouseButton(0))
            {
                var dir1 = (endPivot.position - pointPivot.position).normalized;
                var needed = Vector2.Lerp(dir1, Vector2.down, angleOfClick);
                var angle = Vector2.SignedAngle(Vector2.down, needed);
                hand.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                hand.position = pointPivot.position - Vector3.down * shadowOffset;
            }
            else
            {
                hand.localRotation = Quaternion.AngleAxis(0, Vector3.forward);
                hand.position = pointPivot.position;
            }
        }
    }
}