using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class tmplogin : MonoBehaviour
        {
            public LoginManager loginManager;
            public AvatarSelection avatarSelection;
            // Start is called before the first frame update
            void Start()
            {
                
            }

            // Update is called once per frame
            void Update()
            {
                if (Input.GetKeyDown("space"))
                {
                    loginManager.Login();
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    avatarSelection.ConfirmAvatar();
                }
            }
        }
    }
}
