using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class RecordPanelManager : MonoBehaviour
        {

            [Header("===Record Button===")]
            [SerializeField] private Image _recordImage;
            [SerializeField] private Image _stopRecordImage;
            [SerializeField] private TextMeshProUGUI _recordButtonTooltipText;
            [SerializeField] private GameObject _recordCountdownPanel;
            [SerializeField] private TextMeshProUGUI _recordCountdownText;

            [Header("===record Buffer===")]
            [SerializeField] private GameObject _recordDataBufferPanel;
            [SerializeField] private TextMeshProUGUI _recordDataBufferInfoText;

            [Header("===Upload Record Data Button===")]
            [SerializeField] private GameObject _uploadRecordDataPanel;
            [SerializeField] private TMP_Dropdown _coachIdDropdown;
            [SerializeField] private GameObject _uploadResultPanel;
            [SerializeField] private TextMeshProUGUI _uploadResultText;
            

            //////////////////////////////////////////////////////
            ////////////////   Record Button  //////////////////
            /////////////////////////////////////////////////////
            public void SetRecordingButton(bool p_onRecording){
                _SetRecordButtonImage(p_onRecording);
                _SetRecordButtonTooltip(p_onRecording);
            }
            private void _SetRecordButtonImage(bool p_onRecording){
                _recordImage.enabled = !p_onRecording;
                _stopRecordImage.enabled = p_onRecording;
            }

            private void _SetRecordButtonTooltip(bool p_onRecording){
                _recordButtonTooltipText.text = p_onRecording ? "Stop Recording" : "Start Recording";
                
            }

            public void SetRecordCountdownPanelActive(bool p_active){
                _recordCountdownPanel.SetActive(p_active);
            }

            public void SetRecordCountdownText(int p_countdown){
                if(p_countdown > 0)
                    _recordCountdownText.text = $"Countdown: {p_countdown}...";
                else
                    _recordCountdownText.text = $"Start Recording!!";
            }


            //////////////////////////////////////////////////////
            ////////////////   Record Panel  ////////////////////
            /////////////////////////////////////////////////////
            public void SetPanelActive(bool p_active){
                this.transform.gameObject.SetActive(p_active);
            }

            //////////////////////////////////////////////////////
            ////////////////   Record Data Buffer  ///////////////
            /////////////////////////////////////////////////////
            public void SetRecordDataBufferActive(bool p_active, int p_dataLength){
                _recordDataBufferPanel.SetActive(p_active);
                if(p_active){ 
                    _recordDataBufferInfoText.text = "Length: " + p_dataLength;
                }
            }

            //////////////////////////////////////////////////////
            ////////////////   Upload Button  //////////////////
            /////////////////////////////////////////////////////

            public void SetUploadRecordDataPanelActive(bool p_active){
                _uploadRecordDataPanel.SetActive(p_active);
            }

            public void SetCoachIdDropdown(List<CoachData> p_coachData){
                _coachIdDropdown.ClearOptions();

                List<string> optionList = new();
                foreach(CoachData data in p_coachData){
                    string option = $"CoachId: {data.coachId} / Skill: {data.skill}";
                    optionList.Add(option);
                }
                _coachIdDropdown.AddOptions(optionList);
            }
            public int GetCoachId(){
                string coachIdInfoText = _coachIdDropdown.options[_coachIdDropdown.value].text;
                int coachId = int.Parse(coachIdInfoText.Split('/')[0].Split(':')[1].Trim());
                return coachId;
            }

            public void SetUploadResultPanelActive(bool p_active){
                _uploadResultPanel.SetActive(p_active);
            }
            public void SetUploadResultText(bool p_success){
                if(p_success)
                    _uploadResultText.text = "Upload Success!";
                else
                    _uploadResultText.text = "Upload Failed!";
            }
        }
    }
}