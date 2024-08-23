using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public partial class ExperimentUXML
{
    [Serializable]
    public abstract class IContainer
    {
        #region [ Unserialsed Fields ]
        protected ExperimentUXML _experimentUxml;
        protected readonly VisualElement _containerVisualElement;
        private const float _fadeDuration = 0.5f;
        private float Alpha
        {
            get => _containerVisualElement.style.opacity.value;
            set => _containerVisualElement.style.opacity = value;
        }
        #endregion
        
        protected IContainer(ExperimentUXML experimentUxml, VisualElement root, string containerVisualElementID)
        {
            _experimentUxml = experimentUxml;
            _containerVisualElement = root.Q<VisualElement>(containerVisualElementID);
            Hide();
        }
        
        public void Display() => Alpha = 1;
        public void Hide() => Alpha = 0;

        public virtual IEnumerator DisplayCoroutine()
        {
            for (float timeElapsed = 0; timeElapsed < _fadeDuration; timeElapsed += Time.deltaTime)
            {
                float weight = Mathf.InverseLerp(0, _fadeDuration, timeElapsed);
                Alpha = Mathf.Lerp(0, 1, weight);
                yield return null;
            }
            
            Alpha = 1;
        }
        
        public virtual IEnumerator HideCoroutine()
        {
            for (float timeElapsed = 0; timeElapsed < _fadeDuration; timeElapsed += Time.deltaTime)
            {
                float weight = Mathf.InverseLerp(0, _fadeDuration, timeElapsed);
                Alpha = Mathf.Lerp(1, 0, weight);
                yield return null;
            }
            
            Alpha = 0;
        }
    }

}