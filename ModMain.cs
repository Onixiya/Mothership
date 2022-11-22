global using Assets.Scripts.Models.Towers;
global using Assets.Scripts.Utils;
global using Assets.Scripts.Unity;
global using Assets.Scripts.Models.Towers.Behaviors.Abilities;
global using Assets.Scripts.Simulation.Towers;
global using Assets.Scripts.Simulation.Towers.Weapons;
global using static Mothership.ModMain;
global using System.Collections.Generic;
global using System;
global using System.Linq;
global using UnhollowerBaseLib;
global using MelonLoader;
global using HarmonyLib;
global using BTD_Mod_Helper.Extensions;
global using BTD_Mod_Helper;
global using BTD_Mod_Helper.Api.Towers;
global using Assets.Scripts.Models.Towers.Behaviors.Attack;
global using uObject=UnityEngine.Object;
using UnityEngine;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Unity.Display;
using Assets.Scripts.Unity.Audio;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models;
using Assets.Scripts.Simulation.Objects;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Unity.UI_New.Main.HeroSelect;
using Assets.Scripts.Simulation.Input;
using Assets.Scripts.Simulation.Towers.Behaviors;
using Assets.Scripts.Simulation.Towers.Weapons.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Behaviors.Emissions;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Data;
using Assets.Scripts.Data.Skins;
using Assets.MainMenuWorld.Scripts;
using Assets.Scripts.Unity.UI_New.Main.WorldItems;
[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(Mothership.ModMain),"Mothership","1.0.1","Silentstorm")]
namespace Mothership{
    public class ModMain:BloonsTD6Mod{
        public static Il2CppStructArray<AreaType>flyingAreaType=new(4);
        public static Dictionary<string,SC2Tower>towerTypes=new();
        public static AbilityModel blankAbilityModel;
        public static AttackModel WarpAttack;
        private static MelonLogger.Instance mllog;
        public static string currentHero="";
        public static void Log(object thingtolog,string type="msg"){
            switch(type){
                case"msg":
                    mllog.Msg(thingtolog);
                    break;
                case"warn":
                    mllog.Warning(thingtolog);
                    break;
                 case"error":
                    mllog.Error(thingtolog);
                    break;
            }
        }
        public override void OnInitialize(){
            mllog=LoggerInstance;
            foreach(Type type in MelonAssembly.Assembly.GetTypes()){
                try{
                    if(type.BaseType!=null){
                        towerTypes.Add(type.Name,((SC2Tower)type.GetField("SC2Data").GetValue(null)));
                        //read comment in mothership.cs
                        towerTypes[type.Name].SoundNames=(Dictionary<string,string>)type.GetField("SoundNames").GetValue(null);
                    }
                }catch{}                     
            }
            flyingAreaType[0]=AreaType.land;
            flyingAreaType[1]=AreaType.water;
            flyingAreaType[2]=AreaType.ice;
            flyingAreaType[3]=AreaType.track;
        }
        public static uObject LoadAsset(AssetBundle bundle,string asset){
            uObject uObj=null;
            try{
                uObj=bundle.LoadAsset(asset);
            }catch{
                foreach(SC2Tower tower in towerTypes.Values){
                    tower.Bundle=AssetBundle.LoadFromMemory(tower.TowerBundle);
                }
                uObj=bundle.LoadAsset(asset);
            }
            return uObj;
        }
        public override void OnGameModelLoaded(GameModel model){
            WarpAttack=model.GetTowerFromId("EngineerMonkey-100").GetAttackModel().Duplicate();
            WarpAttack.RemoveBehavior<RotateToTargetModel>();
            WarpAttack.weapons[0].projectile.display=new(){guidRef=""};
            WarpAttack.weapons[0].projectile.GetBehavior<ArriveAtTargetModel>().expireOnArrival=false;
            WarpAttack.weapons[0].projectile.GetBehavior<ArriveAtTargetModel>().altSpeed=400;
            WarpAttack.weapons[0].projectile.GetBehavior<DisplayModel>().delayedReveal=1;
            WarpAttack.weapons[0].projectile.GetBehavior<DisplayModel>().positionOffset=new(0,0,190);
            WarpAttack.GetBehavior<RandomPositionModel>().minDistance=70;
            WarpAttack.GetBehavior<RandomPositionModel>().maxDistance=90;
            WarpAttack.GetBehavior<RandomPositionModel>().idealDistanceWithinTrack=0;
            WarpAttack.GetBehavior<RandomPositionModel>().useInverted=false;
            blankAbilityModel=model.GetTowerFromId("Quincy 4").GetAbility().Duplicate();
            blankAbilityModel.description="AbilityDescription";
            blankAbilityModel.displayName="AbilityDisplayName";
            blankAbilityModel.name="AbilityName";
            blankAbilityModel.RemoveBehavior<TurboModel>();
            blankAbilityModel.RemoveBehavior<CreateEffectOnAbilityModel>();
            blankAbilityModel.RemoveBehavior<CreateSoundOnAbilityModel>();
        }
        [HarmonyPatch(typeof(Factory.__c__DisplayClass21_0),"_CreateAsync_b__0")]
        public class FactoryCreateAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(ref Factory.__c__DisplayClass21_0 __instance,ref UnityDisplayNode prototype){
                string TowerName=__instance.objectId.guidRef.Split('-')[0];
                if(TowerName!=null&&towerTypes.ContainsKey(TowerName)){
                    GameObject gObj=uObject.Instantiate(LoadAsset(towerTypes[TowerName].Bundle,__instance.objectId.guidRef).Cast<GameObject>(),__instance.__4__this.DisplayRoot);
                    gObj.name=__instance.objectId.guidRef;
                    gObj.transform.position=new(0,0,30000);
                    gObj.AddComponent<UnityDisplayNode>();
                    gObj.AddComponent<SC2Sound>();
                    prototype=gObj.GetComponent<UnityDisplayNode>();
                    __instance.__4__this.active.Add(prototype);
                    __instance.onComplete.Invoke(prototype);
                    return false;
                }
                return true;                
            }
        }
        [HarmonyPatch(typeof(UnityEngine.U2D.SpriteAtlas),"GetSprite")]
        public class SpriteAtlasGetSprite_Patch{
            [HarmonyPostfix]
            public static void Postfix(string name,ref Sprite __result){
                string towerName="";
                try{
                    towerName=name.Split('-')[0];
                }catch{}
                if(towerTypes.ContainsKey(towerName)){
                    Texture2D texture=LoadAsset(towerTypes[towerName].Bundle,name).Cast<Texture2D>();
                    __result=Sprite.Create(texture,new(0,0,texture.width,texture.height),new());
                }
            }
        }
        public override void OnWeaponFire(Weapon weapon){
            string towerName;
            try{
                towerName=weapon.attack.tower.towerModel.baseId.Split('-')[1];
            }catch{
                return;
            }
            if(towerTypes.ContainsKey(towerName)&&towerTypes[towerName].Attack!=null){
                towerTypes[towerName].Attack(weapon);
            }
        }
        public override void OnTowerCreated(Tower tower,Entity target,Model modelToUse){
            string towerName;
            try{
                towerName=tower.towerModel.baseId.Split('-')[1];
            }catch{
                return;
            }
            if(towerTypes.ContainsKey(towerName)&&towerTypes[towerName].Create!=null){
                towerTypes[towerName].Create();
            }
        }
        public override void OnAbilityCast(Ability ability){
            string towerName=ability.tower.towerModel.baseId.Split('-')[0];
            if(towerTypes.ContainsKey(towerName)&&towerTypes[towerName].Ability!=null){
                towerTypes[towerName].Ability(ability.abilityModel.name,ability.tower);
            }
        }
        public override void OnAudioFactoryStart(AudioFactory audioFactory) {
            AssetBundle bundle=AssetBundle.LoadFromMemory(Bundles.Bundles.audioclips);
            foreach(string asset in bundle.GetAllAssetNames()){
                string audioClip=asset.Split('/').Last().Remove(asset.Split('/').Last().Length-4);
                audioFactory.RegisterAudioClip(audioClip,bundle.LoadAsset(audioClip).Cast<AudioClip>());
            }
            bundle.Unload(false);
        }
        public override void OnTowerUpgraded(Tower tower,string upgradeName,TowerModel newBaseTowerModel){
            string towerName;
            try{
                towerName=tower.towerModel.baseId.Split('-')[1];
            }catch{
                return;
            }
            if(towerTypes.ContainsKey(towerName)&&towerTypes[towerName].Upgrade!=null){
                towerTypes[towerName].Upgrade(upgradeName,tower);
            }
        }
        public static void PlaySound(string name){
            Game.instance.audioFactory.PlaySoundFromUnity(null,name,"FX",1,1);
        }
        public static void PlayAnimation(UnityDisplayNode udn,string anim){
            udn.GetComponent<Animator>().Play(anim);
        }
        public override void OnTowerSelected(Tower tower){
            string towerName;
            try{
                towerName=tower.towerModel.baseId.Split('-')[1];
            }catch{
                return;
            }
            if(towerTypes.ContainsKey(towerName)&&towerTypes[towerName].Select!=null){
                towerTypes[towerName].Select(tower);
            }
        }
        public override void OnTowerSold(Tower tower,float amount){
            string towerName;
            try{
                towerName=tower.towerModel.baseId.Split('-')[1];
            }catch{
                return;
            }
            if(towerTypes.ContainsKey(towerName)&&towerTypes[towerName].Sell!=null){
                towerTypes[towerName].Sell(tower);
            }
        }
        [HarmonyPatch(typeof(CommonForegroundScreenHeroButton),"LoadIcon")]
        public class CommonForegroundScreenHeroButtonLoadIcon_Patch{
            [HarmonyPostfix]
            public static void Postfix(string heroSkin){
                try{
                    currentHero=heroSkin.Split('-')[1].ToLower();
                }catch{}
            }
        }
        [HarmonyPatch(typeof(TowerInventory),"CreatedTower")]
        public class TowerInventoryCreatedTower_Patch{
            [HarmonyPrefix]
            public static bool Prefix(TowerInventory __instance,TowerModel def){
                if(!__instance.towerCounts.ContainsKey(def.baseId)){
                    __instance.towerCounts.Add(def.baseId,0);
                }
		        __instance.towerCounts[def.baseId]=__instance.towerCounts[def.baseId]+1;
		        return false;
            }
        }
        [HarmonyPatch(typeof(TowerInventory),"DestroyedTower")]
        public class TowerInventoryDestroyedTower_Patch{
            [HarmonyPrefix]
            public static bool Prefix(TowerInventory __instance,TowerModel def){
                if(!__instance.towerCounts.ContainsKey(def.baseId)){
                    __instance.towerCounts.Add(def.baseId,0);
                }
		        __instance.towerCounts[def.baseId]=__instance.towerCounts[def.baseId]-1;
		        return false;
            }
        }
        [HarmonyPatch(typeof(FighterMovement),"Process")]
        public class FighterMovementProcess_Patch{
            [HarmonyPrefix]
            public static bool Prefix(FighterMovement __instance,int elapsed){
                __instance.timer++;
		        if(__instance.flyoverEngaged==false){
                    __instance.timer=0;
		        }
		        __instance.ApplyMovement(new(0,0),elapsed);
                return false;
            }
        }
        [HarmonyPatch(typeof(SubTowerFilter),nameof(SubTowerFilter.FilterEmission))]
        public class SubTowerFilterFilterEmission_Patch{
            [HarmonyPrefix]
            public static bool Prefix(SubTowerFilter __instance,ref bool __result){
                if(__instance.createdSubTowers.Count>=__instance.subTowerFilterModel.maxNumberOfSubTowers){
                    __result=false;
                }else{
                    __result=true;
                }
                return false;
            }
        }
    }
}