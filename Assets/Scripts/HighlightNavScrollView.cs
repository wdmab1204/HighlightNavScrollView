using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HNSV
{
    public class HighlightEvent : UnityEvent<int> { }

    public class HighlightNavScrollView : ScrollRect
    {
        [SerializeField] int elementalCount;
        [SerializeField] RectTransform buttonGroup;
        [SerializeField] Color normalColor;
        [SerializeField] Color highlightColor;

        RectTransform[] views;
        Button[] buttons;
        Image[] buttonImages;
        Image[] originalButtonImages;
        List<float> viewPointList;

        public HighlightEvent onHighlightChanged = new HighlightEvent();

        float ViewHeight(int i)
        {
            if (i < 0) return views[0].sizeDelta.y;
            return views[i].sizeDelta.y;
        }

        protected override void Awake()
        {
            onValueChanged.AddListener(OnChange);
            float total = content.sizeDelta.y;
            views = new RectTransform[elementalCount];
            buttons = buttonGroup.GetComponentsInChildren<Button>();
            buttonImages = new Image[elementalCount];
            viewPointList = new List<float>();

            for (int i = 0; i < elementalCount; i++)
            {
                int index = i;
                views[i] = content.GetChild(i).GetComponent<RectTransform>();
                buttons[i].onClick.AddListener(() => MoveContent(index));
                buttonImages[i] = buttons[i].GetComponent<Image>();
            }

            originalButtonImages = buttonImages;
            viewPointList.Add(ViewHeight(0) / 2 - ViewHeight(0));
            for (int i = 0; i < elementalCount; i++)
            {
                var t = ViewHeight(i) / 2 + ViewHeight(i - 1) / 2;
                viewPointList.Add(viewPointList.Last() + t);
            }

            OnChange(default);
            base.Awake();
        }

        protected override void OnDisable()
        {
            onValueChanged.RemoveListener(OnChange);
            buttonImages = originalButtonImages;
            for (int i = 0; i < elementalCount; i++)
            {
                int index = i;
                buttons[i].onClick.RemoveListener(() => MoveContent(index));
            }
            base.OnDisable();
        }

        private void OnChange(Vector2 arg0)
        {
            float scrollY = content.anchoredPosition.y;

            for (int i = 0; i < elementalCount; i++)
            {
                if (scrollY >= viewPointList[i] && scrollY < viewPointList[i + 1])
                {
                    onHighlightChanged?.Invoke(i);
                    Highlight(i, true);
                }
                else Highlight(i, false);
            }
        }

        private void Highlight(int index, bool on)
        {
            if (on)
            {
                buttonImages[index].color = highlightColor;
            }
            else
                buttonImages[index].color = normalColor;
        }

        private void MoveContent(int index)
        {
            float posY = 0f;
            while (index-- > 0)
            {
                posY += ViewHeight(index);
            }
            velocity = Vector2.zero;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, posY);
            //content.DOAnchorPosY(posY, .2f).OnStart(() => { }).OnComplete(() => { });
        }
    }
}