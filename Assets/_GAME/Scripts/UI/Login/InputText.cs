using UnityEngine;
using UnityEngine.UI;

public class InputText : MonoBehaviour
{
    [SerializeField] private Text title;
    [SerializeField] private InputField field;

    public string Text
    {
        get => field.text;
        set => field.text = value;
    }

    public InputField Field => field;

#if UNITY_EDITOR
    private void OnValidate()
    {
        field = GetComponentInChildren<InputField>();
        if (title == null) title = GetComponentInChildren<Text>();
    }
#endif
}
