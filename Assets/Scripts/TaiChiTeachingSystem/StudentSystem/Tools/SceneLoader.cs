using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TaichiTeachingSystem{
    namespace StudentSystem{

        // Switch to LoginScene or MainScene
        public class SceneLoader : MonoBehaviour
        {
            public void LoadMainScene(){
                SceneManager.LoadScene("MainScene");
            }
            public void LoadLoginScene(){
                SceneManager.LoadScene("LoginScene");
            }
        }
    }
}