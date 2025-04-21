using Sirenix.OdinInspector;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Robot : SingletonPatternMonoAutoBase<Robot>
{
    private float popSpeed = 5f;
    private bool isShown;
    private Coroutine popCor;
    private Coroutine hideCor;
    private Coroutine displayCor;

    [Title("�ı�")]
    public Text content;
    public Text contentByButton;

    [Title("�۾�")]
    public Image eyeL;
    public Image eyeR;
    public Sprite eye_Nor;
    public Sprite eye_Warn;

    [Title("�Ի���")]
    public Transform dialogBox;
    public Transform dialogBoxByBut;

    [Title("������ʾʱ��")]
    public float displayTime = 1f;

    [Title("��ť")]
    public Button but_X;
    public Button but_XHasBut;
    public Button but_L;
    public Button but_R;

    [Title("��ǰ����")]
    [ReadOnly][ShowInInspector][Multiline] private string currentContent;

    [Title("�߹���ʾ")]
    public bool isTip;

    public void Initialize()
    {
        HideAllDialog();

        but_X.onClick.AddListener(Hide);
    }

    /// <summary>
    /// ��������˻������ʾ
    /// </summary>
    public void ControlHighlightTip()
    {
        if (GlobalComponent.Instance.GameMode == GameMode.Exam) return;
        isTip = !isTip;
        ModelCtrl.Instance.SetInteractiveModelsHighlight(isTip);
    }

    /// <summary>
    /// ���û������۾���ɫ
    /// </summary>
    /// <param name="isWarning"></param>
    public void SetEye(bool isWarning)
    {
        eyeL.sprite = isWarning ? eye_Warn : eye_Nor;
        eyeR.sprite = isWarning ? eye_Warn : eye_Nor;
    }

    #region HaveButton
    public void ShowTipByBut(string content, UnityAction actL = null, UnityAction actR = null, string lContent = "��", string rContent = "��", bool isSpeak = true)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audios/" + content);
        if (clip == null)
        {
            Debug.Log($"<color=red>δ�ҵ���Ƶ��</color>{content}");
            if (isSpeak)
                SetContent($"<color=red>δ�ҵ���Ƶ��</color>{content}");
            else
                SetContent(content);
        }
        else
        {
            TalkWithMod.Instance.Play(clip);

            SetContent(content);
        }

        transform.localScale = Vector3.one;
        transform.GetComponentInChildren<RobotEyes>().transform.position = transform.GetComponentInChildren<RobotEyes>().eyePos.position;

        // SetContent(content);
        HideImmediate();

        dialogBoxByBut.gameObject.SetActive(true);

        but_L.onClick.RemoveAllListeners();
        but_R.onClick.RemoveAllListeners();
        but_XHasBut.onClick.RemoveAllListeners();

        but_L.onClick.AddListener(HideImmediateByBut);
        but_R.onClick.AddListener(HideImmediateByBut);
        but_L.onClick.AddListener(() => TalkWithMod.Instance.Stop());
        but_R.onClick.AddListener(() => TalkWithMod.Instance.Stop());
        but_XHasBut.onClick.AddListener(HideImmediateByBut);
        if (actL != null) but_L.onClick.AddListener(actL);
        if (actR != null) but_R.onClick.AddListener(actR);
        but_XHasBut.onClick.AddListener(actR);

        but_L.GetComponentInChildren<Text>().text = lContent;
        but_R.GetComponentInChildren<Text>().text = rContent;
        but_L.gameObject.SetActive(false);
        but_R.gameObject.SetActive(false);

        popCor = StartCoroutine(nameof(PoppingByBut));

    }

    private IEnumerator PoppingByBut()
    {
        contentByButton.text = "";

        while (dialogBoxByBut.transform.localScale.x < 1)
        {
            dialogBoxByBut.transform.localScale += new Vector3(popSpeed, popSpeed, popSpeed) * Time.deltaTime;
            yield return null;
        }

        dialogBoxByBut.transform.localScale = Vector3.one;
        isShown = true;

        but_L.gameObject.SetActive(true);
        but_R.gameObject.SetActive(true);

        yield return displayCor = StartCoroutine(DisplayContentByBut());

        yield break;
    }

    /// <summary>
    /// ���ֹ���(����ť��)
    /// </summary>
    private IEnumerator DisplayContentByBut()
    {
        foreach (char letter in currentContent)
        {
            contentByButton.text += letter;

            yield return new WaitForSeconds(1.0f / (currentContent.Length / displayTime));
        }

        yield break;
    }

    public void HideImmediateByBut()
    {
        if (popCor != null) StopCoroutine(popCor);
        if (displayCor != null) StopCoroutine(displayCor);
        if (hideCor != null) StopCoroutine(hideCor);

        dialogBoxByBut.transform.localScale = Vector3.zero;
        isShown = false;
    }
    #endregion

    #region NoButton
    /// <summary>
    /// ShowTip
    /// </summary>
    /// <param name="s">����</param>
    /// <param name="name">��Ƶ��</param>
    /// <param name="isSpeak">�Ƿ������Ƶ</param>
    /// <returns></returns>
    public float ShowTips(string s, string name = "", bool isSpeak = true)
    {
        transform.localScale = Vector3.one;
        transform.GetComponentInChildren<RobotEyes>().transform.position = transform.GetComponentInChildren<RobotEyes>().eyePos.position;

        // gameObject.SetActive(true);
        HideImmediateByBut();

        if (string.IsNullOrWhiteSpace(name)) name = s;
        AudioClip clip = Resources.Load<AudioClip>("Audios/" + name);
        if (clip == null)
        {
            Debug.Log($"<color=red>δ�ҵ���Ƶ��</color>{name}");
            if (isSpeak)
                Show($"<color=red>δ�ҵ���Ƶ��</color>{name}");
            else
                Show(name);
            return 2;
        }
        else
        {
            TalkWithMod.Instance.Play(clip);

            Show(name);

            return clip.length;
        }
    }

    public void Show(string txt)
    {
        SetContent(txt);

        Show();
    }

    public void HideTips()
    {
        if (gameObject.activeInHierarchy)
        {
            Hide();
        }
        TalkWithMod.Instance.Stop();
    }

    public void SetActive(bool active)
    {
        TalkWithMod.Instance.Stop();
        gameObject.SetActive(active);
    }

    private void Show()
    {
        if (popCor != null) StopCoroutine(popCor);
        if (displayCor != null) StopCoroutine(displayCor);
        popCor = StartCoroutine(nameof(Popping));
        if (hideCor != null) StopCoroutine(hideCor);
    }

    private void Hide()
    {
        if (popCor != null) StopCoroutine(popCor);
        if (displayCor != null) StopCoroutine(displayCor);
        popCor = StartCoroutine(nameof(Hiding));
    }

    private IEnumerator Popping()
    {
        content.text = "";

        while (dialogBox.transform.localScale.x < 1)
        {
            dialogBox.transform.localScale += new Vector3(popSpeed, popSpeed, popSpeed) * Time.deltaTime;
            yield return null;
        }

        dialogBox.transform.localScale = Vector3.one;
        isShown = true;

        yield return displayCor = StartCoroutine(DisplayContent());

        yield break;
    }

    /// <summary>
    /// ���ֹ���
    /// </summary>
    private IEnumerator DisplayContent()
    {
        foreach (char letter in currentContent)
        {
            content.text += letter;

            yield return new WaitForSeconds(1.0f / (currentContent.Length / displayTime));
        }

        yield break;
    }

    private IEnumerator Hiding()
    {
        while (dialogBox.transform.localScale.x > 0)
        {
            dialogBox.transform.localScale -= new Vector3(popSpeed, popSpeed, popSpeed) * Time.deltaTime;
            yield return null;
        }
        dialogBox.transform.localScale = Vector3.zero;
        isShown = false;
        yield break;
    }

    private IEnumerator InvokeHide(float time)
    {
        yield return new WaitForSeconds(time);
        Hide();
    }

    /// <summary>
    /// ��������dialog
    /// </summary>
    public void HideImmediate()
    {
        if (popCor != null) StopCoroutine(popCor);
        if (displayCor != null) StopCoroutine(displayCor);
        if (hideCor != null) StopCoroutine(hideCor);

        dialogBox.transform.localScale = Vector3.zero;
        isShown = false;
    }

    #endregion

    public void HideAllDialog()
    {
        HideImmediate();
        HideImmediateByBut();
    }

    private void SetContent(string txt)
    {
        currentContent = txt;

#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
    }
}
