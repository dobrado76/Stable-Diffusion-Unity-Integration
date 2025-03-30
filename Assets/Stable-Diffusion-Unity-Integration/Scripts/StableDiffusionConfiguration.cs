using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Math = System.Math;

/// <summary>
/// Global Stable Diffusion parameters configuration.
/// </summary>
[ExecuteInEditMode]
public class StableDiffusionConfiguration : MonoBehaviour
{
    [SerializeField] 
    public SDSettings settings;

    [SerializeField]
    public string[] samplers = new string[]{
        "Euler a", "Euler", "LMS", "Heun", "DPM2", "DPM2 a", "DPM++ 2S a", "DPM++ 2M", "DPM++ SDE", "DPM fast", "DPM adaptive",
        "LMS Karras", "DPM2 Karras", "DPM2 a Karras", "DPM++ 2S a Karras", "DPM++ 2M Karras", "DPM++ SDE Karras", "DDIM", "PLMS"
    };

    [SerializeField]
    public string[] modelNames;

    /// <summary>
    /// Data structure that represents a Stable Diffusion model to help deserialize from JSON string.
    /// </summary>
    class Model
    {
        public string title;
        public string model_name;
        public string hash;
        public string sha256;
        public string filename;
        public string config;
    }

    /// <summary>
    /// Method called when the user click on List Model from the inspector.
    /// </summary>
    public void ListModels()
    {
        StartCoroutine(ListModelsAsync());
    }

    /// <summary>
    /// Get the list of available Stable Diffusion models.
    /// </summary>
    /// <returns></returns>
    IEnumerator ListModelsAsync()
    {
        // Stable diffusion API url for getting the models list
        string url = settings.StableDiffusionServerURL + settings.ModelsAPI;
        Debug.Log("Requesting models from: " + url);

        UnityWebRequest request = new UnityWebRequest(url, "GET");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        if (settings.useAuth && !settings.user.Equals("") && !settings.pass.Equals(""))
        {
            Debug.Log("Using API key to authenticate");
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(settings.user + ":" + settings.pass);
            string encodedCredentials = Convert.ToBase64String(bytesToEncode);
            request.SetRequestHeader("Authorization", "Basic " + encodedCredentials);
        }
        
        yield return request.SendWebRequest();

        // Check for connection errors first
        if (request.result == UnityWebRequest.Result.ConnectionError || 
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Connection error: " + request.error);
            Debug.LogError("Response code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);
            
            if (request.responseCode == 404)
            {
                Debug.LogError("API endpoint not found. Trying alternate API endpoint...");
                // Try with '/api/v1/sd-models' as an alternative
                yield return TryAlternateModelAPI();
                yield break;
            }
            else if (request.responseCode == 401 || request.responseCode == 403)
            {
                Debug.LogError("Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.");
                yield break;
            }
            else
            {
                Debug.LogError("Make sure Stable Diffusion is running locally on port 7860, or update the URL in your settings.");
                yield break;
            }
        }

        try
        {
            Debug.Log("Response received: " + request.downloadHandler.text.Substring(0, Math.Min(100, request.downloadHandler.text.Length)) + "...");
            
            // Check if response is empty or not JSON
            if (string.IsNullOrEmpty(request.downloadHandler.text) || 
                (!request.downloadHandler.text.StartsWith("[") && !request.downloadHandler.text.StartsWith("{")))
            {
                Debug.LogError("Invalid response format. Expected JSON array but got: " + request.downloadHandler.text);
                yield break;
            }

            // Deserialize the response to a class
            Model[] ms = JsonConvert.DeserializeObject<Model[]>(request.downloadHandler.text);

            // Keep only the names of the models
            List<string> modelsNames = new List<string>();

            foreach (Model m in ms) 
                modelsNames.Add(m.model_name);

            // Convert the list into an array and store it for futur use
            modelNames = modelsNames.ToArray();
            Debug.Log("Successfully loaded " + modelNames.Length + " models");
        }
        catch (Exception e)
        {
            Debug.LogError("Error parsing models response: " + e.Message);
            Debug.LogError("Response was: " + request.downloadHandler.text);
            
            // Try with alternate endpoint
            StartCoroutine(TryAlternateModelAPI());
        }
    }
    
    /// <summary>
    /// Try an alternate API endpoint to get models list
    /// </summary>
    private IEnumerator TryAlternateModelAPI()
    {
        string alternateUrl = settings.StableDiffusionServerURL + "/api/sd-models";
        Debug.Log("Trying alternate models API: " + alternateUrl);
        
        UnityWebRequest request = new UnityWebRequest(alternateUrl, "GET");
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        
        if (settings.useAuth && !settings.user.Equals("") && !settings.pass.Equals(""))
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(settings.user + ":" + settings.pass);
            string encodedCredentials = Convert.ToBase64String(bytesToEncode);
            request.SetRequestHeader("Authorization", "Basic " + encodedCredentials);
        }
        
        yield return request.SendWebRequest();
        
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Alternate API also failed: " + request.error);
            Debug.LogError("Please check if your Stable Diffusion WebUI is running at " + settings.StableDiffusionServerURL);
            Debug.LogError("If you're using AUTOMATIC1111's WebUI, make sure you launched it with the '--api' flag");
            yield break;
        }
        
        try
        {
            // Deserialize the response to a class
            Model[] ms = JsonConvert.DeserializeObject<Model[]>(request.downloadHandler.text);
            
            // Keep only the names of the models
            List<string> modelsNames = new List<string>();
            
            foreach (Model m in ms)
                modelsNames.Add(m.model_name);
                
            // Convert the list into an array and store it for future use
            modelNames = modelsNames.ToArray();
            Debug.Log("Successfully loaded " + modelNames.Length + " models using alternate API");
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to parse models from alternate API: " + e.Message);
            Debug.LogError("Make sure your Stable Diffusion WebUI was launched with the '--api' flag");
            Debug.LogError("Example launch command: './webui.sh --api'");
        }
    }

    /// <summary>
    /// Set a model to use by Stable Diffusion.
    /// </summary>
    /// <param name="modelName">Model to set</param>
    /// <returns></returns>
    public IEnumerator SetModelAsync(string modelName)
    {
        // Stable diffusion API url for setting a model
        string url = settings.StableDiffusionServerURL + settings.OptionAPI;
        Debug.Log("Setting model to: " + modelName + " using URL: " + url);

        // Load the list of models if not filled already
        if (modelNames == null || modelNames.Length == 0)
            yield return ListModelsAsync();
            
        // If models still not loaded, can't continue
        if (modelNames == null || modelNames.Length == 0)
        {
            Debug.LogError("Failed to load model list. Cannot set model.");
            yield break;
        }

        // First try with UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(new SDOption { sd_model_checkpoint = modelName })));
        request.SetRequestHeader("Content-Type", "application/json");
        
        if (settings.useAuth && !settings.user.Equals("") && !settings.pass.Equals(""))
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(settings.user + ":" + settings.pass);
            string encodedCredentials = Convert.ToBase64String(bytesToEncode);
            request.SetRequestHeader("Authorization", "Basic " + encodedCredentials);
        }
        
        yield return request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Model successfully set to: " + modelName);
            yield break;
        }
        
        // If unity web request failed, try the alternate method with HttpWebRequest
        Debug.LogWarning("Failed to set model using UnityWebRequest: " + request.error);
        Debug.LogWarning("Trying with HttpWebRequest as fallback...");
        
        try
        {
            // Tell Stable Diffusion to use the specified model using an HTTP POST request
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            // add auth-header to request
            if (settings.useAuth && !settings.user.Equals("") && !settings.pass.Equals(""))
            {
                httpWebRequest.PreAuthenticate = true;
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(settings.user + ":" + settings.pass);
                string encodedCredentials = Convert.ToBase64String(bytesToEncode);
                httpWebRequest.Headers.Add("Authorization", "Basic " + encodedCredentials);
            }
            
            // Write to the stream the JSON parameters to set a model
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                // Model to use
                SDOption sd = new SDOption();
                sd.sd_model_checkpoint = modelName;

                // Serialize into a JSON string
                string json = JsonConvert.SerializeObject(sd);

                // Send the POST request to the server
                streamWriter.Write(json);
                Debug.Log("Sent JSON: " + json);
            }

            // Get the response of the server
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                Debug.Log("Model set successfully. Response: " + result);
            }
        }
        catch (WebException e)
        {
            Debug.LogError("Error setting model: " + e.Message);
            
            // Try one more time with alternate endpoint
            if (url.Contains("/sdapi/v1/options"))
            {
                string alternateUrl = settings.StableDiffusionServerURL + "/api/options";
                Debug.LogWarning("Trying alternate options API: " + alternateUrl);
                
                // We can't yield inside a catch block, so let's use a flag
                Debug.LogWarning("Please try again if this fails - we can't properly retry in a catch block");
                
                // Try launching the alternate method
                StartCoroutine(TrySetModelWithAlternateAPI(modelName, alternateUrl));
            }
        }
    }
    
    /// <summary>
    /// Helper method to try setting a model with an alternate API endpoint
    /// </summary>
    private IEnumerator TrySetModelWithAlternateAPI(string modelName, string alternateUrl)
    {
        UnityWebRequest altRequest = new UnityWebRequest(alternateUrl, "POST");
        altRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        altRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(
            JsonConvert.SerializeObject(new SDOption { sd_model_checkpoint = modelName })));
        altRequest.SetRequestHeader("Content-Type", "application/json");
        
        if (settings.useAuth && !settings.user.Equals("") && !settings.pass.Equals(""))
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(settings.user + ":" + settings.pass);
            string encodedCredentials = Convert.ToBase64String(bytesToEncode);
            altRequest.SetRequestHeader("Authorization", "Basic " + encodedCredentials);
        }
        
        yield return altRequest.SendWebRequest();
        
        if (altRequest.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Model successfully set to: " + modelName + " using alternate API");
        }
        else
        {
            Debug.LogError("All attempts to set model failed. Make sure Stable Diffusion WebUI is running with '--api' flag.");
        }
    }

}
