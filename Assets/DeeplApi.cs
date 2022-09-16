using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DeeplApi : MonoBehaviour
{
    //Variablen VoiceInput und Translation als Output
    public Text VoiceInput; // gesprochener Text
    public Text TranslationOutput; //übersetzter Text

    //Variablen SpeechToText
    public bool IsActivelyListening;
    public bool HasStoppedDictating;
    public string TextCache; // gesprochener Text als string
    private string m_Recognitions;
    private DictationRecognizer m_DictationRecognizer;


    void Start()
    {
        IsActivelyListening = false;
        HasStoppedDictating = false;
        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += (text, confidence) => {
            TextCache += " " + text;
            IsActivelyListening = false;
        };

        m_DictationRecognizer.DictationHypothesis += (text) => {
            m_Recognitions = TextCache + " " + text;
            VoiceInput.text = TextCache + " " + text;
            IsActivelyListening = true;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) => {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            m_DictationRecognizer.Stop();
        };

        m_DictationRecognizer.DictationError += (error, hresult) => {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        Invoke("StartDictation", 0.1f);

    }

    public void StartDictation()
    {

        if (IsActivelyListening == false) m_DictationRecognizer.Start();
        Debug.Log("StartedDictation");

    }

    public void OnClickEnter()
    {
        StartCoroutine(PostTranslate());
    }

    IEnumerator PostTranslate()
    {
        // past in the correct values from api - to connect with
        WWWForm form = new WWWForm();
        form.AddField("auth_key", "write_your_authorization_key_here");
        form.AddField("target_lang", "EN");//In welche Sprache übersetzt werden soll
        // Text wird dann mit TEXT TranslationOutput verknüpft, überbrückend, als TextCache (string)

        form.AddField("text", TextCache); //TextCache
        Debug.Log(TextCache);
        

        UnityWebRequest www = UnityWebRequest.Post("https://api-free.deepl.com/v2/translate", form);
        yield return www.SendWebRequest();


        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Antwortet auf die Anfrage mit der Übersetzung
            // Debug.Log(www.downloadHandler.text); //zeigt Übersetzung in der Konsole
            TranslationOutput.text = www.downloadHandler.text;

        }
    }

    

    public void StopDictation()
    {
        if (IsActivelyListening == false)
        {
            m_DictationRecognizer.Stop();
            HasStoppedDictating = true;
            Debug.Log("StoppedDictation");
        }
    }
}