using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;



public class XSpeech : MonoBehaviour
{
    //public button microphoneButton;
    //public GameObject MicrophoneButton;
    /* public List<LunarcomButtonController> microphoneButton;
     private bool isSelected = false;
     public Sprite Default;
     public Sprite Mic_Activated;
     private static LunarcomController lunarcomController;
     private Button button;
     public RecognitionMode speechRecognitionMode = RecognitionMode.Disabled;*/

    //  Reference to LanguateTranslatorSample - Set in the Inspector
    // [SerializeField]
    // private LangaugeTranslatorSample.LanguageTranslatorSample languageTranslatorSample;

    public bool IsActivelyListening;//wenn gesprochen wird, wird angezeigt, wann gleichzeitig activ zugehört wird
    public bool HasStoppedDictating;
    //public bool HasDictationRunning;
    public string TextCache;  //geändert auf public geändert : schreibt den gesprochenen text im inspector als string mit
    private string m_Recognitions;

    public Text m_Hypotheses; //zeigt gehörten Text an: ---> output text :
    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        //lunarcomController = LunarcomController.lunarcomController;
        //button = GetComponent<Button>();





        IsActivelyListening = false;
        HasStoppedDictating = false;
        //HasDictationRunning = false;
        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationResult += (text, confidence) => {
            //Debug.LogFormat("Dictation result: {0}", text);
            TextCache += " " + text;
            IsActivelyListening = false;
            //Debug.LogFormat("Result: IsActivelyListening {0}", IsActivelyListening);
        };

        m_DictationRecognizer.DictationHypothesis += (text) => {
            m_Recognitions = TextCache + " " + text;
            m_Hypotheses.text = TextCache + " " + text;
            IsActivelyListening = true;
            //Debug.LogFormat("Hypothesis: IsActivelyListening {0}", IsActivelyListening);
        };

        m_DictationRecognizer.DictationComplete += (completionCause) => {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogFormat("Dictation completed unsuccessfully: {0}.", completionCause);
            //HasDictationRunning = false;
            //StopCoroutine(TryToStartDictation());
            //StartDictation();
            m_DictationRecognizer.Stop();
        };

        m_DictationRecognizer.DictationError += (error, hresult) => {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        Invoke("StartDictation", 0.1f);

    }

    public void StartDictation()
    {
        //List<LunarcomButtonController> Mic_Activated = null;
        //if (Mic_Activated = this.GetComponent<SpriteRenderer>().sprite)//wenn sprite change to is Activated

        if (IsActivelyListening == false) m_DictationRecognizer.Start();
        Debug.Log("StartedDictation");

        //SENDE DIE INFORMATIONEN ZUM LANGUAGE TRANSLATOR SAMPLE SCRIPT
        // languageTranslatorSample.Translate(alt.transcript);

    }

    
    //button.image.sprite = Activated;
    //isSelected = true;
    // lunarcomController.SetActiveButton(GetComponent<LunarcomButtonController>());
    //wenn der microphone butto activ ist, dann höre zu
    // if (microphoneButton.isSelected = true)
    //{
    // if (IsActivelyListening == false) m_DictationRecognizer.Start();
    //   Debug.Log("StartedDictation");
    //}


    public void StopDictation()
    {
        if (IsActivelyListening == false)
        {
            m_DictationRecognizer.Stop();
            HasStoppedDictating = true;
            Debug.Log("StoppedDictation");

        }
    }

    /*public bool GetIsSelected()
    {
        return isSelected;
    }

    public void ShowNotSelected()
    {
        button.image.sprite = Default;
        isSelected = false;
    }

    public void DeselectButton()
    {
        ShowNotSelected();
        lunarcomController.SelectMode(RecognitionMode.Disabled);
    }
    */

    //Melissa, [28.07.21 15:38]
    //void WriteString() {
    //  File.WriteAllText(Application.dataPath + "/Output_" + System.DateTime.Now.ToString("yyyy/MM/dd") + "_" + System.DateTime.Now.ToString("hh") + "h." + System.DateTime.Now.ToString("mm") + "m." + System.DateTime.Now.ToString("ss") + "s.txt", m_Recognitions);// 
    //}
}