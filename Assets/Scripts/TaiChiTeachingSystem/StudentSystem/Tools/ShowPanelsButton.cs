using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TaichiTeachingSystem{
    public class ShowPanelsButton : MonoBehaviour
    {
        [SerializeField] private GameObject _panels;
        [SerializeField] private Image _buttonImage;
        private bool _showPanels;
        void Start(){
            _showPanels = true;
        }
        // Show or hide UI Panels
        public void SwitchShowPanels(){
            _showPanels = !_showPanels;
            _panels.SetActive(_showPanels);
            _buttonImage.transform.Rotate(0, 0, 180);
        }
    }
}
