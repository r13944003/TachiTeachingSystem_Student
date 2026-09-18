using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TaichiTeachingSystem{
    namespace StudentSystem{

        // Decide user avatar in LoginScene
        public class AvatarSelection : MonoBehaviour
        {
            private int _avatarId=0;
            [SerializeField] private List<GameObject> _avatars;
            [SerializeField] private SceneLoader _sceneLoader;

            public void NextAvatar(){
                _avatars[_avatarId].SetActive(false);
                _avatarId = (_avatarId+1)%_avatars.Count;
                _avatars[_avatarId].SetActive(true);
            }
            public void PrevAvatar(){
                _avatars[_avatarId].SetActive(false);
                _avatarId = (_avatarId-1+_avatars.Count)%_avatars.Count;
                _avatars[_avatarId].SetActive(true);
            }
            public void ConfirmAvatar(){
                PlayerPrefs.SetInt("AvatarId", _avatarId);
                _sceneLoader.LoadMainScene();
            }
        }
    }
}
