using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class ModePanelManager : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI _modeText;
            public List<string> modeName;

            public void SetModeText(int p_mode){
                _modeText.text = modeName[p_mode];
            }

        }
    }
}
