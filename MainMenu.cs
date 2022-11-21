using Assets.MainMenuWorld.Scripts;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Unity.UI_New.Main.WorldItems;
using UnityEngine;
namespace Mothership{
    public class MainMenu{
        public static SC2Tower SC2Data=new(Bundles.Bundles.mainmenu);
        [HarmonyPatch(typeof(MainMenuWorldChoreographer),"Start")]
        public class MainMenuWorldChoreographerStart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref MainMenuWorldChoreographer  __instance){
                var thing=__instance.GetComponentsInChildren<PlayInteractableAudio>();
                for(int i=0;thing.Count>0;i++){
                    var thing1=thing[i];
                    if(thing1.transform.parent.name=="MonkeyAcePopup"){
                        uObject.Destroy(thing1.transform.GetChild(0).gameObject);
                        uObject.Instantiate(LoadAsset(towerTypes["Phoenix"].Bundle,"Phoenix-Phoenix-Prefab"),thing1.transform).Cast<GameObject>().
                            transform.localPosition=new(0.102f,3.159f,4.6885f);
                        return;
                    }
                    if(thing1.name=="Windmill"){
                        thing1.audioClips[0]=Game.instance.audioFactory.audioClips["phoenix-move"];
                        thing1.audioClips.Add(Game.instance.audioFactory.audioClips["phoenix-clip"]);
                        thing1.audioDelays=new(2);
                        thing1.audioDelays[0]=0;
                        thing1.audioDelays[1]=0;
                        thing1.groupLimit=2;
                        return;
                    }
                    if(thing1.transform.parent.name=="DartMonkeyPopUp"){
                        uObject.Destroy(thing1.transform.parent.parent.gameObject);
                        uObject.Instantiate(LoadAsset(SC2Data.Bundle,"BloodHunter-Prefab"),__instance.transform.GetChild(0)).
                            Cast<GameObject>().AddComponent<BloodHunterBehavior>().transform.localPosition=new(-64f,-810,0);
                        return;
                    }
                }
            }
        }
        [RegisterTypeInIl2Cpp]
        public class BloodHunterBehavior:MonoBehaviour{
            public BloodHunterBehavior(IntPtr ptr):base(ptr){}
            float visibility=0;
            bool idfk=false;
            List<Material>materials=new();
            public void Start(){
                foreach(var thing in GetComponentsInChildren<SkinnedMeshRenderer>()){
                    materials.Add(thing.material);
                    thing.material.SetFloat("_Visibility",0);
                }
                var alchmorph = Game.instance.model.GetTower(TowerType.Alchemist, 0, 5).GetAbility().GetBehavior<MorphTowerModel>().Duplicate();
                var morph = new MorphTowerModel("MorphModel", true, 9999999, " RoboMonkeyFanCLub", 20f, true, true,

                null, Game.instance.model.GetTower(TowerType.PatFusty, 20),
                alchmorph.effectModel, 3, 9999999f, 20, "DartMonkey",
                alchmorph.effectOnTransitionBackModel,
                alchmorph.resetOnDefeatScreen, alchmorph.ignoreWithMutators);
            }
            public void Update(){
                if(Input.GetKeyDown(KeyCode.H)){
                    idfk=true;
                }
                if(idfk==true){
                    visibility=materials[0].GetFloat("_Visibility");
                    transform.localPosition=new(transform.localPosition.x+0.365f,transform.localPosition.y,transform.localPosition.z);
                    if(transform.position.x<-50){
                        foreach(Material material in materials){
                            if(visibility>=1){
                                visibility=1;
                                material.SetFloat("_Visibility",1);
                            }else{
                                material.SetFloat("_Visibility",visibility+0.02f);
                            }
                        }
                        Log(transform.localPosition.x);
                        return;
                    }
                    if(transform.localPosition.x>64){
                        transform.localPosition=new(-64f,-810,0);
                        visibility=0;
                        foreach(Material material in materials){
                            material.SetFloat("_Visibility",0);
                        }
                        idfk=false;
                        return;
                    }
                    if(transform.localPosition.x>50){
                        foreach(Material material in materials){
                            if(visibility<=0){
                                visibility=0;
                                material.SetFloat("_Visibility",0);
                            }else{
                                material.SetFloat("_Visibility",visibility-0.02f);
                            }
                        }
                        return;
                    }
                }
            }
        }
    }
}