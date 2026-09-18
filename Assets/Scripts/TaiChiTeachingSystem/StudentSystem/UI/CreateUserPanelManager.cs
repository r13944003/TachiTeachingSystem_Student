using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class CreateUserPanelManager : MonoBehaviour
        {
            [SerializeField] private GameObject _createUserPanel;
            [SerializeField] private TMP_InputField _usernameInputField;
            [SerializeField] private TMP_InputField _emailInputField;
            [SerializeField] private TMP_InputField _passwordInputField;


            public string GetUsername(){
                return _usernameInputField.text;
            }
            public string GetPassword(){
                return _passwordInputField.text;
            }
            public string GetEmail(){
                return _emailInputField.text;
            }
            public void SetCreateUserPanelActive(bool p_active){
                _createUserPanel.SetActive(p_active);
            }
        }
    }
}