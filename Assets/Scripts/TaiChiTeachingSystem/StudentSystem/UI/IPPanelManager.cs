using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class IPPanelManager : MonoBehaviour
        {
            [SerializeField] private GameObject _ipPanel;
            [SerializeField] private TMP_InputField _ipInputField;

            public string GetIPInputField(){
                return _ipInputField.text;
            }
            public void SetIPInputField(string p_IP){
                _ipInputField.text = p_IP;
            }
            public void SetIPPanelActive(bool p_active){
                _ipPanel.SetActive(p_active);
            }
        }
    }
}