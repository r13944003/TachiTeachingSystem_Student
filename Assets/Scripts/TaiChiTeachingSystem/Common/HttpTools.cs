using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

namespace TaichiTeachingSystem
{
    // Some basic tools for data transfer between app and server
    public static class HttpTools
    {
        public static UnityWebRequest request_post(string api_url, string jsonString, string access_token = "")
        {
            UnityWebRequest r = new UnityWebRequest(api_url, "POST");
            
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonString );
            r.uploadHandler = new UploadHandlerRaw(jsonToSend);
            r.downloadHandler = new DownloadHandlerBuffer();
            r.SetRequestHeader("Content-Type", "application/json");
            if (access_token.Length > 0)
                r.SetRequestHeader("Authorization", "Bearer " + access_token);
            r.certificateHandler = new BypassCertificate(); 
            return r;
        }
        
        public static UnityWebRequest request_get(string api_url, string access_token="")
        {
            // 創建一個新的GET請求
            UnityWebRequest r = new UnityWebRequest(api_url, "GET");
            r.downloadHandler = new DownloadHandlerBuffer();
            if (access_token.Length > 0)
                r.SetRequestHeader("Authorization", "Bearer " + access_token);
            r.certificateHandler = new BypassCertificate(); 
            return r;
        }
        
        public static UnityWebRequest.Result check_respone(UnityWebRequest rs, string ErrorMsg="")
        {
            switch (rs.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.ProtocolError:
                case UnityWebRequest.Result.DataProcessingError:
                    if (ErrorMsg.Length>0)
                        Debug.LogError($"{ErrorMsg} : {rs.error}  text: { rs.downloadHandler.text}");
                    return rs.result;
            }
            return rs.result;
        }
        
        private static byte[] GetJsonToSend<T>(T data)
        {
            string jsonTokenRequestData = JsonUtility.ToJson(data);
            return new System.Text.UTF8Encoding().GetBytes(jsonTokenRequestData );
        }
        
        public static T GetResponseObj<T>(string JsonString)
        {
            T Obj= JsonUtility.FromJson<T>(JsonString);
            if (Obj == null)
                Debug.LogError($"Failed to parse from response : {typeof(T)}");
            return Obj;
        }
        
        private class BypassCertificate : CertificateHandler
        {
            protected override bool ValidateCertificate(byte[] certificateData)
            {
                return true;
            }
        }
        
    }
}
