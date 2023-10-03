using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class HighlightNavScrollViewScript : MonoBehaviour
{
    [SerializeField] int count;
    [SerializeField] RectTransform content;
    [SerializeField] Transform buttonGroup;
    [SerializeField] ScrollRect scrollRect;

    RectTransform[] views;
    Button[] buttons;
    List<float> viewPointList;

    float ViewHeight(int i)
    {
        if (i < 0) return views[0].sizeDelta.y;
        return views[i].sizeDelta.y;
    }

    void Awake()
    {
        //init
        scrollRect.onValueChanged.AddListener(OnChange);
        float total = content.sizeDelta.y;
        views = new RectTransform[count];
        buttons = buttonGroup.GetComponentsInChildren<Button>();
        viewPointList = new List<float>();

        for (int i = 0; i < count; i++)
        {
            int index = i;
            views[i] = content.GetChild(i).GetComponent<RectTransform>();
            buttons[i].onClick.AddListener(() => MoveContent(index));
        }

        viewPointList.Add(ViewHeight(0) / 2 - ViewHeight(0));
        for (int i = 0; i < count; i++)
        {
            var t = ViewHeight(i) / 2 + ViewHeight(i - 1) / 2;
            viewPointList.Add(viewPointList.Last() + t);
        }

        OnChange(default);
    }

    private void OnChange(Vector2 arg0)
    {
        float scrollY = content.anchoredPosition.y;

        for (int i = 0; i < count; i++)
        {
            if (scrollY >= viewPointList[i] && scrollY < viewPointList[i + 1]) Highlight(i, true);
            else Highlight(i, false);
        }
    }

    private void Highlight(int index, bool on)
    {
        if (on)
        {
            buttons[index].GetComponent<Image>().color = new Color(1f, 0f, 0f, 1f);

        }
        else
        {
            buttons[index].GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void MoveContent(int index)
    {
        float posY = 0f;
        while (index-- > 0)
        {
            posY += ViewHeight(index);
        }
        scrollRect.velocity = Vector2.zero;
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, posY);
        //content.DOAnchorPosY(posY, .2f).OnStart(() => { }).OnComplete(() => { });
    }
}
