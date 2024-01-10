using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using System.Linq;

public class SignTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI mainText;
    [SerializeField] private Transform signParent;

    [SerializeField] private Color32 hiddenColor = Color.white;
    [SerializeField] private Color32 activeColor = Color.white;

    [SerializeField] private float timeBetweenLetters;
    [SerializeField] private float timeUntilRemove;
    [SerializeField] private float timeBetweenLettersHiding;
    [SerializeField] private float timeBetweenFonts;

    [SerializeField] private bool SingleTextTyping;

    private bool startedCoroutine = false;

    private IEnumerator mainCoroutine;

    [SerializeField] private TextMeshProUGUI signText;

    Color32[] mainSignColors;
    // Start is called before the first frame update
    void Start()
    {
        if (SingleTextTyping) signText.gameObject.SetActive(false);
        //signText = Instantiate(mainText.gameObject, transform).GetComponent<TextMeshProUGUI>();
        //signText.transform.localPosition = signParent.localPosition;
        //signText.font = GameManager.Instance.GetAlienFontAsset();
        //signText.fontSize = 34.5f;
        signText.text = mainText.text;
        mainText.ForceMeshUpdate();
        TMP_TextInfo textInfo = mainText.textInfo;
        Color32[] newVertexColors;
        for (int i = 0; i < textInfo.characterCount;i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            newVertexColors[vertexIndex + 0].a = 0;
            newVertexColors[vertexIndex + 1].a = 0;
            newVertexColors[vertexIndex + 2].a = 0;
            newVertexColors[vertexIndex + 3].a = 0;

            newVertexColors[vertexIndex + 0] = (Color)newVertexColors[vertexIndex + 0] * activeColor;
            newVertexColors[vertexIndex + 1] = (Color)newVertexColors[vertexIndex + 1] * activeColor;
            newVertexColors[vertexIndex + 2] = (Color)newVertexColors[vertexIndex + 2] * activeColor;
            newVertexColors[vertexIndex + 3] = (Color)newVertexColors[vertexIndex + 3] * activeColor;

            mainText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
        mainText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
        signText.ForceMeshUpdate();
        TMP_TextInfo signTextInfo = signText.textInfo;
        Color32[] newVertexColors2;
        for (int i = 0; i < signTextInfo.characterCount; i++)
        {
            if (!signTextInfo.characterInfo[i].isVisible) continue;
            int materialIndex = signTextInfo.characterInfo[i].materialReferenceIndex;
            newVertexColors2 = signTextInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = signTextInfo.characterInfo[i].vertexIndex;
            newVertexColors2[vertexIndex + 0].a = 0;
            newVertexColors2[vertexIndex + 1].a = 0;
            newVertexColors2[vertexIndex + 2].a = 0;
            newVertexColors2[vertexIndex + 3].a = 0;

            newVertexColors2[vertexIndex + 0] = (Color)newVertexColors2[vertexIndex + 0] * activeColor;
            newVertexColors2[vertexIndex + 1] = (Color)newVertexColors2[vertexIndex + 1] * activeColor;
            newVertexColors2[vertexIndex + 2] = (Color)newVertexColors2[vertexIndex + 2] * activeColor;
            newVertexColors2[vertexIndex + 3] = (Color)newVertexColors2[vertexIndex + 3] * activeColor;

            signText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
        //Debug.Log("Makes it to the end");
        signText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
    }

    private IEnumerator ChangeAlphaOneLetterAtATime(float time, TextMeshProUGUI text, int alpha)
    {
        //text.ForceMeshUpdate();
        TMP_TextInfo textInfo = text.textInfo;
        Color32[] newVertexColors;
        for (int i = 0; i < textInfo.characterCount;i++)
        {
            if (!textInfo.characterInfo[i].isVisible) continue;
            yield return new WaitForSeconds(time);
            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            newVertexColors = textInfo.meshInfo[materialIndex].colors32;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            newVertexColors[vertexIndex + 0].a = (byte)alpha;
            newVertexColors[vertexIndex + 1].a = (byte)alpha;
            newVertexColors[vertexIndex + 2].a = (byte)alpha;
            newVertexColors[vertexIndex + 3].a = (byte)alpha;

            newVertexColors[vertexIndex + 0] = (Color)newVertexColors[vertexIndex + 0] * activeColor;
            newVertexColors[vertexIndex + 1] = (Color)newVertexColors[vertexIndex + 1] * activeColor;
            newVertexColors[vertexIndex + 2] = (Color)newVertexColors[vertexIndex + 2] * activeColor;
            newVertexColors[vertexIndex + 3] = (Color)newVertexColors[vertexIndex + 3] * activeColor;

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
        yield return null;
    }

    private IEnumerator MainCoroutine(float timeForFontToChange, float timeBetweenLetters, float timeUntilRemove, float timeBetweenLettersHide, TextMeshProUGUI mainText, TextMeshProUGUI alienText)
    {
        StartCoroutine(ChangeAlphaOneLetterAtATime(timeBetweenLetters, alienText, 255));
        yield return new WaitForSeconds(timeForFontToChange);
        StartCoroutine(ChangeAlphaOneLetterAtATime(timeBetweenLetters, mainText, 255));
        StartCoroutine(ChangeAlphaOneLetterAtATime(timeBetweenLetters, alienText, 0));
        yield return new WaitForSeconds(timeUntilRemove + (mainText.text.Count(c => !char.IsWhiteSpace(c)) * timeBetweenLetters) * 2);
        StartCoroutine(ChangeAlphaOneLetterAtATime(timeBetweenLettersHide, mainText, 0));
        //startedCoroutine = false;
    }

    private IEnumerator ShowAndHideSingleFont(float timeUntilRemove, float timeBetweenLetters, float timeBetweenLettersHide, TextMeshProUGUI mainText)
    {
        StartCoroutine(ChangeAlphaOneLetterAtATime(timeBetweenLetters, mainText, 255));
        yield return new WaitForSeconds(timeUntilRemove + mainText.text.Count(c=>!char.IsWhiteSpace(c)) * timeBetweenLetters);
        StartCoroutine(ChangeAlphaOneLetterAtATime(timeBetweenLettersHide, mainText, 0));

    }

    // Update is called once per frame
    void Update()
    { 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (!startedCoroutine)
        {
            startedCoroutine = true;
            if (SingleTextTyping)
                mainCoroutine = ShowAndHideSingleFont(timeUntilRemove, timeBetweenLetters, timeBetweenLettersHiding, mainText);
            else
                mainCoroutine = MainCoroutine(timeBetweenFonts, timeBetweenLetters, timeUntilRemove, timeBetweenLettersHiding, mainText, signText);
            StartCoroutine(mainCoroutine);
        }
    }
}
