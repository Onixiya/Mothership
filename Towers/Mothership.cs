using Assets.Scripts.Models.GenericBehaviors;
using Assets.Scripts.Models.Towers.Projectiles.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Assets.Scripts.Models.Towers.Filters;
using Assets.Scripts.Models.Towers.Projectiles;
using Assets.Scripts.Models.Towers.Weapons;
using Assets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Simulation.Towers.Behaviors.Abilities.Behaviors;
using Assets.Scripts.Models.Towers.Behaviors;
using Assets.Scripts.Data.Skins;

namespace Mothership{
    public class Mothership:ModHero{
        public override string BaseTower=>"WizardMonkey";
        public override int Cost=>975;
        public override string DisplayName=>"Mothership";
        public override string Title=>"Capital Ship";
        public override string Level1Description=>"Fires charged psionic bolts at bloons";
        public override string Description=>"Capital ships often found leading the Tal'darim Death Fleet";
        public override string NameStyle=>TowerType.Gwendolin;
        public override string BackgroundStyle=>TowerType.Gwendolin;
        public override string GlowStyle=>TowerType.Gwendolin;
        protected override int Order=>2;
        public override Dictionary<int,SpriteReference>SelectScreenPortraits=>new(){
            {0,PortraitReference},
            {1,PortraitReference},
            {10,new(){guidRef="Ui[Mothership-Portrait]"}},
            {20,new(){guidRef="Ui[Mothership-FathershipPortrait]"}}
        };
        public override string SelectSound=>"mothership-birth";
        public override int MaxLevel=>20;
        public override float XpRatio=>2.5f;
        public override SpriteReference PortraitReference=>new(){guidRef="Ui[Mothership-CorePortrait]"};
        public override SpriteReference ButtonReference=>new(){guidRef="Ui[Mothership-Button]"};
        public override SpriteReference SquareReference=>new(){guidRef="Ui[Mothership-HeroIcon]"};
        public override SpriteReference IconReference=>new(){guidRef="Ui[Mothership-Icon]"};
        //incredibly fucking annoying having to split sound shit into its own separate field but only way it appears to work
        public static Dictionary<string,string>SoundNames=new(){{"Core","no"},{"Mothership","mothership-"},{"Fathership","mothership-"}};
        public static SC2Tower SC2Data=new(Bundles.Bundles.mothership,/*null,*/null,Upgrade,Ability,Create,Select,Sell);
        public override void ModifyBaseTowerModel(TowerModel mothership){
            AttackModel repulsor=mothership.GetAttackModel();
            WeaponModel repulsorWeapon=repulsor.weapons[0];
            ProjectileModel repulsorProjectile=repulsorWeapon.projectile;
            DamageModel repulsorDamage=repulsorProjectile.GetDamageModel();
            DisplayModel mothershipDisplay=mothership.GetBehavior<DisplayModel>();
            repulsor.range=40;
            repulsor.GetBehavior<RotateToTargetModel>().rotateTower=false;
            repulsorWeapon.rate=0.8f;
            repulsorProjectile.pierce=1;
            repulsorProjectile.GetBehavior<TravelStraitModel>().speed*=2;
            repulsorDamage.damage=1;
            repulsorDamage.immuneBloonProperties=(BloonProperties)8;
            mothershipDisplay.positionOffset=new(0,0,200);
            mothershipDisplay.display=new(){guidRef="Mothership-Core-Prefab"};
            mothership.doesntRotate=true;
            mothership.radius=1;
            mothership.areaTypes=flyingAreaType;
            mothership.range=repulsor.range;
            mothership.display=new(){guidRef=mothershipDisplay.display.guidRef};
        }
        public class L2:ModHeroLevel<Mothership>{
            public override string Description=>"Further range";
            public override int Level=>2;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.range+=7;
                mothership.GetAttackModel().range=mothership.range;
            }
        }
        public class L3:ModHeroLevel<Mothership>{
            public override string Description=>"Increases damage dealt";
            public override int Level=>3;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=1;
            }
        }
        public class L4:ModHeroLevel<Mothership>{
            public override string AbilityName=>"Time Warp";
            public override string AbilityDescription=>"Temporarily warps time and space in a small area to force bloons to half their speed";
            public override string Description=>AbilityName+": "+AbilityDescription;
            public override int Level=>4;
            public override void ApplyUpgrade(TowerModel mothership){
                AbilityModel timeWarp=blankAbilityModel.Duplicate();
                AttackModel timeWarpAttack=Game.instance.model.GetTowerFromId("WizardMonkey-020").GetAttackModels().First(a=>a.name.Contains("Wall")).Duplicate();
                ProjectileModel timeWarpProjectile=timeWarpAttack.weapons[0].projectile;
                ProjectileFilterModel timeWarpProjFilter=timeWarpProjectile.GetBehavior<ProjectileFilterModel>();
                SlowModel slowModel=Game.instance.model.GetTowerFromId("GlueGunner").Duplicate().GetAttackModel().weapons[0].projectile.GetBehavior<SlowModel>();
                SlowModel.SlowMutator slowMutator=slowModel.Mutator;
                timeWarp.name=AbilityName;
                timeWarp.displayName=AbilityName;
                timeWarp.description=AbilityDescription;
                timeWarp.icon=new(){guidRef="Ui[Mothership-TimeWarpIcon]"};
                timeWarp.AddBehavior(Game.instance.model.GetTowerFromId("SuperMonkey-040").GetAbility().GetBehavior<ActivateAttackModel>().Duplicate());
                ActivateAttackModel timeWarpActivateAttack=timeWarp.GetBehavior<ActivateAttackModel>();
                timeWarpActivateAttack.turnOffExisting=false;
                timeWarpActivateAttack.attacks[0]=timeWarpAttack;
                timeWarp.addedViaUpgrade=Id;
                timeWarpAttack.name="TimeWarp";
                timeWarpAttack.range=mothership.range;
                timeWarpAttack.RemoveBehavior<RotateToTargetModel>();
                timeWarpProjectile.GetBehavior<AgeModel>().Lifespan=5;
                timeWarpProjectile.RemoveBehavior<DamageModel>();
                timeWarpProjectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
                timeWarpProjectile.display=new(){guidRef="Mothership-TimeWarpPrefab"};
                timeWarpProjectile.GetBehavior<DisplayModel>().display=new(){guidRef=timeWarpProjectile.display.guidRef};
                timeWarpProjFilter.filters=timeWarpProjFilter.filters.AddTo(new FilterOutTagModel(null,null,null){name="FilterOutTagModel",tag="Moabs",disableWhenSupportMutatorIDs=new(0)});
                timeWarpProjectile.AddBehavior(slowModel);
                timeWarpProjectile.collisionPasses[0]=-1;
                slowModel.mutationId="TimeWarp-Slow";
                slowModel.countGlueAchievement=false;
                slowModel.Lifespan=1;
                slowModel.overlayType="";
                slowModel.overlayLayer=0;
                slowModel.glueLevel=99999;
                slowModel.Multiplier=0.85f;
                slowMutator.id=slowModel.mutationId;
                slowMutator.glueLevel=slowModel.glueLevel;
                slowMutator.mutationId=slowModel.mutationId;
                slowMutator.overlayType=slowModel.overlayType;
                slowMutator.multiplier=slowModel.Multiplier;
                mothership.AddBehavior(timeWarp);
            }
        }
        public class L5:ModHeroLevel<Mothership>{
            public override string Description=>"Adds more pierce and faster attack speed";
            public override int Level=>5;
            public override void ApplyUpgrade(TowerModel mothership){
                WeaponModel repulsorWeapon=mothership.GetAttackModel().weapons[0];
                repulsorWeapon.projectile.pierce+=2;
                repulsorWeapon.rate-=0.15f;
            }
        }
        public class L6:ModHeroLevel<Mothership>{
            public override string Description=>"Even further range";
            public override int Level=>6;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.range+=8;
                mothership.GetAttackModel().range=mothership.range;
                mothership.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].range=mothership.range;
            }
        }
        public class L7:ModHeroLevel<Mothership>{
            public override string Description=>"Deals more damage";
            public override int Level=>7;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.GetAttackModel().weapons[0].projectile.GetDamageModel().damage+=2;
            }
        }
        public class L8:ModHeroLevel<Mothership>{
            public override string AbilityName=>"Blink";
            public override string AbilityDescription=>"Teleports to a nearby area";
            public override string Description=>AbilityName+": "+AbilityDescription;
            public override int Level=>8;
            public override void ApplyUpgrade(TowerModel mothership) {
                AbilityModel blink=blankAbilityModel.Duplicate();
                blink.AddBehavior(Game.instance.model.GetTowerFromId("SuperMonkey-003").GetAbility().GetBehavior<DarkshiftModel>().Duplicate());
                DarkshiftModel teleModel=blink.GetBehavior<DarkshiftModel>();
                blink.name=AbilityName;
                blink.displayName=AbilityName;
                blink.description=AbilityDescription;
                blink.cooldown=50;
                blink.icon=new(){guidRef="Ui[Mothership-BlinkIcon]"};
                blink.addedViaUpgrade=Id;
                teleModel.disappearEffectModel.assetId=new(){guidRef=""};
                teleModel.reappearEffectModel.assetId=new(){guidRef=""};
                mothership.AddBehavior(blink);
            }
        }
        public class L9:ModHeroLevel<Mothership>{
            public override string Description=>"Time warp affects up to BFB's";
            public override int Level=>9;
            public override void ApplyUpgrade(TowerModel mothership){
                ProjectileFilterModel filter=mothership.GetAbilities().First(a=>a.name=="Time Warp").GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.
                    GetBehavior<ProjectileFilterModel>();
                filter.filters=filter.filters.AddTo(filter.filters[1].Duplicate());
                filter.filters[1].Cast<FilterOutTagModel>().tag="ZOMG";
                filter.filters[2].Cast<FilterOutTagModel>().tag="BAD";
            }
        }
        public class L10:ModHeroLevel<Mothership>{
            public override string Description=>"Upgrades to a Mothership. Victory has come";
            public override int Level=>10;
            public override void ApplyUpgrade(TowerModel mothership){
                AttackModel terminator=mothership.GetAttackModel();
                terminator.weapons[0]=Game.instance.model.GetTowerFromId("BallOfLightTower").GetAttackModel().weapons[0].Duplicate();
                WeaponModel terminatorWeapon=terminator.weapons[0];
                terminator.range=mothership.range;
                terminator.addsToSharedGrid=false;
                terminatorWeapon.ejectX=10;
                terminatorWeapon.ejectY=10;
                terminatorWeapon.rate=1;
                terminatorWeapon.projectile.GetDamageModel().damage=30;
                for(int i=0;i<5;i++){
                    terminator.weapons=terminator.weapons.AddTo(terminatorWeapon.Duplicate());
                }
                terminator.weapons[1].ejectX=0;
                terminator.weapons[2].ejectX=-10;
                terminator.weapons[5].ejectX=10;
                terminator.weapons[3].ejectY=0;
                terminator.weapons[4].ejectY=-10;
                terminator.weapons[5].ejectY=10;
                mothership.range+=50;
                mothership.GetAbilities().First(a=>a.name=="Time Warp").GetBehavior<ActivateAttackModel>().attacks[0].range=mothership.range;
                mothership.display=new(){guidRef="Mothership-Mothership-Prefab"};
                mothership.GetBehavior<DisplayModel>().display=new(){guidRef=mothership.display.guidRef};
            }
        }
        public class L11:ModHeroLevel<Mothership>{
            public override string Description=>"Blink no longer has a range limit and decreases the cooldown";
            public override int Level=>11;
            public override void ApplyUpgrade(TowerModel mothership){
                AbilityModel blink=mothership.GetAbilities().First(a=>a.name=="Blink");
                blink.GetBehavior<DarkshiftModel>().restrictToTowerRadius=false;
                blink.cooldown-=15;
            }
        }
        public class L12:ModHeroLevel<Mothership>{
            public override string Description=>"Increases the duration and range of Time Warp";
            public override int Level=>12;
            public override void ApplyUpgrade(TowerModel mothership){
                ProjectileModel timeWarpProj=mothership.GetAbilities().First(a=>a.name=="Time Warp").GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile;
                timeWarpProj.radius+=10;
                timeWarpProj.GetBehavior<AgeModel>().lifespan+=10;
                timeWarpProj.GetBehavior<DisplayModel>().scale=1.5f;
            }
        }
        public class L13:ModHeroLevel<Mothership>{
            public override string Description=>"Faster attack speed";
            public override int Level=>13;
            public override void ApplyUpgrade(TowerModel mothership){
                foreach(WeaponModel weaponModel in mothership.GetAttackModel().weapons){
                    weaponModel.rate-=0.2f;
                }
            }
        }
        public class L14:ModHeroLevel<Mothership>{
            public override string Description=>"Increases attack range";
            public override int Level=>14;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.range+=15;
                mothership.GetAbilities().First(a=>a.name=="Time Warp").GetBehavior<ActivateAttackModel>().attacks[0].range=mothership.range;
                mothership.GetAttackModel().range=mothership.range;
            }
        }
        public class L15:ModHeroLevel<Mothership>{
            public override string Description=>"Detects camo bloons";
            public override int Level=>15;
            public override void ApplyUpgrade(TowerModel mothership){
                AttackModel Terminator=mothership.GetAttackModel();
                Terminator.GetBehavior<AttackFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive=false;
                foreach(WeaponModel weapon in Terminator.weapons){
                    weapon.projectile.filters[0].Cast<FilterInvisibleModel>().isActive=false;
                    weapon.projectile.GetBehavior<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive=false;
                }
            }
        }
        public class L16:ModHeroLevel<Mothership>{
            public override string AbilityName=>"Summon Death Fleet";
            public override string AbilityDescription=>"Warps in 4 temporary Destroyers with beefed up range and damage";
            public override string Description=>AbilityName+": "+AbilityDescription;
            public override int Level=>16;
            public override void ApplyUpgrade(TowerModel mothership){
                AttackModel destroyerWarp=WarpAttack.Duplicate();
                WeaponModel destroyerWarpWeapon=destroyerWarp.weapons[0];
                ProjectileModel destroyerWarpProj=destroyerWarpWeapon.projectile;
                AbilityModel deathFleet=blankAbilityModel.Duplicate();
                destroyerWarp.name="DestroyerWarp";
                destroyerWarp.range=mothership.range;
                destroyerWarpWeapon.rate=0.025f;
                destroyerWarpProj.display=new(){guidRef="Destroyer-WarpPrefab"};
                destroyerWarpProj.GetBehavior<DisplayModel>().display=new(){guidRef="Destroyer-WarpPrefab"};
                destroyerWarpProj.GetBehavior<CreateTowerModel>().tower=Game.instance.model.GetTowerFromId("Mothership-Destroyer");
                deathFleet.AddBehavior(Game.instance.model.GetTowerFromId("SuperMonkey-040").GetAbility().GetBehavior<ActivateAttackModel>().Duplicate());
                ActivateAttackModel deathFleetAttack=deathFleet.GetBehavior<ActivateAttackModel>();
                deathFleetAttack.turnOffExisting=false;
                deathFleetAttack.attacks[0]=destroyerWarp;
                deathFleetAttack.lifespan=0.1f;
                deathFleetAttack.isOneShot=false;
                deathFleet.name="Death Fleet";
                deathFleet.icon=new(){guidRef="Ui[Mothership-DeathFleetIcon]"};
                deathFleet.displayName="Summon Death Fleet";
                deathFleet.cooldown=80;
                deathFleet.addedViaUpgrade=Id;
                mothership.AddBehavior(deathFleet);
            }
        }
        public class L17:ModHeroLevel<Mothership>{
            public override string Description=>"Decreases Time Warp cooldown";
            public override int Level=>17;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.GetAbilities().First(a=>a.name=="Time Warp").cooldown-=20;
            }
        }
        public class L18:ModHeroLevel<Mothership>{
            public override string Description=>"All bloon types can now be damaged and affected by abilities";
            public override int Level=>18;
            public override void ApplyUpgrade(TowerModel mothership){
                foreach(var weapon in mothership.GetAttackModel().weapons){
                    weapon.projectile.GetDamageModel().immuneBloonProperties=0;
                }
                mothership.GetAbilities().First(a=>a.name=="Time Warp").GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].projectile.RemoveBehavior<ProjectileFilterModel>();
            }
        }
        public class L19:ModHeroLevel<Mothership>{
            public override string Description=>"Summonning the Death Fleet warps in 3 more Destroyers";
            public override int Level=>19;
            public override void ApplyUpgrade(TowerModel mothership){
                mothership.GetAbilities().First(a=>a.name=="Death Fleet").GetBehavior<ActivateAttackModel>().attacks[0].weapons[0].rate=0.0125f;
            }
        }
        public class L20:ModHeroLevel<Mothership>{
            public override string Description=>"Upgrades to a Fathership. Ultimate expression of Tal'darim might";
            public override int Level=>20;
            public override void ApplyUpgrade(TowerModel mothership){
                AttackModel carrierWarp=WarpAttack.Duplicate();
                WeaponModel carrierWarpWeapon=carrierWarp.weapons[0];
                ProjectileModel carrierWarpProj=carrierWarpWeapon.projectile;
                AttackModel phoenixWarp=WarpAttack.Duplicate();
                WeaponModel phoenixWarpWeapon=phoenixWarp.weapons[0];
                ProjectileModel phoenixWarpProj=phoenixWarpWeapon.projectile;
                AbilityModel deathFleet=mothership.GetAbilities().First(a=>a.name=="Death Fleet");
                ActivateAttackModel deathFleetAttack=deathFleet.GetBehavior<ActivateAttackModel>();
                AttackModel timeWarp=mothership.GetAbility().GetBehavior<ActivateAttackModel>().attacks[0];
                carrierWarp.name="CarrierWarp";
                carrierWarp.range=mothership.range;
                carrierWarpWeapon.rate=0.06f;
                carrierWarpProj.display=new(){guidRef="Carrier-WarpPrefab"};
                carrierWarpProj.GetBehavior<DisplayModel>().display=new(){guidRef="Carrier-WarpPrefab"};
                carrierWarpProj.GetBehavior<CreateTowerModel>().tower=Game.instance.model.GetTowerFromId("Mothership-Carrier");
                phoenixWarp.name="PhoenixWarp";
                phoenixWarp.range=mothership.range;
                phoenixWarpWeapon.rate=0.025f;
                phoenixWarpProj.display=new(){guidRef="Phoenix-WarpPrefab"};
                phoenixWarpProj.GetBehavior<DisplayModel>().display=new(){guidRef="Phoenix-WarpPrefab"};
                phoenixWarpProj.GetBehavior<CreateTowerModel>().tower=Game.instance.model.GetTowerFromId("Mothership-Phoenix");
                deathFleet.cooldown-=30;
                deathFleetAttack.attacks=deathFleetAttack.attacks.AddTo(carrierWarp);
                deathFleetAttack.attacks=deathFleetAttack.attacks.AddTo(phoenixWarp);
                timeWarp.weapons[0].rate=14;
                timeWarp.weapons[0].projectile.radius*=2;
                timeWarp.weapons[0].projectile.GetBehavior<DisplayModel>().scale*=2;
                foreach(var weapon in mothership.GetAttackModel().weapons){
                    weapon.rate-=0.2f;
                    weapon.projectile.GetDamageModel().damage*=1.5f;
                }
                mothership.AddBehavior(timeWarp);
                mothership.RemoveBehavior(mothership.GetAbility());
                mothership.AddBehavior(Game.instance.model.GetTowerFromId("Mothership-Carrier").GetAttackModel().Duplicate());
                mothership.GetAbility().cooldown-=10;
                mothership.display=new(){guidRef="Mothership-Fathership-Prefab"};
                mothership.GetBehavior<DisplayModel>().display=new(){guidRef=mothership.display.guidRef};
            }
        }
        [HarmonyPatch(typeof(Darkshift),"GetCustomInputData")]
        public class thing{
            [HarmonyPostfix]
            public static void Postfix(Darkshift __instance,Il2CppSystem.Object __result){
                TowerModel towerModel=__instance.ability.tower.towerModel;
                if(towerModel.name.Contains("Mothership")){
                    if(towerModel.tier>10){
                        __result.Cast<RepositionTowerCIData>().helperMsg="Click to Blink to any valid location";
                    }else{
                        __result.Cast<RepositionTowerCIData>().helperMsg="Click to Blink within range";
                    }
                }
            }
        }
        public static bool Upgraded=false;
        public static void Create(){
            PlaySound("mothership-core"+new Random().Next(1,9));
        }
        public static void Upgrade(string upgradeName,Tower tower){
            if(int.Parse(upgradeName.Split(' ').Last())>=10){
                if(Upgraded==false){
                    PlaySound("mothership-birth");
                    Upgraded=true;
                    return;
                }
                tower.Node.graphic.GetComponent<SC2Sound>().PlayUpgradeSound();
                return;
            }
            PlaySound("mothership-core"+new Random().Next(1,9));
        }
        public static void Select(Tower tower){ 
            if(Upgraded){
                tower.Node.graphic.GetComponent<SC2Sound>().PlaySelectSound();
                return;
            }
            PlaySound("mothership-core"+new Random().Next(1,9));
        }
        public static void Ability(string abilityName,Tower tower){
            switch(abilityName){
                case"AbilityModel_Time Warp":
                    if(Upgraded){
                        PlaySound("mothership-timewarp");
                    }else{
                        PlaySound("mothership-core"+new Random().Next(1,9));
                    }
                    break;
                case"AbilityModel_Blink":
                    if(Upgraded){
                        PlaySound("mothership-blink");
                    }else{
                        PlaySound("mothership-core"+new Random().Next(1,9));
                    }
                    break;
                case"AbilityModel_Death Fleet":
                    PlaySound("mothership-deathfleet");
                    break;
            }
        }
        public static void Sell(Tower tower){
            Upgraded=false;
        }
    }
}
