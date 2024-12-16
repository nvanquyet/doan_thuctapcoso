using TMPro;
using UnityEngine;

public class InputText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TMP_InputField field;
    [SerializeField] private bool isPassword;
    private string actualText = "";
    public string Text
    {
        get
        {
            return isPassword ? actualText : field.text;
        }
    }

    void Start()
    {
        if(isPassword) field.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(string value)
    {
        if (value.Length < actualText.Length)
        {
            actualText = actualText.Substring(0, value.Length);
        }
        else if (value.Length > actualText.Length)
        {
            actualText += value[value.Length - 1];
        }

        field.text = new string('*', actualText.Length);

        field.caretPosition = field.text.Length;
    }

    public TMP_InputField Field => field;

    public void SetTitle(string title) => this.title?.SetText(title);

#if UNITY_EDITOR
    private void OnValidate()
    {
        field = GetComponentInChildren<TMP_InputField>();
    }
#endif
}
