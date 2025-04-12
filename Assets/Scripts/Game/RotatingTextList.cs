using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace ZeroStats.Game
{
    public class RotatingTextList : MonoBehaviour
    {
        [SerializeField] private RectTransform container = default!;
        [SerializeField] private TMP_Text textPrefab = default!;
        [SerializeField] private float scrollSpeed = 50f;
        [SerializeField] private TextProvider textProvider = default!;

        private readonly List<TMP_Text> _items = new();
        private float _elementHeight;
        private int _count;
        private Bounds _containerRect;

        private void Start()
        {
            _elementHeight = textPrefab.GetComponent<RectTransform>().rect.height;
            _count = Mathf.CeilToInt(container.rect.height / _elementHeight)+1;
            _containerRect = RectTransformUtility.CalculateRelativeRectTransformBounds(container);

            for (int i = 0; i < _count; i++)
            {
                var item = Instantiate(textPrefab, container);
                item.text = textProvider.GetNextText();
                item.rectTransform.localPosition = Vector2.up *
                                                   (container.rect.height - i * _elementHeight -
                                                    container.rect.height * 0.5f);
                _items.Add(item);
            }
        }

        private void Update()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                var item = _items[i];
                item.rectTransform.localPosition += Vector3.up * (scrollSpeed * Time.deltaTime);
                if (item.rectTransform.localPosition.y - _elementHeight > container.rect.height * 0.5f)
                {
                    item.text = textProvider.GetNextText();
                    var last =_items.OrderBy(text => text.rectTransform.localPosition.y).First();
                    item.rectTransform.localPosition = last.rectTransform.localPosition - Vector3.up * _elementHeight;
                }
            }
        }
    }
}