using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class StudentPanelManager : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI _avatarModeText;
            [SerializeField] private Button _avatarModePrevButton;
            [SerializeField] private Button _avatarModeNextButton;
            public List<string> avatarModeName;

            public void SetAvatarModeText(int p_avatarMode){
                _avatarModeText.text = avatarModeName[p_avatarMode];
            }
            public void SetAvatarModeButtonInteractable(bool p_interactable){
                _avatarModePrevButton.interactable = p_interactable;
                _avatarModeNextButton.interactable = p_interactable;
            }
        }
    }
}