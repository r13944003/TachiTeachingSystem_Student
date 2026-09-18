using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class LoginPanelManager : MonoBehaviour
        {
            [SerializeField] private GameObject _loginPanel;
            [SerializeField] private TMP_InputField _emailInputField;
            [SerializeField] private TMP_InputField _passwordInputField;
            [SerializeField] private Toggle _rememberMeToggle;

            [Header("===Error Message===")]
            [SerializeField] private GameObject _errorMsgPanel;
            [SerializeField] private TextMeshProUGUI _errorMsgText;

            public string GetEmail(){
                return _emailInputField.text;
            }
            public string GetPassword(){
                return _passwordInputField.text;
            }
            public void SetLoginPanelActive(bool p_active){
                _loginPanel.SetActive(p_active);
            }

            public void SetErrorMsgPanelActive(bool p_active){
                _errorMsgPanel.SetActive(p_active);
            }
            public void SetErrorMsgText(string p_errorMsg){
                _errorMsgText.text = p_errorMsg;
            }
            
            public void SetEmailInputField(string p_email){
                _emailInputField.text = p_email;
            }

            public void SetPasswordInputField(string p_password){
                _passwordInputField.text = p_password;
            }
            public bool GetRememberMeToggle(){
                return _rememberMeToggle.isOn;
            }
            public void SetRememberMeToggle(bool p_isOn){
                _rememberMeToggle.isOn = p_isOn;
            }
        }
    }
}