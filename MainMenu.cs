using Assets.Scripts.Unity.Menu;
using Assets.Scripts.Unity.UI_New;
using Assets.Scripts.Unity.UI_New.Main;
using Assets.Scripts.Unity.UI_New.Main.WorldItems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace Mothership{
    public class TalDarimMainMenu{
        public static SC2Tower SC2Data=new(Bundles.Bundles.mainmenu);
        public static Il2CppReferenceArray<GameObject>MenuObjects;
        public static GameObject Pylon;
        public static GameObject BloodHunter;
        public static GameObject Gateway;
        public static GameObject CyberCore;
        public static GameObject TwlCouncil;
        public static GameObject PhoCannon;
        public static GameObject Nexus;
        public static GameObject Phoenix;
        public static GameObject BackgroundImage;
        public static GameObject Camera;
        [HarmonyPatch(typeof(InteractionChecker),"Update")]
        public class InteractionCheckerUpdate_Patch{
            public static bool moved=false;
            [HarmonyPrefix]
            public static bool Prefix(ref InteractionChecker __instance){
		        if(InputSystemController.GetMouseButtonDown(0)){
		            Vector2 mousePosition=InputSystemController.MousePosition;
                    Vector3 v3=new(mousePosition.x/Screen.width,mousePosition.y/Screen.height,0);
		            Ray ray=__instance.sceneCamera.ViewportPointToRay(v3);
                    if(Physics.Raycast(ray,out RaycastHit hit)){
                        GameObject gObj=hit.collider.gameObject;
                        if(gObj.HasComponent<InteractableObject>()){
                            switch(gObj.name){
                                case"Pylon-Model":
                                    BloodHunterBehavior BHbehavior=BloodHunter.GetComponent<BloodHunterBehavior>();
                                    BHbehavior.transform.localPosition=new(-130,0,-55);
                                    foreach(var renderer in BHbehavior.GetComponentsInChildren<SkinnedMeshRenderer>()){
                                        renderer.material.SetFloat("_Visibility",0);
                                    }
                                    BHbehavior.gameObject.active=true;
                                    PlaySound("bloodhunter-clip");
                                    break;
                                case"Gateway-Model":
                                    GatewayBehavior Gbehavior=Gateway.GetComponent<GatewayBehavior>();
                                    if(!Gbehavior.Transforming){
                                        MelonCoroutines.Start(Gbehavior.Transform());
                                    }
                                    break;
                                default:
                                    gObj.GetComponent<InteractableObject>().OnInteract();
                                    break;
                            }
                        }
                    }
                }
                return false;
            }
        }
        [HarmonyPatch(typeof(MainMenu),"Open")]
        public class MainMenuOpen_Patch{
            [HarmonyPostfix]
            public static void Postfix(){
                if(SceneManager.GetSceneByName("MainMenuWorld").IsValid()){
                    SceneManager.UnloadScene("MainMenuWorld");
                }
                if(!SceneManager.GetSceneByName("TalDarimMainMenu").IsValid()){
                    SceneManager.LoadScene(SC2Data.Bundle.GetAllScenePaths()[0],LoadSceneMode.Additive);
                    MelonCoroutines.Start(Setup());
                }
                uObject.FindObjectOfType<CommonBackgroundScreen>().EnableClearObject(false);
            }
            public static System.Collections.IEnumerator Setup(){
                yield return null;
                MenuObjects=SceneManager.GetSceneByName("TalDarimMainMenu").GetRootGameObjects();
                BloodHunter=MenuObjects.First(a=>a.name=="BloodHunter-Model");
                Pylon=MenuObjects.First(a=>a.name=="Pylon-Model");
                BackgroundImage=MenuObjects.First(a=>a.name=="TalDarimBackgroundImage");
                Camera=MenuObjects.First(a=>a.name=="MainCamera");
                Gateway=MenuObjects.First(a=>a.name=="Gateway-Model");
                Gateway.AddComponent<InteractableObject>();
                Gateway.AddComponent<GatewayBehavior>();
                Pylon.AddComponent<InteractableObject>();
                BloodHunter.active=false;
                BloodHunter.AddComponent<BloodHunterBehavior>();
                Camera.AddComponent<InteractionChecker>().sceneCamera=Camera.GetComponent<Camera>();
            }
        }
        [HarmonyPatch(typeof(CommonBackgroundScreen),"GetMenuBGData")]
        public class CommonBackgroundScreenGetMenuBGData_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref CommonBackgroundScreen __instance){
                if(__instance.mainMenuWorldBlurredImg.texture.name!="TaldarimBackgroundTexture"&&SceneManager.GetSceneByName("TalDarimMainMenu").IsValid()){
                    __instance.mainMenuWorldBlurredImg.texture=BackgroundImage.GetComponent<RawImage>().texture;
                    __instance.mainMenuWorldBlurredImg.gameObject.AddComponent<TalDarimBackgroundScrolling>();
                }
            }
        }
        [HarmonyPatch(typeof(MenuManager),"UnloadSceneAsync")]
        public class MenuManagerUnloadSceneAsync_Patch{
            [HarmonyPrefix]
            public static void Prefix(ref string sceneName){
                if(sceneName=="MainMenuWorld"){
                    sceneName="TalDarimMainMenu";
                    if(SceneManager.GetSceneByName(sceneName).IsValid()){
                        SceneManager.UnloadScene(sceneName);
                    }
                }
            }
        }
        [RegisterTypeInIl2Cpp]
        public class GatewayBehavior:MonoBehaviour{
            public GatewayBehavior(IntPtr ptr):base(ptr){}
            public Animator AnimCtrl;
            public Il2CppArrayBase<ParticleSystem>PartSys;
            public bool Transforming;
            public void Start(){
                AnimCtrl=GetComponent<Animator>();
                PartSys=GetComponentsInChildren<ParticleSystem>();
            }
            public System.Collections.IEnumerator Transform(){
                Transforming=true;
                AnimCtrl.Play("Gateway-TransformStart");
                foreach(ParticleSystem partSys in PartSys){
                    partSys.Play();
                }
                yield return new WaitForSeconds(10);
                AnimCtrl.Play("Gateway-TransformEnd");
                yield return new WaitForSeconds(3.3f);
                Transforming=false;
            }
        }
        [RegisterTypeInIl2Cpp]
        public class TalDarimBackgroundScrolling:MonoBehaviour{
            public TalDarimBackgroundScrolling(IntPtr ptr):base(ptr){}
            RawImage image;
            public void Start(){
                image=GetComponent<RawImage>();
                image.uvRect=new(0,0,16,16);
            }
            public void Update(){
                image.uvRect=new(image.uvRect.x+0.001f,0,image.uvRect.width,image.uvRect.height);
            }
        }
        [RegisterTypeInIl2Cpp]
        public class BloodHunterBehavior:MonoBehaviour{
            public BloodHunterBehavior(IntPtr ptr):base(ptr){}
            float visibility=0;
            List<Material>materials=new();
            public void Start(){
                foreach(var renderer in GetComponentsInChildren<SkinnedMeshRenderer>()){
                    materials.Add(renderer.material);
                    renderer.material.SetFloat("_Visibility",0);
                }
            }
            public void Update(){
                visibility=materials[0].GetFloat("_Visibility");
                transform.localPosition=new(transform.localPosition.x+0.4f,transform.localPosition.y,transform.localPosition.z);
                if(transform.position.x<-100){
                    foreach(Material material in materials){
                        if(visibility>=1.05){
                            visibility=1;
                            material.SetFloat("_Visibility",1);
                        }else{
                            material.SetFloat("_Visibility",visibility+0.024f);
                        }
                        return;
                    }
                }
                if(transform.localPosition.x>130){
                    transform.localPosition=new(-130,0,-55);
                    visibility=0;
                    foreach(Material material in materials){
                        material.SetFloat("_Visibility",0);
                    }
                    gameObject.active=false;
                    return;
                }
                if(transform.localPosition.x>100){
                    foreach(Material material in materials){
                        if(visibility<=0){
                            visibility=0;
                            material.SetFloat("_Visibility",0);
                        }else{
                            material.SetFloat("_Visibility",visibility-0.024f);
                        }
                        return;
                    }
                }
            }
        }
    }
}