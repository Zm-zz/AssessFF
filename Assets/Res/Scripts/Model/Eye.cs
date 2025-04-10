using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    public SkinnedMeshRenderer skinned;
    private float transitionDuration = 0.2f;
    private float elapsedTime;
    private float blendShapeWeightWeight = 0f;
    private bool isInscreasing = true;
    private bool shouldStop = false;

    public int blinkIndex;

    private Coroutine cor_գ��;

    private void OnEnable()
    {

        cor_գ�� = StartCoroutine(ChangeBlendShapeWeight());
    }

    private void OnDisable()
    {
        if (cor_գ�� != null) StopCoroutine(cor_գ��);
    }

    /// <summary>
    /// �����۾����գ�����ʱ����ϣ�
    /// .ControlEye(true)���ܻ����
    /// </summary>
    /// <param name="open"></param>
    public void ControlEye(bool open)
    {
        if (open)
        {
            shouldStop = false;
            if (cor_գ�� != null) StopCoroutine(cor_գ��);
            skinned.SetBlendShapeWeight(blinkIndex, 0);
            StartCoroutine(ChangeBlendShapeWeight());
        }
        else
        {
            shouldStop = true;
            StopCoroutine(cor_գ��);

            Invoke("CloseEye", 0.4f);
        }
    }

    private void CloseEye()
    {
        skinned.SetBlendShapeWeight(blinkIndex, 100);
    }

    IEnumerator ChangeBlendShapeWeight()
    {

        float elapsedTime = 0f;
        while (true)
        {
            if (shouldStop)
            {
                yield break;
            }


            if (isInscreasing)
            {
                if (shouldStop)
                {
                    yield break;
                }
                while (elapsedTime < transitionDuration && isInscreasing)
                {
                    if (shouldStop)
                    {
                        yield break;
                    }

                    yield return

                    elapsedTime += Time.deltaTime;
                    blendShapeWeightWeight = Mathf.Lerp(0, 100, elapsedTime / transitionDuration);
                    skinned.SetBlendShapeWeight(blinkIndex, blendShapeWeightWeight);
                    yield return null;
                }

                if (blendShapeWeightWeight >= 100)
                {
                    isInscreasing = false;
                    elapsedTime = 0f; // ����ʱ���Ա���һ��Lerp

                    // �ȴ�����
                    //yield return new WaitForSeconds(2f);

                    // ������ʼȨ��Ϊ100��׼������
                    blendShapeWeightWeight = 100f;
                }

            }
            else
            {
                while (elapsedTime < transitionDuration && !isInscreasing)
                {
                    if (shouldStop)
                    {
                        yield break;
                    }

                    elapsedTime += Time.deltaTime;
                    blendShapeWeightWeight = Mathf.Lerp(100, 0, elapsedTime / transitionDuration);
                    skinned.SetBlendShapeWeight(blinkIndex, blendShapeWeightWeight);
                    yield return null;
                }

                if (blendShapeWeightWeight <= 0)
                {
                    isInscreasing = true;
                    elapsedTime = 0f; // ����ʱ���Ա���һ��Lerp

                    float time = Random.Range(3, 5);
                    yield return new WaitForSeconds(time);

                    if (shouldStop)
                    {
                        yield break;
                    }
                }
            }




        }

    }

    private void OnDestroy()
    {
        if (cor_գ�� != null) StopCoroutine(cor_գ��);
    }
}


