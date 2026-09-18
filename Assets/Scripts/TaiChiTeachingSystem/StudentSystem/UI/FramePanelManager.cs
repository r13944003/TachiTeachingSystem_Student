using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaichiTeachingSystem{
    
    namespace StudentSystem{
        public class FramePanelManager : MonoBehaviour
        {
            [SerializeField] private Slider _fpsSlider;
            [SerializeField] private TextMeshProUGUI _frameNumberText;
            [SerializeField] private TextMeshProUGUI _fpsText;
            private float _standardFPS = 30;
            // Start is called before the first frame update
            void Start()
            {
                SetFrameNumberText(0);
                SetFPSText();
            }

            public void SetFrameNumberText(int p_frameNumber){
                _frameNumberText.text = "Frame Number: " + p_frameNumber;
            }

            public void SetFPSText(){
                float speed = _fpsSlider.value / _standardFPS;
                _fpsText.text = "Speed(FPS): " + speed.ToString("0.0") + "x(" + _fpsSlider.value + ")";
            }

            public float GetFPS(){
                return _fpsSlider.value;
            }
            public float GetSpeed(){
                return _fpsSlider.value / _standardFPS;
            }
            public void SetFPS(float p_fps){
                _fpsSlider.value = p_fps;
            }
            public void SetFpsSliderInteractable(bool p_interactable){
                _fpsSlider.interactable = p_interactable;
            }
        }
    }
}