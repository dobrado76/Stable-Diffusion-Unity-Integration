using Newtonsoft.Json;
using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// Component to help generate a Material Texture using Stable Diffusion.
/// </summary>
[ExecuteAlways]
public class StableDiffusionText2Material : StableDiffusionGenerator
{
    [ReadOnly]
    public string guid = "";
    
    public string prompt;
    public string negativePrompt;

    /// <summary>
    /// List of samplers to display as Drop-Down in the inspector
    /// </summary>
    [SerializeField]
    public string[] samplersList
    {
        get
        {
            if (sdc == null)
                sdc = GameObject.FindObjectOfType<StableDiffusionConfiguration>();
            return sdc.samplers;
        }
    }
    /// <summary>
    /// Actual sampler selected in the drop-down list
    /// </summary>
    [HideInInspector]
    public int selectedSampler = 0;

    public int width = -1;
    public int height = -1;
    public int steps = 0;
    public float cfgScale = 0;
    public long seed = -1;
    
    public long generatedSeed = -1;

    public bool tiling = true;
    [Range(1, 100)]
    public int tilingX = 1;
    [Range(1, 100)]
    public int tilingY = 1;
    [Range(0, 1)]
    public float metallic = 0.1f;
    [Range(0, 1)]
    public float smoothness = 0.5f;

    public bool generateNormalMap = true;
    [Range(0, 10)]
    public float normalMapStrength = 0.5f;

    string filename = "";

    private Texture2D generatedTexture = null;
    private Texture2D generatedNormal = null;

    /// <summary>
    /// List of models to display as Drop-Down in the inspector
    /// </summary>
    [SerializeField]
    public string[] modelsList
    {
        get
        {
            if (sdc == null)
                sdc = GameObject.FindObjectOfType<StableDiffusionConfiguration>();
            return sdc.modelNames;
        }
    }
    /// <summary>
    /// Actual model selected in the drop-down list
    /// </summary>
    [HideInInspector]
    public int selectedModel = 0;


    public bool applyRecursively = true;


    /// <summary>
    /// On Awake, fill the properties with default values from the selected settings.
    /// </summary>
    void Awake()
    {
#if UNITY_EDITOR
        if (width < 0 || height < 0)
        {
            StableDiffusionConfiguration sdc = GameObject.FindObjectOfType<StableDiffusionConfiguration>();
            if (sdc != null)
            {
                SDSettings settings = sdc.settings;
                if (settings == null)
                {

                    width = settings.width;
                    height = settings.height;
                    steps = settings.steps;
                    cfgScale = settings.cfgScale;
                    seed = settings.seed;
                    return;
                }
            }

            width = 512;
            height = 512;
            steps = 50;
            cfgScale = 7;
            seed = -1;
        }
#endif
    }

    private void Start()
    {
#if UNITY_EDITOR
        EditorUtility.ClearProgressBar();
#endif
    }


    /// <summary>
    /// Get the mesh renderer in this object, or in childrens if allowed.
    /// </summary>
    /// <returns>The first mesh renderer found in the hierarchy at this level or in the children</returns>
    MeshRenderer GetMeshRenderer()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null)
        {
            if (!applyRecursively)
                return null;

            MeshRenderer[] mrs = FindInChildrenAll<MeshRenderer>(this.gameObject);
            if (mrs == null || mrs.Length == 0)
                return null;

            mr = mrs[0];
        }
        
        return mr;
    }

    // Keep track of material properties value, to detect if the user changes them on the fly, from the inspector
    float _normalMapStrength = -1;
    int _tilingX = -1;
    int _tilingY = -1;
    float _metallic = -1;
    float _smoothness = -1;

    /// <summary>
    /// Loop update
    /// </summary>
    void Update()
    {
#if UNITY_EDITOR
        // Clamp image dimensions values between 128 and 2048 pixels
        if (width < 128) width = 128;
        if (height < 128) height = 128;
        if (width > 2048) width = 2048;
        if (height > 2048) height = 2048;

        // If not setup already, generate a GUID (Global Unique Identifier)
        if (guid == "")
            guid = Guid.NewGuid().ToString();

        // Update normal map strength whenever the user modifies it in the inspector
        if (_normalMapStrength != normalMapStrength)
        {
            MeshRenderer mr = GetMeshRenderer();
            if (mr != null)
                mr.sharedMaterial.SetFloat("_BumpScale", normalMapStrength);

            UpdateMaterialProperties();

            _normalMapStrength = normalMapStrength;
        }

        // Update tilling, metallic and smoothness properties whenever the user modifies them in the inspector
        if (_tilingX != tilingX || _tilingY != tilingY || _metallic != metallic || _smoothness != smoothness)
        {
            UpdateMaterialProperties();

            _tilingX = tilingX;
            _tilingY = tilingY;
            _metallic = metallic;
            _smoothness = smoothness;
        }
#endif
    }


    // Internally keep tracking if we are currently generating (prevent re-entry)
    bool generating = false;

    /// <summary>
    /// Callback function for the inspector Generate button.
    /// </summary>
    public void Generate()
    {
        // Start generation asynchronously
        if (!generating && !string.IsNullOrEmpty(prompt))
        {
            StartCoroutine(GenerateAsync());
        }
    }


    /// <summary>
    /// Setup the output path and filename for image generation
    /// </summary>
    void SetupFolders()
    {
        // Get the configuration settings
        if (sdc == null)
            sdc = GameObject.FindObjectOfType<StableDiffusionConfiguration>();

        try
        {
            // Determine output path
            string root = Application.dataPath + sdc.settings.OutputFolder;
            if (root == "" || !Directory.Exists(root))
                root = Application.streamingAssetsPath;
            string mat = Path.Combine(root, "SDMaterials");
            filename = Path.Combine(mat, guid + ".png");

            // If folders not already exists, create them
            if (!Directory.Exists(root))
                Directory.CreateDirectory(root);
            if (!Directory.Exists(mat))
                Directory.CreateDirectory(mat);

            // If the file already exists, delete it
            if (File.Exists(filename))
                File.Delete(filename);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\n\n" + e.StackTrace);
        }
    }


    /// <summary>
    /// Request an image generation to the Stable Diffusion server, asynchronously.
    /// </summary>
    /// <returns></returns>
    IEnumerator GenerateAsync()
    {
        generating = true;

        SetupFolders();

        // Set the model parameters
        yield return sdc.SetModelAsync(modelsList[selectedModel]);

        // Generate the image
        HttpWebRequest httpWebRequest = null;

        try
        {
            // Make a HTTP POST request to the Stable Diffusion server
            httpWebRequest = (HttpWebRequest)WebRequest.Create(sdc.settings.StableDiffusionServerURL + sdc.settings.TextToImageAPI);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            // add auth-header to request
            if (sdc.settings.useAuth && !sdc.settings.user.Equals("") && !sdc.settings.pass.Equals(""))
            {
                httpWebRequest.PreAuthenticate = true;
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(sdc.settings.user + ":" + sdc.settings.pass);
                string encodedCredentials = Convert.ToBase64String(bytesToEncode);
                httpWebRequest.Headers.Add("Authorization", "Basic " + encodedCredentials);
            }
            
            // Send the generation parameters along with the POST request
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                SDParamsInTxt2Img sd = new SDParamsInTxt2Img();
                sd.prompt = prompt;
                sd.negative_prompt = negativePrompt;
                sd.steps = steps;
                sd.cfg_scale = cfgScale;
                sd.width = width;
                sd.height = height;
                sd.seed = seed;
                sd.tiling = tiling;

                if (selectedSampler >= 0 && selectedSampler < samplersList.Length)
                    sd.sampler_name = samplersList[selectedSampler];

                // Serialize the input parameters
                string json = JsonConvert.SerializeObject(sd);

                // Send to the server
                streamWriter.Write(json);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\n\n" + e.StackTrace);
        }

        // Read the output of generation
        if (httpWebRequest != null)
        {
            // Wait that the generation is complete before procedding
            Task<WebResponse> t = httpWebRequest.GetResponseAsync();
            while (!t.IsCompleted)
            {
                if (sdc.settings.useAuth && !sdc.settings.user.Equals("") && !sdc.settings.pass.Equals(""))
                {
                    UpdateGenerationProgressWithAuth();
                    yield return new WaitForSeconds(0.5f);
                }
                else
                {
                    UpdateGenerationProgress();
                    yield return new WaitForSeconds(0.5f);
                }
            }
            var httpResponse = t.Result;

            // Get response from the server
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                // Decode the response as a JSON string
                string result = streamReader.ReadToEnd();

                // Deserialize the JSON string into a data structure
                SDResponseTxt2Img json = JsonConvert.DeserializeObject<SDResponseTxt2Img>(result);

                // If no image, there was probably an error so abort
                if (json.images == null || json.images.Length == 0)
                {
                    Debug.LogError("No image was return by the server. This should not happen. Verify that the server is correctly setup.");

                    generating = false;
#if UNITY_EDITOR
                    EditorUtility.ClearProgressBar();
#endif
                    yield break;
                }

                // Decode the image from Base64 string into an array of bytes
                byte[] imageData = Convert.FromBase64String(json.images[0]);

                // Write it in the specified project output folder
                using (FileStream imageFile = new FileStream(filename, FileMode.Create))
                {
#if UNITY_EDITOR
                    AssetDatabase.StartAssetEditing();
#endif
                    yield return imageFile.WriteAsync(imageData, 0, imageData.Length);
#if UNITY_EDITOR
                    AssetDatabase.StopAssetEditing();
                    AssetDatabase.SaveAssets();
#endif
                }

                try
                {
                    // Read back the image into a texture
                    if (File.Exists(filename))
                    {
                        Texture2D texture = new Texture2D(2, 2);
                        texture.LoadImage(imageData);
                        texture.Apply();
                    
                        LoadIntoMaterial(texture);
                    }

                    // Read the generation info back (only seed should have changed, as the generation picked a particular seed)
                    if (json.info != "")
                    {
                        SDParamsOutTxt2Img info = JsonConvert.DeserializeObject<SDParamsOutTxt2Img>(json.info);

                        // Read the seed that was used by Stable Diffusion to generate this result
                        generatedSeed = info.seed;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message + "\n\n" + e.StackTrace);
                }
            }
        }
#if UNITY_EDITOR
        EditorUtility.ClearProgressBar();
#endif
        generating = false;
        yield return null;
    }


    /// <summary>
    /// Load the texture into a material.
    /// </summary>
    /// <param name="texture">Texture to add to the material</param>
    void LoadIntoMaterial(Texture2D texture)
    {
        try
        {
            MeshRenderer mr = GetMeshRenderer();
            if (mr == null)
                return;

            Shader standardShader = sdc.settings.useUniversalRenderPipeline ? Shader.Find("Universal Render Pipeline/Lit") : Shader.Find("Standard");
            
            if(!standardShader)
                Debug.LogError("Shader setup wrong: Please check if you're project uses 'Standard' or 'Universal Render Pipeline'");
            
            mr.sharedMaterial = new Material(standardShader);
            mr.sharedMaterial.mainTexture = texture;
            generatedTexture = texture;

            // Apply the material to childrens if required
            if (applyRecursively)
            {
                MeshRenderer[] mrs = FindInChildrenAll<MeshRenderer>(this.gameObject);
                foreach (MeshRenderer m in mrs)
                    if (m != mr)
                    {
                        m.sharedMaterial = mr.sharedMaterial;
                    }
            }

            // Generate the normal map
            GenerateNormalMap();

            // Force the assets and scene to refresh with new material
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                EditorUtility.SetDirty(generatedTexture);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                SceneView.RepaintAll();
                SceneView.FrameLastActiveSceneView();
                //SceneView.FocusWindowIfItsOpen(typeof(SceneView));
                EditorApplication.QueuePlayerLoopUpdate();
                EditorSceneManager.MarkAllScenesDirty();
                EditorUtility.RequestScriptReload();
            }
#endif
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\n\n" + e.StackTrace);
        }
    }


    /// <summary>
    /// Generate a normal map from the generated texture.
    /// </summary>
    public void GenerateNormalMap()
    {
        if (generatedTexture == null)
            return;

        try
        {
            MeshRenderer mr = GetMeshRenderer();
            if (mr == null)
                return;

            if (generateNormalMap)
            {
                generatedNormal = CreateNormalmap(generatedTexture, 0.5f);
#if UNITY_EDITOR
                EditorUtility.SetDirty(generatedNormal);
#endif
            }
            else
                generatedNormal = null;

            UpdateMaterialProperties();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message + "\n\n" + e.StackTrace);
        }
    }


    /// <summary>
    /// Update the material properties. 
    /// Also apply to children if set to apply recursively.
    /// </summary>
    void UpdateMaterialProperties()
    {
        MeshRenderer mr = GetMeshRenderer();
        if (mr == null)
            return;

        // Apply tilling, metallic and smoothness
        mr.sharedMaterial.mainTextureScale = new Vector2(-tilingX, -tilingY);
        mr.sharedMaterial.SetFloat("_Metallic", metallic);
        mr.sharedMaterial.SetFloat("_Glossiness", smoothness);

        // Apply normal map if required
        if (generateNormalMap && generatedNormal != null)
        {
            mr.sharedMaterial.SetTexture("_BumpMap", generatedNormal);
            mr.sharedMaterial.SetFloat("_BumpScale", normalMapStrength);
            mr.sharedMaterial.EnableKeyword("_NORMALMAP");
        }
        // Disable normal map
        else
        {
            mr.sharedMaterial.SetTexture("_BumpMap", null);
            mr.sharedMaterial.DisableKeyword("_NORMALMAP");
        }

        // Apply recursively if required
        if (applyRecursively)
        {
            MeshRenderer[] mrs = FindInChildrenAll<MeshRenderer>(this.gameObject);
            foreach (MeshRenderer m in mrs)
                if (m != mr)
                {
                    m.sharedMaterial = mr.sharedMaterial;
                }
        }
    }


    /// <summary>
    /// Create a Normal map based on the gradient in 3x3 surrounding neighborhood.
    /// Based on UnityCoder code: https://github.com/unitycoder/NormalMapFromTexture
    /// </summary>
    /// <returns>Normal map texture</returns>
    /// <param name="t">Source texture</param>
    /// <param name="normalStrength">Normal map strength float (example: 1-20)</param>
    public static Texture2D CreateNormalmap(Texture2D t, float normalStrength)
    {
        Color[] pixels = new Color[t.width * t.height];
        Texture2D texNormal = new Texture2D(t.width, t.height, TextureFormat.RGB24, false, false);
        Vector3 vScale = new Vector3(0.3333f, 0.3333f, 0.3333f);

        Color tc;
        for (int y = 0; y < t.height; y++)
        {
            for (int x = 0; x < t.width; x++)
            {
                tc = t.GetPixel(x - 1, y - 1); Vector3 cSampleNegXNegY = new Vector3(tc.r, tc.g, tc.g); 
                tc = t.GetPixel(x - 0, y - 1); Vector3 cSampleZerXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y - 1); Vector3 cSamplePosXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 1, y - 0); Vector3 cSampleNegXZerY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y - 0); Vector3 cSamplePosXZerY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 1, y + 1); Vector3 cSampleNegXPosY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 0, y + 1); Vector3 cSampleZerXPosY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y + 1); Vector3 cSamplePosXPosY = new Vector3(tc.r, tc.g, tc.g);

                float fSampleNegXNegY = Vector3.Dot(cSampleNegXNegY, vScale);
                float fSampleZerXNegY = Vector3.Dot(cSampleZerXNegY, vScale);
                float fSamplePosXNegY = Vector3.Dot(cSamplePosXNegY, vScale);
                float fSampleNegXZerY = Vector3.Dot(cSampleNegXZerY, vScale);
                float fSamplePosXZerY = Vector3.Dot(cSamplePosXZerY, vScale);
                float fSampleNegXPosY = Vector3.Dot(cSampleNegXPosY, vScale);
                float fSampleZerXPosY = Vector3.Dot(cSampleZerXPosY, vScale);
                float fSamplePosXPosY = Vector3.Dot(cSamplePosXPosY, vScale);

                float edgeX = (fSampleNegXNegY - fSamplePosXNegY) * 0.25f + (fSampleNegXZerY - fSamplePosXZerY) * 0.5f + (fSampleNegXPosY - fSamplePosXPosY) * 0.25f;
                float edgeY = (fSampleNegXNegY - fSampleNegXPosY) * 0.25f + (fSampleZerXNegY - fSampleZerXPosY) * 0.5f + (fSamplePosXNegY - fSamplePosXPosY) * 0.25f;

                Vector2 vEdge = new Vector2(edgeX, edgeY) * normalStrength;
                Vector3 norm = new Vector3(vEdge.x, vEdge.y, 1.0f).normalized;
                Color c = new Color(norm.x * 0.5f + 0.5f, norm.y * 0.5f + 0.5f, norm.z * 0.5f + 0.5f, 1);

                pixels[x + y * t.width] = c;
            }
        } 

        texNormal.SetPixels(pixels);
        texNormal.Apply();

        return texNormal;
    } 
}
