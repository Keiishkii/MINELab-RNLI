using UnityEngine;
using UnityEngine.UIElements;

public partial class ExperimentUXML : MonoBehaviour
{
    #region [ UXML ]
    private UIDocument _uiDocument;
    private VisualElement _root;
    #endregion
    
    

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        QueryUIDocument();
    }

    private void QueryUIDocument()
    {
        _root = _uiDocument.rootVisualElement;
        
        instructionContainer = new InstructionContainer(this, _root);
        tutorialAVScaleContainer = new TutorialAVScaleContainer(this, _root);
        videoPlayerContainer = new VideoPlayerContainer(this, _root);
        calibrationContainer = new CalibrationContainer(this, _root);
        timerContainer = new TimerContainer(this, _root);
    }
}
