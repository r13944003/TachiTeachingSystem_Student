using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TaichiTeachingSystem{
    namespace StudentSystem{

        // Switch to LoginSceneForDemo or MainSceneForDemo
        public class SceneLoaderForDemo : MonoBehaviour
        {
            public void LoadMainScene(){
                SceneManager.LoadScene("MainSceneForDemo");
            }
            public void LoadLoginScene(){
                SceneManager.LoadScene("LoginSceneForDemo");
            }
        }
    }
}