using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;


namespace TaichiTeachingSystem{
    namespace StudentSystem{
        public class HttpService : MonoBehaviour
        {
            public static string base_url = "https://172.20.10.4:4433";

            // Setting ip of the server
            public static void SetBaseUrl(string p_ip){
                base_url = $"https://{p_ip}:4433";
            }
            private static string _GetApiUrl(string api_url)
            {
                return $"{base_url}{api_url}";
            }

            ////////////////////////////////////////////////
            ////////////////    Login    ///////////////////
            ////////////////////////////////////////////////
            private static string _accessToken;        
            private static string _GetAccessToken(UnityWebRequest rs)
            {
                string tokenJson = rs.downloadHandler.text;
                Token_response tokenData = JsonUtility.FromJson<Token_response>(tokenJson);
                if (tokenData == null || tokenData.access_token == null || tokenData.access_token.Length == 0)
                    Debug.LogError("Failed to parse access token from response.");
                Debug.Log("Access Token: " + tokenData.access_token);
                return tokenData.access_token;
            }
            
            public static IEnumerator GetUserAccessToken(string p_email, string p_password)
            {
                Debug.Log("GetToken");
                // 準備token請求的JSON數據
                var tokenRequestData = new token_request();
                tokenRequestData.email = p_email;
                tokenRequestData.password = p_password;
                
                // 取得token
                var tokenRequest = HttpTools.request_post( _GetApiUrl("/api/v1/token"), JsonUtility.ToJson(tokenRequestData) );
                yield return tokenRequest.SendWebRequest();
                UnityWebRequest.Result result =  HttpTools.check_respone(tokenRequest);
                if (result == UnityWebRequest.Result.Success)
                {
                    _accessToken = _GetAccessToken(tokenRequest);
                    yield break;
                }
                _accessToken = null;
                Debug.LogError("Error getting token: " + tokenRequest.error);
            }
            public static string GetAccessToken(){
                return _accessToken;
            }


            ////////////////////////////////////////////////
            ////////////    Get User Info    ////////////////
            ////////////////////////////////////////////////
            private static string _username;
            private static int _userId;
            public static IEnumerator Get_MeUser(string p_access_token)
            {
                
                // 創建一個新的GET請求
                var meUserRequest = HttpTools.request_get(_GetApiUrl($"/api/v1/me"), p_access_token);
                yield return meUserRequest.SendWebRequest();

                if( HttpTools.check_respone(meUserRequest,"/api/v1/me error: ") != UnityWebRequest.Result.Success)
                    yield break;
                
                Debug.Log($"/api/v1/queryuser status: {meUserRequest.responseCode} text: {meUserRequest.downloadHandler.text}");
                if (meUserRequest.responseCode == 200)
                {
                    var Obj = HttpTools.GetResponseObj<MeUser_response>(meUserRequest.downloadHandler.text);
                    _userId = Obj.id;
                    _username = Obj.username;
                }
            }
            public static string GetUsername(){
                return _username;
            }
            public static int GetUserId(){
                return _userId;
            }

            ////////////////////////////////////////////////
            ////////////    Create User    ////////////////
            ////////////////////////////////////////////////
            private static bool _createUserSuccess;
            public static IEnumerator Post_CreateUser(string p_username, string p_email, string p_password)
            {
                // 準備建立使用者的JSON數據
                var userData = new createUser_request();
                userData.username = p_username;
                userData.email = p_email;
                userData.password = p_password;
                        
                // 建立User
                var createUserRequest = HttpTools.request_post(_GetApiUrl("/api/v1/createuser"), JsonUtility.ToJson(userData) );
                yield return createUserRequest.SendWebRequest();
                
                if( HttpTools.check_respone(createUserRequest,"Failed to create user: ") != UnityWebRequest.Result.Success){
                    _createUserSuccess = false;
                    yield break;
                }
                _createUserSuccess = true;
                Debug.Log($"/api/v1/createusere status: {createUserRequest.responseCode} text: {createUserRequest.downloadHandler.text}");
            }
            public static bool IsCreateUserSuccess(){
                return _createUserSuccess;
            }

            
            
            

            
            ////////////////////////////////////////////////
            ////////////    Get Coach Info    ////////////
            ////////////////////////////////////////////////
            private static List<CoachData> _coachDataList;
            public static IEnumerator Get_CoachAll(string access_token)
            {
                var coachAllRequest = HttpTools.request_get(_GetApiUrl("/api/v1/coach/all"), access_token );
                yield return coachAllRequest.SendWebRequest();
                
                if( HttpTools.check_respone(coachAllRequest,"/api/v1/coach/all error: ") != UnityWebRequest.Result.Success)
                    yield break;
                
                Debug.Log($"/api/v1/coach/all status: {coachAllRequest.responseCode} text: {coachAllRequest.downloadHandler.text}");
                if (coachAllRequest.responseCode == 200)
                {
                    var Obj = HttpTools.GetResponseObj<CoachAll_response>(coachAllRequest.downloadHandler.text);
                    _coachDataList = Obj.data;
                    Debug.Log($"coach number:{ _coachDataList.Count }");
                }
                else
                {
                    Debug.LogWarning($" coachAllRequest.responseCode :{ coachAllRequest.responseCode }");
                }
            }
            public static List<CoachData> GetCoachDataList(){
                return _coachDataList;
            }
            
            ////////////////////////////////////////////////
            ///////    Get Motion Data By UserId   /////////
            ////////////////////////////////////////////////
            private static List<MotionRecord> _motionRecordList;
            public static IEnumerator Get_MotionDataByUserId(int p_userId, string p_access_token)
            {
                // 創建一個新的GET請求
                var motionDataByUserIdRequest = HttpTools.request_get(_GetApiUrl($"/api/v1/motiondata/search/userid?value={p_userId}"),p_access_token);
                yield return motionDataByUserIdRequest.SendWebRequest();

                if( HttpTools.check_respone(motionDataByUserIdRequest,$"/api/v1/motiondata/search/userid?value={p_userId} error: ") != UnityWebRequest.Result.Success)
                    yield break;
                
                Debug.Log($"/api/v1/motiondata/search/userid?value={p_userId} status: {motionDataByUserIdRequest.responseCode} text: {motionDataByUserIdRequest.downloadHandler.text}");
                if (motionDataByUserIdRequest.responseCode == 200)
                {
                    var Obj = HttpTools.GetResponseObj<MotionDataAll_response>(motionDataByUserIdRequest.downloadHandler.text);
                    _motionRecordList = Obj.data;
                    Debug.Log($"Number of Files: {_motionRecordList.Count}");
                    
                }
                else{
                    Debug.LogWarning($" motionDataByUserIdRequest.responseCode :{ motionDataByUserIdRequest.responseCode }");
                }
            }
            public static List<MotionRecord> GetMotionRecordList(){
                return _motionRecordList;
            }

            ///////////////////////////////////////////////////////
            //////////    Get Motion Data by filename   ///////////
            ///////////////////////////////////////////////////////
            private static MotionData _motionData;
            public static IEnumerator Get_MotionDataLoad(string p_filename, string p_access_token)
            {
                // 創建一個新的GET請求
                var motionDataLoadRequest = HttpTools.request_get(_GetApiUrl($"/api/v1/motiondata/load/{p_filename}"),p_access_token);
                yield return motionDataLoadRequest.SendWebRequest();

                if( HttpTools.check_respone(motionDataLoadRequest,"/api/v1/motiondata/load error: ") != UnityWebRequest.Result.Success)
                    yield break;
                
                Debug.Log($"/api/v1/motiondata/load status: {motionDataLoadRequest.responseCode} text: {motionDataLoadRequest.downloadHandler.text}");
                if (motionDataLoadRequest.responseCode == 200)
                {
                    var Obj = HttpTools.GetResponseObj<MotionData>(motionDataLoadRequest.downloadHandler.text);
                    _motionData = Obj;
                }
                else{
                    _motionData = null;
                    Debug.LogWarning($" motionDataLoadRequest.responseCode :{ motionDataLoadRequest.responseCode }");
                }
            }
            public static MotionData GetMotionData(){
                return _motionData;
            }
            ///////////////////////////////////////////////////////
            ////////////    Post Record Motion Data    ////////////
            ///////////////////////////////////////////////////////
            
            private static bool _uploadSuccess;
            public static IEnumerator Post_MotionDataNew(MotionData p_motionData, string p_access_token)
            {
                Debug.Log($"Frames Count:{p_motionData.motionFrames.Count}");
                
                var motionNewRequest = HttpTools.request_post(_GetApiUrl("/api/v1/motiondata/new"), JsonUtility.ToJson(p_motionData), p_access_token);
                yield return motionNewRequest.SendWebRequest();
                
                if( HttpTools.check_respone(motionNewRequest,"/api/v1/motiondata/new error: ") != UnityWebRequest.Result.Success){
                    _uploadSuccess = false;
                    yield break;
                }
                _uploadSuccess = true;
                
                Debug.Log("Upload Record Data Success!");
                Debug.Log($"/api/v1/coach/motiondata/new status: {motionNewRequest.responseCode} text: {motionNewRequest.downloadHandler.text}");
            }

            public static bool GetUploadSuccess(){
                return _uploadSuccess;
            }
            
        }
    }
}

