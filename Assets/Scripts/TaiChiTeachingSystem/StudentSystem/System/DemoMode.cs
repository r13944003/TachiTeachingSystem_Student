using System;
using System.Collections;
using Mocopi.Receiver;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class DemoMode : MonoBehaviour
        {
            [SerializeField] private AvatarManagerForDemo _avatarManager;
            [SerializeField] private StudentPanelManager _studentPanelManager;
            [SerializeField] private CoachManagerForDemo _coachManager;
            [SerializeField] private MocopiSimpleReceiver _mocopiSimpleReceiver;

            private int _studentAvatarMode;

            void Awake(){
                _avatarManager.InstantiateRecordModeAvatars();

                // Set the first record avatar as the mocopi receiver
                GameObject avatar = _avatarManager.GetFirstRecordAvatar();
                avatar.AddComponent<MocopiAvatar>();
                _mocopiSimpleReceiver.AvatarSettings[0].MocopiAvatar = avatar.GetComponent<MocopiAvatar>();
                _mocopiSimpleReceiver.enabled = true;
            }

            private void Start(){
                // Avatars
                _avatarManager.EnterRecordMode();
                //Coach
                _coachManager.EnterRecordMode();
                //Set default StudentAvatarMode to "Eight Avatar"
                _studentAvatarMode = 1;
                _studentPanelManager.SetAvatarModeButtonInteractable(true);

            }
            private void Update(){
                // Do nothing if in no student avatar mode
                if(_studentAvatarMode == 0)
                    return;

                // Copy avatar pose to other 7 non-mocopi avatars if in eight student avatar mode
                if(_studentAvatarMode == 1)
                    _avatarManager.SetRecordAvatarsPose();

                // Play or Stop Coach
                if(OVRInput.GetDown(OVRInput.Button.One)){
                    _coachManager.ChangePlayState();
                }

            }

            // Change Student(user) avatar number (0, 8)
            public void SetStudentAvatarMode(int p_modeChange){
                _studentAvatarMode = (_studentAvatarMode + p_modeChange + 2)%2;
                _studentPanelManager.SetAvatarModeText(_studentAvatarMode);
                _avatarManager.SetRecordAvatarNumber(_studentAvatarMode);
                _coachManager.SetCoachAvatarsPosition(_studentAvatarMode);
                _coachManager.SetTaichiStartMove(0);
                _coachManager.Stop();
            }

        }
    }
}
