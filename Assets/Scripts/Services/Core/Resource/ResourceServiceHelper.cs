using Common.Extensions;
using GameWarriors.EventDomain.Abstraction;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Services.Core.Resource
{
    public static class ResourceServiceHelper
    {
        public static UnityWebRequest CreatePostRequest(string url, string authToken, string data)
        {
            var webRequest = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            webRequest.SetRequestHeader("Content-Type", "application/json");
            //webRequest.SetRequestHeader("Accept", "*/*");
            if (!string.IsNullOrEmpty(authToken))
                webRequest.SetRequestHeader("Authorization", authToken);
            //webRequest.SetRequestHeader("Accept-Encoding", "gzip, deflate, br");
            //webRequest.SetRequestHeader("Connection", "keep-alive");
            //webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            return webRequest;
        }

        public static UnityWebRequest CreateGetRequest(string url, string authToken)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);
            if (!string.IsNullOrEmpty(authToken))
                webRequest.SetRequestHeader("Authorization", authToken);
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            return webRequest;
        }

        public static UnityWebRequest CreateGetTextureRequest(string url, string authToken)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url);
            if (!string.IsNullOrEmpty(authToken))
                webRequest.SetRequestHeader("Authorization", authToken);
            webRequest.SetRequestHeader("Access-Control-Allow-Origin", "*");
            return webRequest;
        }


        public static void HandlerRequestError(IEvent @event ,long errorCode)
        {
            if (errorCode == 401)
                @event.BroadcastEvent(EEventType.OnUserUnauthorize);
        }
    }
}
