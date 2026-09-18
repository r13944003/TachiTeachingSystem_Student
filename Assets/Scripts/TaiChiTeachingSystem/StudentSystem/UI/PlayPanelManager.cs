using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class PlayPanelManager : MonoBehaviour
        {
            [Header("===Load Button===")]
            [SerializeField] private GameObject _loadModifiedDataPanel;
            [SerializeField] private TMP_Dropdown _modifiedDataFileDropdown;


            [Header("===Play Button===")]
            [SerializeField] private Image _playImage;
            [SerializeField] private Image _pauseImage;
            [SerializeField] private TextMeshProUGUI _playButtonTooltipText;
            [SerializeField] private Button _playButton;

            [Header("===Avatar Number Button===")]
            [SerializeField] private Image _singleAvatarImage;
            [SerializeField] private Image _duoAvatarImage;
            [SerializeField] private TextMeshProUGUI _avatarNumberButtonTooltipText;


            [Header("===Setting===")]
            [SerializeField] private Toggle _indicatorToggle;
            [SerializeField] private Toggle _originPoseToggle;
            [SerializeField] private Toggle _modifyPoseToggle;
            

            ////////////////////////////////////////////////////////////////////////////
            /////////////  Set the dropdown of the motionData list /////////////////////
            ////////////////////////////////////////////////////////////////////////////
            public void SetLoadModifiedDataPanelActive(bool p_active){
                _loadModifiedDataPanel.SetActive(p_active);
            }
            public void SetModifiedDataFileDropdown(List<MotionRecord> p_motionRecordList){
                _modifiedDataFileDropdown.ClearOptions();

                List<TMP_Dropdown.OptionData> optionDataList = new();
                foreach(MotionRecord record in p_motionRecordList){
                    TMP_Dropdown.OptionData optionData = new TMP_Dropdown.OptionData(){
                        text = $"From CoachId: {record.coachId} / Filename: {record.fileName}"
                    };
                    optionDataList.Add(optionData);
                }
                _modifiedDataFileDropdown.AddOptions(optionDataList);
            }
            public string GetModifiedDataFilename(){
                string fileInfoText = _modifiedDataFileDropdown.options[_modifiedDataFileDropdown.value].text;
                string filename = fileInfoText.Split('/')[1].Split(':')[1].Trim();
                return filename;
            }

            ///////////////////////////////////////////////
            ///////////// Play Button /////////////////////
            ///////////////////////////////////////////////
            public void SetPlayButton(bool p_onPlay){
                _SetPlayButtonImage(p_onPlay);
                _SetPlayButtonTooltip(p_onPlay);
            }

            private void _SetPlayButtonImage(bool p_onPlay){
                _playImage.enabled = !p_onPlay;
                _pauseImage.enabled = p_onPlay;
            }

            private void _SetPlayButtonTooltip(bool p_onPlay){
                _playButtonTooltipText.text = p_onPlay ? "Pause" : "Play";
                
            }

            public void SetPlayButtonInteractible(bool p_interactable){
                _playButton.interactable = p_interactable;
            }

            //////////////////////////////////////////////////////////////
            ///////////// Single Avatar Mode Button /////////////////////
            /////////////////////////////////////////////////////////////
            public void SetSingleAvatarModeButton(bool p_singleAvatarMode){
                _SetAvatarNumberButtonImage(p_singleAvatarMode);
                _SetAvatarNumberButtonTooltip(p_singleAvatarMode);
            }

            private void _SetAvatarNumberButtonImage(bool p_singleAvatarMode){
                _singleAvatarImage.enabled = p_singleAvatarMode;
                _duoAvatarImage.enabled = !p_singleAvatarMode;
            }

            private void _SetAvatarNumberButtonTooltip(bool p_singleAvatarMode){
                _avatarNumberButtonTooltipText.text = p_singleAvatarMode ? "Single Avatar" : "Duo Avatar";    
            }


            ///////////////////////////////////////////////
            ///////////// Play Panel /////////////////////
            ///////////////////////////////////////////////

            public void SetPanelActive(bool p_active){
                this.transform.gameObject.SetActive(p_active);
            }

            ///////////////////////////////////////////////////
            ///////////// Indicator Toggle /////////////////////
            ///////////////////////////////////////////////////
            public bool GetIndicatorToggleIsOn(){
                return _indicatorToggle.isOn;
            }

            ///////////////////////////////////////////////////
            ///////////// Avatar Pose Toggle /////////////////////
            ///////////////////////////////////////////////////
            public bool GetOriginPoseToggleIsOn(){
                return _originPoseToggle.isOn;
            }
            public void SetSingleAvatarPoseToggleInteractable(bool p_singleAvatarMode){
                _originPoseToggle.interactable = p_singleAvatarMode;
                _modifyPoseToggle.interactable = p_singleAvatarMode;
            }
        }
    }
}