using Assets.MainMenuWorld.Scripts;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Unity.UI_New.Main.WorldItems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Mothership{
    public class MainMenu{
        public static SC2Tower SC2Data=new(Bundles.Bundles.mainmenu);
        [HarmonyPatch(typeof(MainMenuWorldChoreographer),"Start")]
        public class MainMenuWorldChoreographerStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref MainMenuWorldChoreographer  __instance){
                var thing=__instance.GetComponentsInChildren<PlayInteractableAudio>();
                for(int i=0;thing.Count>i;i++){
                    var thing1=thing[i];
                    Log(thing1.transform.parent.name+" "+thing1.name+" "+thing.Count);
                    if(thing1.transform.parent.name=="MonkeyAcePopup"){
                        uObject.Destroy(thing1.transform.GetChild(0).gameObject);
                        uObject.Instantiate(LoadAsset(towerTypes["Phoenix"].Bundle,"Phoenix-Phoenix-Prefab"),thing1.transform).Cast<GameObject>().
                            transform.localPosition=new(0.102f,3.159f,4.6885f);
                    }
                    if(thing1.name=="Windmill"){
                        thing1.audioClips[0]=Game.instance.audioFactory.audioClips["phoenix-move"];
                        thing1.audioClips.Add(Game.instance.audioFactory.audioClips["phoenix-clip"]);
                        thing1.audioDelays=new(2);
                        thing1.audioDelays[0]=0;
                        thing1.audioDelays[1]=0;
                        thing1.groupLimit=2;
                    }
                    if(thing1.transform.parent.name=="DartMonkeyPopUp"){
                        uObject.Destroy(thing1.transform.parent.parent.gameObject);
                        GameObject pylon=uObject.Instantiate(LoadAsset(SC2Data.Bundle,"Pylon-Prefab"),__instance.transform.GetChild(0)).Cast<GameObject>();
                        pylon.transform.localPosition=new(0,-775,25);
                        pylon.AddComponent<InteractableObject>();
                        uObject.Instantiate(LoadAsset(SC2Data.Bundle,"BloodHunter-Prefab"),pylon.transform).Cast<GameObject>().
                            AddComponent<BloodHunterBehavior>().transform.localPosition=new(-64f,0,-80);
                    }
                }
            }
        }
        [HarmonyPatch(typeof(InteractionChecker),"Update")]
        public class InteractionCheckerUpdate_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref InteractionChecker __instance){
		        if(InputSystemController.GetMouseButtonDown(0)){
		            EventSystem current = EventSystem.current;
		            GameObject m_CurrentSelected = current.m_CurrentSelected;
		            if(!uObject.Equals(m_CurrentSelected, 0)){
		                Vector2 mousePosition=InputSystemController.MousePosition;
		                Camera sceneCamera=__instance.sceneCamera;
		                int width=Screen.width;
                        int height=Screen.height;
		                Vector3 v3=new(mousePosition.x/width,mousePosition.y/height,0);
		                Ray ray=sceneCamera.ViewportPointToRay(v3);
                        if(Physics.Raycast(ray,out RaycastHit hit)){
                            GameObject gObj=hit.collider.gameObject;
                            if(gObj.HasComponent<InteractableObject>()){
                                if(gObj.name=="Pylon-Prefab(Clone)"){
                                    BloodHunterBehavior bloodHunter=gObj.GetComponentInChildren<BloodHunterBehavior>();
                                    bloodHunter.transform.localPosition=new(-64f,0,-80);
                                    foreach(var renderer in bloodHunter.GetComponentsInChildren<SkinnedMeshRenderer>()){
                                        renderer.material.SetFloat("_Visibility",0);
                                    }
                                    bloodHunter.on=true;
                                    PlaySound("bloodhunter-clip");
                                    return false;
                                }
                                gObj.GetComponent<InteractableObject>().OnInteract();
                            }
                        }
                    }
                }
                return false;
            }
        }
        [RegisterTypeInIl2Cpp]
        public class BloodHunterBehavior:MonoBehaviour{
            public BloodHunterBehavior(IntPtr ptr):base(ptr){}
            float visibility=0;
            public bool on=false;
            List<Material>materials=new();
            public void Start(){
                foreach(var renderer in GetComponentsInChildren<SkinnedMeshRenderer>()){
                    materials.Add(renderer.material);
                    renderer.material.SetFloat("_Visibility",0);
                }
            }
            public void Update(){
                if(on==true){
                    visibility=materials[0].GetFloat("_Visibility");
                    transform.localPosition=new(transform.localPosition.x+0.365f,transform.localPosition.y,transform.localPosition.z);
                    if(transform.position.x<-50){
                        foreach(Material material in materials){
                            if(visibility>=1.05){
                                visibility=1;
                                material.SetFloat("_Visibility",1);
                            }else{
                                material.SetFloat("_Visibility",visibility+0.0255f);
                            }
                        }
                        return;
                    }
                    if(transform.localPosition.x>64){
                        transform.localPosition=new(-64f,-810,0);
                        visibility=0;
                        foreach(Material material in materials){
                            material.SetFloat("_Visibility",0);
                        }
                        on=false;
                        return;
                    }
                    if(transform.localPosition.x>50){
                        foreach(Material material in materials){
                            if(visibility<=0){
                                visibility=0;
                                material.SetFloat("_Visibility",0);
                            }else{
                                material.SetFloat("_Visibility",visibility-0.0255f);
                            }
                        }
                        return;
                    }
                }
            }
        }
    }
}