using Services.Abstraction;
using System;
using GameWarriors.EventDomain.Abstraction;
using UnityEngine.Networking;
using System.Net;
using UnityEngine;

namespace Services.Core.Resource
{
    public class ResourceService : IResourceService
    {

        private readonly string CONNECT_TO_SHOP_URL = "api/asset";



        private readonly IEvent _event;
        private readonly IAppService _appService;




        public ResourceService(IEvent @event, IAppService appService)
        {
            _event = @event;
            _appService = appService;
            string serverAddress = _appService.ServerURL;
            CONNECT_TO_SHOP_URL = serverAddress + CONNECT_TO_SHOP_URL;
        }

        public async void ConnectToShop(Action<(int status, string result)> onDone)
        {
            UnityWebRequest webRequest = ResourceServiceHelper.CreateGetRequest(CONNECT_TO_SHOP_URL, string.Empty);
            UnityWebRequestAsyncOperation operation = webRequest.SendWebRequest();
            operation.completed += (input) => OnVehicleAssetDownloadDone(input, onDone);
        }


        private void OnVehicleAssetDownloadDone(AsyncOperation operation, Action<(int status, string result)> onDone)
        {
            UnityWebRequestAsyncOperation webOperation = operation as UnityWebRequestAsyncOperation;
            UnityWebRequest webRequest = webOperation.webRequest;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                if (webRequest.downloadHandler != null && webRequest.downloadHandler.data.Length > 0)
                {
                    //File.WriteAllText("data.txt",webRequest.downloadHandler.text);
                    onDone?.Invoke(((int)webRequest.responseCode, webRequest.downloadHandler?.text));
                }
                else
                {
                    ResourceServiceHelper.HandlerRequestError(_event, webRequest.responseCode);
                    onDone?.Invoke(((int)webRequest.responseCode, default));
                }
            }
            else
            {
                ResourceServiceHelper.HandlerRequestError(_event, webRequest.responseCode);
                onDone?.Invoke(((int)webRequest.responseCode, default));
            }
        }

        private void UserUnauthorize()
        {
            //TODO clear files
        }


    }
}