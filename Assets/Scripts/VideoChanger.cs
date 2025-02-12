using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.Exceptions;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoChanger : MonoBehaviour
{
    public VideoPlayer player;
    public float fadeDuration = 1.0f;
    public Material videoMaterial;
    public Material skyboxMaterial;
    public CanvasFade fadeCanvas;
    public int selectedVideoClip;
    public Slider downloadSlider;
    public TMP_Text downloadText;
    private bool videoDownload = false;
    
    private int lastPickedVideoIndex = -1;
    private IEnumerator coroutine;
    private AsyncOperationHandle currentVideoOperationHandle;
    [SerializeField] public AssetReference[] assetReferences;



    private Material skyMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skyMaterial = RenderSettings.skybox;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            Debug.Log("Start");
            StartVideo(0);
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            Debug.Log("Pause");

            PauseVideo();
        }
    }

    public void StartVideo(int i)
    {
        

        if (videoDownload == false)
        {
                    videoDownload = true;

            coroutine = SwitchVideo(videoMaterial, i, player.Play);
                StartCoroutine(coroutine);
            
       
                  
                }

    }


    public void PauseVideo()
    {
        StartCoroutine(StopVideo(skyMaterial, player.Pause));
    }


    private IEnumerator SwitchVideo(Material material, int clip, Action onCompleteAction)
    {
        fadeCanvas.StartFadeIn(fadeDuration);
        


        yield return new WaitForSeconds(fadeDuration);
        if (lastPickedVideoIndex != clip)
        {
            if (currentVideoOperationHandle.IsValid())
            {
                player.clip = null;
                Addressables.Release(currentVideoOperationHandle);
                Debug.Log("Handle is valid");
            }
            lastPickedVideoIndex = clip;

            currentVideoOperationHandle = assetReferences[clip].LoadAssetAsync<VideoClip>();

            downloadSlider.gameObject.SetActive(true);
            downloadText.gameObject.SetActive(true);
            Debug.Log("set slider active");
            while (!currentVideoOperationHandle.IsDone)
            {
                string bytesProgress = BytesToString(currentVideoOperationHandle.GetDownloadStatus().DownloadedBytes);
                string bytesToDownload = BytesToString(currentVideoOperationHandle.GetDownloadStatus().TotalBytes);


                downloadText.text = bytesProgress + " / " + bytesToDownload;
                float progress = currentVideoOperationHandle.GetDownloadStatus().Percent;
                
                Debug.Log(progress);
                downloadSlider.value = progress;
                
                yield return null;

            }
            yield return currentVideoOperationHandle;
            




            Debug.Log("No error");

            
            downloadSlider.gameObject.SetActive(false);
            downloadText.gameObject.SetActive(false);
            Debug.Log("set slider non active");
            if (currentVideoOperationHandle.Status == AsyncOperationStatus.Failed)
            {
                Debug.Log("Network Error");
                downloadText.gameObject.SetActive(true);
                downloadText.text = "Check Internet connection!";
                videoDownload = false;
                lastPickedVideoIndex = -1;
                player.Stop();
                yield return new WaitForSeconds(5.0f);
                downloadText.gameObject.SetActive(false);
                fadeCanvas.StartFadeOut(fadeDuration);
                
                StopCoroutine(coroutine);
                yield break;
            }
            


            RenderSettings.skybox = material;
            if (player.clip != (VideoClip)currentVideoOperationHandle.Result)
            {
                player.clip = (VideoClip)currentVideoOperationHandle.Result;
            }
            else
            {
                player.time = 0;
            }
        }
        else
        {
            player.time = 0;
        }

            onCompleteAction.Invoke();
            Debug.Log("Start playing");

            yield return new WaitUntil(() => player.isPlaying);
            yield return new WaitForSeconds(fadeDuration);
        
       

        fadeCanvas.StartFadeOut(fadeDuration);
        videoDownload = false;
    }

    private IEnumerator StopVideo(Material material, Action onCompleteAction)
    {
        fadeCanvas.StartFadeIn(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        fadeCanvas.StartFadeOut(fadeDuration);
        RenderSettings.skybox = material;
        onCompleteAction.Invoke();
    }

    private String BytesToString(long byteCount) //Format bytes
    {
        string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
        if (byteCount == 0)
            return "0" + suf[0];
        long bytes = Math.Abs(byteCount);
        int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
        double num = Math.Round(bytes / Math.Pow(1024, place), 1);
        return (Math.Sign(byteCount) * num).ToString() + suf[place];
    }



}

public class AcceptAllCertificatesSignedWithASpecificKeyPublicKey : CertificateHandler
{
    private static string PUB_KEY = "3082020A0282020100E3215F0C11A04B73563C898D18BF1980C83F9A5387C84456EEEB88F8B580F11FF0DC859086E7D0E12799F049868A56558EB590355FFA4C9EE7B48C7D91F75063164D2AA489F212BD68FBA1FBEBF43160539066BCA3D7BFFA6F6D94EEBCBB589DC57E415AFCD3981A53A75E22E2F4124DD2E3405400F4B6A6B8EDB296D4752C47D4A0AA6FDBE7A9F41D908D90AD40967E5DA65D0A9924E1D3A2041253C0AB8F96D82CDBAA1D2BCAEE9A65655424F4ACC4E6594BCF8A9721781CC86DC53C8845E3FAA7C4227B698DECFD25ABEA2A399880C21E6428B4DAFD4F722C225E3232AEF29F23620A27C06AA897A3BFDAE9B8390E2AC7B1B15A5168571F211B99890080AC828A73D2113F334DCB11DF31B8E607505E80871ECB657930C744841E2A7337FE546CF257E9F5DCE1628555C14FAECBB69D331C92D05135DE16FED51FA45A6D3D72AE6C614F6FA2DE598EDC76A4CFFC5E574DBA5CE619017DE6195AFE8B1B1374B8CB59501C15EE313F28C8361E46DDBF2B2A3F6E491F5E2A6C045B8FF348047951EC81A57C382D621B88F1762704DBBD7CE1A4E5F90EC79DE77D8FBAA360A2020D8BC4D81AEE6F279CD9F967D807DC6BF1AA147DE143B3F344594E181BBC022C81F66A9A7F5921D70B3E85075F1D988BAA64FE21DF908D87C475DBB9DE62937A13A8326E6175715B1EC64CFEBABF8513D292260366A112A93BCAA9F283F0BE890203010001";

    protected override bool ValidateCertificate(byte[] certificateData)
    {
        X509Certificate2 certificate = new X509Certificate2(certificateData);
        string pk = certificate.GetPublicKeyString();
        Debug.Log(certificate.GetPublicKeyString());
        if (pk.Equals(PUB_KEY))
        {
            return true;
        }
        return false;
    }
}
