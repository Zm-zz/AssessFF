using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LogOn : MonoBehaviour
{
    public InputField accountText;
    public InputField passwordText;
    public GameObject errorText;

    public UnityEvent OnLogEvent;

    private void Awake()
    {
        accountText.onSubmit.AddListener(Verify);
        passwordText.onSubmit.AddListener(Verify);
    }

    private void Update()
    {
        if (accountText.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                passwordText.ActivateInputField();
                passwordText.Select();
            }
        }
        if (passwordText.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                accountText.ActivateInputField();
                accountText.Select();
            }
        }
    }

    public void Verify(string s)
    {
        Verify();
    }

    public void Verify()
    {
#if UNITY_EDITOR
        AllowOn();
        return;
#endif
#pragma warning disable CS0162 // 检测到无法访问的代码
        string a = accountText.text;
        string b = passwordText.text;
        if (a.CompareTo("test") == 0 && b.CompareTo("ugion") == 0)
        {
            AllowOn();
        }
        else if (a.CompareTo("12345678") == 0 && b.CompareTo("qwertyui") == 0)
        {
            AllowOn();
        }
        else if (a.CompareTo("5") == 0 && b.CompareTo("55") == 0)
        {
            AllowOn();
        }
        else
        {
            errorText.SetActive(true);
        }
#pragma warning restore CS0162 // 检测到无法访问的代码
    }

    public void AllowOn()
    {
        accountText.text = null;
        passwordText.text = null;
        errorText.SetActive(false);
        OnLogEvent?.Invoke();
        transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        errorText.SetActive(false);
    }
}
