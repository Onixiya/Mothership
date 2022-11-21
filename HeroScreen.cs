using Assets.Scripts.Unity.UI_New.Main.HeroSelect;
using UnityEngine;
namespace Mothership {
    public class HeroScreen{
        public static SC2Tower SC2Data=new(Bundles.Bundles.heroscreen);
        [HarmonyPatch(typeof(HeroUpgradeDetails),"BindDetails")]
        public class HeroUpgradeDetailsBindDetails_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref HeroUpgradeDetails __instance,string heroIdToUse){
                if(heroIdToUse=="Mothership-Mothership"){
                    GameObject.Instantiate(LoadAsset(SC2Data.Bundle,"Mothership-HeroScreenPrefab").Cast<GameObject>(),__instance.heroBackgroundBanner.transform).
                        transform.GetChild(2).gameObject.AddComponent<RotateCircle>();
                }
            }
            [RegisterTypeInIl2Cpp]
            public class RotateCircle:MonoBehaviour{
                public RotateCircle(IntPtr ptr):base(ptr){}
                public void Update(){
                    transform.Rotate(0,0,0.05f);
                }
            }
        }
    }
}
