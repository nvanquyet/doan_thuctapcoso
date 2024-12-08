using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TetrisItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image image;
    [SerializeField] private Color colorMarked;
    [SerializeField] private Color colorUnmarked;

    public void OnMarkItem(bool mark)
    {
        //itemText.SetText(mark ? "1" : "0");
        //itemText.gameObject.SetActive(false);
        //image.color = mark ? colorMarked : colorUnmarked;
    }
}
