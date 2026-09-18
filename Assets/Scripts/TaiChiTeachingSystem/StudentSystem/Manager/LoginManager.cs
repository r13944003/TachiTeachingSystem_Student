using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class LoginManager : MonoBehaviour
        {
            [SerializeField] private LoginPanelManager _loginPanelManager;
            [SerializeField] private CreateUserPanelManager _createUserPanelManager;
            [SerializeField] private IPPanelManager _ipPanelManager;
            [SerializeField] private GameObject _loginCanvas;
            [SerializeField] private GameObject _avatarSelection;
            private int _userId;
            private string _username;
            private string _password;
            private string _email;
            private string _accessToken;

            void Start(){
                _SetRememberMe();

                // Set serverIP if there's ip value stored in PlayerPrefs
                if(PlayerPrefs.HasKey("ServerIP"))
                    _ipPanelManager.SetIPInputField(PlayerPrefs.GetString("ServerIP"));
                SetServerIP();
            }

            /////////////////////////////////////////////////////////////////
            ////////// Remember the email and password //////////////////////
            /////////////////////////////////////////////////////////////////
            private void _SetRememberMe(){
                if (PlayerPrefs.HasKey("RememberMe") && PlayerPrefs.GetInt("RememberMe") == 1)
                {
                    _loginPanelManager.SetEmailInputField(PlayerPrefs.GetString("Email"));
                    _loginPanelManager.SetPasswordInputField(PlayerPrefs.GetString("Password"));
                    _loginPanelManager.SetRememberMeToggle(true);
                }
                else
                {
                    _loginPanelManager.SetRememberMeToggle(false);
                }
            }

            /////////////////////////////////////////////////////////////////
            ////////// Set Server IP ////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            public void SetServerIP(){
                string serverIP = _ipPanelManager.GetIPInputField();
                HttpService.SetBaseUrl(serverIP);
                PlayerPrefs.SetString("ServerIP", serverIP);
            }        


            /////////////////////////////////////////////////////////////////
            /////////////////     Login     /////////////////////////////////
            /////////////////////////////////////////////////////////////////
            public void Login(){
                StartCoroutine(_HandleLogin());
            }
            IEnumerator _HandleLogin(){
                _email = _loginPanelManager.GetEmail();
                _password = _loginPanelManager.GetPassword();
                yield return StartCoroutine(HttpService.GetUserAccessToken(_email, _password));
                _accessToken = HttpService.GetAccessToken();
                if(_accessToken == null){
                    _loginPanelManager.SetErrorMsgPanelActive(true);
                    _loginPanelManager.SetErrorMsgText("Login Failed!");
                }
                else{
                    // Save Email and Passwrd is remember me is set
                    if (_loginPanelManager.GetRememberMeToggle())
                    {
                        PlayerPrefs.SetString("Email", _email);
                        PlayerPrefs.SetString("Password", _password); // 請注意，這裡的密碼是明文保存，應該進行加密
                        PlayerPrefs.SetInt("RememberMe", 1);
                    }
                    else
                    {
                        // 清除之前保存的帳號和密碼
                        PlayerPrefs.DeleteKey("Email");
                        PlayerPrefs.DeleteKey("Password");
                        PlayerPrefs.SetInt("RememberMe", 0);
                    }
                    PlayerPrefs.Save();

                    // Get other user info from server and save in PlayerPrefs
                    yield return StartCoroutine(HttpService.Get_MeUser(_accessToken));
                    _userId = HttpService.GetUserId();
                    _username = HttpService.GetUsername();
                    PlayerPrefs.SetInt("UserId", _userId);
                    PlayerPrefs.SetString("AccessToken", _accessToken);
                    _loginCanvas.SetActive(false);
                    _avatarSelection.SetActive(true);

                }
            }

            /////////////////////////////////////////////////////////////////
            ///////////////    Create User Account   ////////////////////////
            /////////////////////////////////////////////////////////////////
            public void CreateUser(){
                StartCoroutine(_HandleCreateUser());
            }
            IEnumerator _HandleCreateUser(){
                _username = _createUserPanelManager.GetUsername();
                _email = _createUserPanelManager.GetEmail();
                _password = _createUserPanelManager.GetPassword();
                yield return StartCoroutine(HttpService.Post_CreateUser(_username, _email, _password));
                if(HttpService.IsCreateUserSuccess()){
                    Debug.Log("Create User Success");
                    _loginPanelManager.SetLoginPanelActive(true);
                    _createUserPanelManager.SetCreateUserPanelActive(false);
                }
                else{
                    Debug.LogError("Create User Failed");
                }

            }
            
        }
    }
}
