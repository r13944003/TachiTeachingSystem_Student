using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class FaceCamera : MonoBehaviour
        {

            // Make the object always face the camera(For nametag of each avatar)
            [SerializeField] private Camera _mainCamera;

            // Update is called once per frame
            void Update()
            {
                this.transform.LookAt(_mainCamera.transform);
            }
            

            
        }
    }
}
