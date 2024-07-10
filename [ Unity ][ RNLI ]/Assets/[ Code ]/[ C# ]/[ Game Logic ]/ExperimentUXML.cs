using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ExperimentUXML : MonoBehaviour
{
    #region [ UXML ]
    private UIDocument _uiDocument;
    private VisualElement _root;
    private VisualElement _videoPlayerVisualElement;
    private VisualElement _introductionVisualElement;
    private VisualElement _endVisualElement;
    #endregion

    #region [ Unserialised Fields ]
    public float VideoPlayerVisualElementAlpha
    {
        get => _videoPlayerVisualElement.style.opacity.value;
        set => _videoPlayerVisualElement.style.opacity = value;
    }
    
    public float IntroductionVisualElementAlpha
    {
        get => _introductionVisualElement.style.opacity.value;
        set => _introductionVisualElement.style.opacity = value;
    }
    
    public float EndVisualElementAlpha
    {
        get => _endVisualElement.style.opacity.value;
        set => _endVisualElement.style.opacity = value;
    }
    #endregion

    

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        QueryUIDocument();
        InitialiseUI();
    }

    private void QueryUIDocument()
    {
        _root = _uiDocument.rootVisualElement;
        
        _videoPlayerVisualElement = _root.Q<VisualElement>("VideoPlayerVisualElement");
        _introductionVisualElement = _root.Q<VisualElement>("IntroductionContainer");
        _endVisualElement = _root.Q<VisualElement>("EndContainer");
    }

    private void InitialiseUI()
    {
        HideIntroductionVisualElement();
        HideVideoPlayerVisualElement();
        HideEndVisualElement();
    }

    public void ShowIntroductionVisualElement() => IntroductionVisualElementAlpha = 1;
    public void HideIntroductionVisualElement() => IntroductionVisualElementAlpha = 0;
    
    public void ShowVideoPlayerVisualElement() => VideoPlayerVisualElementAlpha = 1;
    public void HideVideoPlayerVisualElement() => VideoPlayerVisualElementAlpha = 0;
    
    public void ShowEndVisualElement() => EndVisualElementAlpha = 1;
    public void HideEndVisualElement() => EndVisualElementAlpha = 0;
}
