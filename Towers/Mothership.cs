using Il2CppAssets.Scripts.Unity.Bridge;

namespace Mothership{
    public class Mothership:SC2Tower{
        public override string Name=>"Mothership";
        public override string Description=>"Capital ships often found leading the Tal'darim Death Fleet";
        public override Faction TowerFaction=>Faction.Protoss;
		public override bool ShowUpgradeMenu=>false;
		public override bool AddToShop=>false;
		public override bool Hero=>true;
		public override Dictionary<string,Il2CppSystem.Type>Components=>new(){{"Mothership-Prefab",Il2CppType.Of<MothershipBehaviour>()}};
		public override int Order=>2;
		//has to be stored outside of the behaviour as the gameobject will be instantiated again on the blink ability and can fuck with things
		public static string ActiveObj="Core";
        [RegisterTypeInIl2Cpp]
		public class MothershipBehaviour:MonoBehaviour{
			public MothershipBehaviour(IntPtr ptr):base(ptr){}
			int SelectSound=0;
			int UpgradeSound=0;
			public GameObject Active;
			public GameObject Core;
			public GameObject Mothership;
			public GameObject Fathership;
			public bool Upgraded=false;
			void Start(){
				Core=transform.GetChild(0).gameObject;
				Mothership=transform.GetChild(1).gameObject;
				Fathership=transform.GetChild(2).gameObject;
				Core.SetActive(false);
				Mothership.SetActive(false);
				Fathership.SetActive(false);
				switch(ActiveObj){
					case"Core":
						Active=Core;
						Core.SetActive(true);
						break;
					case"Mothership":
						Active=Mothership;
						Mothership.SetActive(true);
						break;
					case"Fathership":
						Active=Fathership;
						Fathership.SetActive(true);
						break;
				}
				Core.transform.localPosition=new(0,0,0);
				Mothership.transform.localPosition=new(0,0,0);
				Fathership.transform.localPosition=new(0,0,0);
			}
			public void PlaySelectSound(){
				if(SelectSound>8){
					SelectSound=0;
				}
				SelectSound+=1;
				PlaySound("Mothership-Select"+SelectSound);
			}
			public void PlayUpgradeSound(){
				if(UpgradeSound>7){
					UpgradeSound=0;
				}
				UpgradeSound+=1;
				SelectSound=0;
				PlaySound("Mothership-Upgrade"+UpgradeSound);
			}
		}
        public override TowerModel[]GenerateTowerModels(){
            return new TowerModel[]{
                Base(),
				Level2(),
				Level3(),
				Level4(),
				Level5(),
				Level6(),
				Level7(),
				Level8(),
				Level9(),
				Level10(),
				Level11(),
				Level12(),
				Level13(),
				Level14(),
				Level15(),
				Level16(),
				Level17(),
				Level18(),
				Level19(),
				Level20()
            };
		}
		//did some basic shit with desmos and figured out the xp ratio for pat fusty, 1.55
		public override UpgradeModel[]GenerateUpgradeModels(){
			List<UpgradeModel>upgradeList=new();
			upgradeList.Add(new("Mothership Level 2",0,255,new(),0,1,0,"","Mothership Level 2"));
			for(int i=3;i<21;i++){
				if(i>9){
					//avoids it going too high
					upgradeList.Add(new("Mothership Level "+i,0,(int)Math.Round(upgradeList.Last().xpCost*1.1),new(),0,i-1,0,"","Mothership Level "+i));
				}else{
					upgradeList.Add(new("Mothership Level "+i,0,(int)Math.Round(upgradeList.Last().xpCost*1.55),new(),0,i-1,0,"","Mothership Level "+i));
				}
			}
			return upgradeList.ToArray();
		}
		//am tempted to turn some of this into how mod helper does it to avoid hard coding it like this
		//but i don't want to make it seem like i'm copying how it does it one for one
		public override HeroDetailsModel GenerateHeroDetails(){
			LocManager.textTable.Add("Mothership Description",Description);
            LocManager.textTable.Add("Mothership Short Description","Capital Ship");
			LocManager.textTable.Add("Mothership Level 1 Description","Fires charged psionic bolts at bloons");
			LocManager.textTable.Add("Mothership Level 2 Description","Further range");
			LocManager.textTable.Add("Mothership Level 3 Description","Increases damage dealt");
			LocManager.textTable.Add("Mothership Level 4 Description","Time Warp: Temporarily warps time and space in a small area to force bloons to half their speed");
			LocManager.textTable.Add("Mothership Level 5 Description","Adds more pierce and faster attack speed");
			LocManager.textTable.Add("Mothership Level 6 Description","Even further range");
			LocManager.textTable.Add("Mothership Level 7 Description","Deals more damage");
			LocManager.textTable.Add("Mothership Level 8 Description","Blink: Teleports to a nearby area");
			LocManager.textTable.Add("Mothership Level 9 Description","Time warp affects up to BFB's");
			LocManager.textTable.Add("Mothership Level 10 Description","Upgrades to a Mothership. Victory has come");
			LocManager.textTable.Add("Mothership Level 11 Description","Blink no longer has a range limit and decreases the cooldown");
			LocManager.textTable.Add("Mothership Level 12 Description","Increases the duration and range of Time Warp");
			LocManager.textTable.Add("Mothership Level 13 Description","Faster attack speed");
			LocManager.textTable.Add("Mothership Level 14 Description","Increases attack range");
			LocManager.textTable.Add("Mothership Level 15 Description","Detects camo bloons");
			LocManager.textTable.Add("Mothership Level 16 Description","Summon Death Fleet: Warps in 4 temporary Destroyers with beefed up range and damage");
			LocManager.textTable.Add("Mothership Level 17 Description","Decreases Time Warp cooldown");
			LocManager.textTable.Add("Mothership Level 18 Description","All bloon types can now be damaged and affected by abilities");
			LocManager.textTable.Add("Mothership Level 19 Description","Summonning the Death Fleet warps in 3 more Destroyers");
			LocManager.textTable.Add("Mothership Level 20 Description","Upgrades to a Fathership. Ultimate expression of Tal'darim might");
			return new("Mothership",14,20,1,0,10,0,false);
		}
		public override SkinData HeroSkin(){
			//won't let me add stuff by a collection init
			Il2CppSystem.Collections.Generic.List<StorePortraits>portraits=new();
			portraits.Add(new(){asset=new(){guidRef="Ui[Mothership-CorePortrait]"},levelTxt="0"});
			portraits.Add(new(){asset=new(){guidRef="Ui[Mothership-Portrait]"},levelTxt="10"});
			portraits.Add(new(){asset=new(){guidRef="Ui[Mothership-FathershipPortrait]"},levelTxt="20"});
			var skin=ScriptableObject.CreateInstance<SkinData>();
			skin.name=Name;
			skin.skinName=LocManager.GetText("Mothership Short Description");
			skin.description=LocManager.GetText("Mothership Description");
			skin.baseTowerName=Name;
			skin.mmCost=0;
			skin.icon=new(){guidRef="Ui[Mothership-Button]"};
			skin.iconSquare=new(){guidRef="Ui[Mothership-HeroIcon]"};
			skin.isDefaultTowerSkin=true;
			skin.textMaterialId="";
			skin.StorePortraitsContainer=new(){
				items=portraits
			};
			skin.unlockedVoiceSound=new(){guidRef="Mothership-Birth"};
			skin.unlockedEventSound=skin.unlockedVoiceSound;
			var gwenSkin=GameData.Instance.skinsData.SkinList.items.First(a=>a.baseTowerName=="Gwendolin");
			skin.backgroundBanner=gwenSkin.backgroundBanner;
			skin.fontMaterial=gwenSkin.fontMaterial;
			skin.backgroundColourTintOverride=gwenSkin.backgroundColourTintOverride;
			return skin;
		}
        public TowerModel Base(){
			TowerModel mothership=gameModel.GetTowerFromId("WizardMonkey").Clone<TowerModel>();
			mothership.baseId=Name;
			mothership.name=Name;
			mothership.portrait=new(){guidRef="Ui[Mothership-CorePortrait]"};
			mothership.icon=new(){guidRef="Ui[Mothership-Icon]"};
            mothership.doesntRotate=true;
			mothership.cost=975;
            mothership.radius=1;
            mothership.range=40;
			mothership.towerSet=(TowerSet)16;
            mothership.display=new(){guidRef="Mothership-Prefab"};
			mothership.tier=1;
			mothership.tiers=new(new[]{1,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 2",Name+" 2")};
			mothership.appliedUpgrades=new(0);
			List<Model>mothershipBehav=mothership.behaviors.ToList();
			DisplayModel mothershipDisplay=mothershipBehav.GetModel<DisplayModel>();
            mothershipDisplay.positionOffset=new(0,0,200);
            mothershipDisplay.display=new(){guidRef=mothership.display.guidRef};
			mothershipBehav.Add(new HeroModel("HeroModel",1,1));
            AttackModel repulsor=mothershipBehav.GetModel<AttackModel>();
            repulsor.range=mothership.range;
            repulsor.behaviors.GetModel<RotateToTargetModel>().rotateTower=false;
			WeaponModel repulsorWeapon=repulsor.weapons[0];
            repulsorWeapon.rate=0.8f;
			ProjectileModel repulsorProjectile=repulsorWeapon.projectile;
            repulsorProjectile.pierce=1;
            repulsorProjectile.behaviors.GetModel<TravelStraitModel>().speed*=2;
			DamageModel repulsorDamage=repulsorProjectile.behaviors.GetModel<DamageModel>();
            repulsorDamage.damage=1;
            repulsorDamage.immuneBloonProperties=(BloonProperties)8;
			mothership.behaviors=mothershipBehav.ToArray();
			return mothership;
        }
		public TowerModel Level2(){
			TowerModel mothership=Base().Clone<TowerModel>();
			mothership.name=Name+" 2";
			mothership.range+=7;
			mothership.tier=2;
			mothership.tiers=new(new[]{2,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 3",Name+" 3")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 2");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
            mothership.behaviors.GetModel<AttackModel>().range=mothership.range;
			return mothership;
		}
		public TowerModel Level3(){
			TowerModel mothership=Level2().Clone<TowerModel>();
			mothership.name=Name+" 3";
			mothership.tier=3;
			mothership.tiers=new(new[]{3,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 4",Name+" 4")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 3");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.behaviors.GetModel<AttackModel>().weapons[0].projectile.behaviors.GetModel<DamageModel>().damage+=1;
			return mothership;
		}
		public TowerModel Level4(){
			TowerModel mothership=Level3().Clone<TowerModel>();
			mothership.name=Name+" 4";
			mothership.tier=4;
			mothership.tiers=new(new[]{4,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 5",Name+" 5")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 4");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			AbilityModel timeWarp=BlankAbilityModel.Clone<AbilityModel>();
            timeWarp.name="Time Warp";
            timeWarp.displayName=timeWarp.name;
            timeWarp.description=LocManager.GetText("Mothership Level 4 Description").Split(':')[1].Remove(0,1);
            timeWarp.icon=new(){guidRef="Ui[Mothership-TimeWarpIcon]"};
			timeWarp.addedViaUpgrade="Mothership Level 4";
			List<Model>timeWarpBehav=timeWarp.behaviors.ToList();
            timeWarpBehav.Add(gameModel.GetTowerFromId("SuperMonkey-040").behaviors.GetModel<AbilityModel>().behaviors.GetModel<ActivateAttackModel>().Clone<ActivateAttackModel>());
			timeWarp.behaviors=timeWarpBehav.ToArray();
            ActivateAttackModel timeWarpActivateAttack=timeWarp.behaviors.GetModel<ActivateAttackModel>();
            timeWarpActivateAttack.turnOffExisting=false;
			AttackModel timeWarpAttack=gameModel.GetTowerFromId("WizardMonkey-020").behaviors.GetModel<AttackModel>("Wall").Clone<AttackModel>();
            timeWarpActivateAttack.attacks[0]=timeWarpAttack;
            timeWarpAttack.name="TimeWarp";
            timeWarpAttack.range=mothership.range;
			List<Model>timeWarpAttackBehav=timeWarpAttack.behaviors.ToList();
            timeWarpAttackBehav.RemoveModel<RotateToTargetModel>();
			timeWarpAttack.behaviors=timeWarpAttackBehav.ToArray();
			ProjectileModel timeWarpProjectile=timeWarpAttack.weapons[0].projectile;
			timeWarpProjectile.display=new(){guidRef="Mothership-TimeWarpPrefab"};
			timeWarpProjectile.behaviors.GetModel<AgeModel>().Lifespan=5;
			timeWarpProjectile.behaviors.GetModel<DisplayModel>().display=new(){guidRef=timeWarpProjectile.display.guidRef};
			List<Model>timeWarpProjectileBehav=timeWarpProjectile.behaviors.ToList();
            timeWarpProjectileBehav.RemoveModel<DamageModel>();
            timeWarpProjectileBehav.RemoveModel<CreateEffectOnExhaustedModel>();
			ProjectileFilterModel timeWarpProjFilter=timeWarpProjectile.behaviors.GetModel<ProjectileFilterModel>();
			List<FilterModel>timeWarpProjFilterList=timeWarpProjFilter.filters.ToList();
			timeWarpProjFilterList.Add(new FilterOutTagModel(null,null,null){name="FilterOutTagModel",tag="Moabs",disableWhenSupportMutatorIDs=new(0)});
            timeWarpProjFilter.filters=timeWarpProjFilterList.ToArray();
            timeWarpProjectile.collisionPasses[0]=-1;
			SlowModel slowModel=gameModel.GetTowerFromId("GlueGunner").behaviors.GetModel<AttackModel>().weapons[0].projectile.behaviors.GetModel<SlowModel>().Clone<SlowModel>();
			timeWarpProjectileBehav.Add(slowModel);
			timeWarpProjectile.behaviors=timeWarpProjectileBehav.ToArray();
            slowModel.mutationId="TimeWarp-Slow";
            slowModel.countGlueAchievement=false;
            slowModel.Lifespan=1;
            slowModel.overlayType="";
            slowModel.overlayLayer=0;
            slowModel.glueLevel=99999;
            slowModel.Multiplier=0.85f;
			SlowModel.SlowMutator slowMutator=slowModel.Mutator;
            slowMutator.id=slowModel.mutationId;
            slowMutator.glueLevel=slowModel.glueLevel;
            slowMutator.mutationId=slowModel.mutationId;
            slowMutator.overlayType=slowModel.overlayType;
            slowMutator.multiplier=slowModel.Multiplier;
			List<Model>mothershipBehav=mothership.behaviors.ToList();
            mothershipBehav.Add(timeWarp);
			mothership.behaviors=mothershipBehav.ToArray();
			return mothership;
		}
		public TowerModel Level5(){
			TowerModel mothership=Level4().Clone<TowerModel>();
			mothership.name=Name+" 5";
			mothership.tier=5;
			mothership.tiers=new(new[]{5,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 6",Name+" 6")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 5");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			WeaponModel repulsorWeapon=mothership.behaviors.GetModel<AttackModel>().weapons[0];
            repulsorWeapon.projectile.pierce+=2;
            repulsorWeapon.rate-=0.15f;
			return mothership;
		}
		public TowerModel Level6(){
			TowerModel mothership=Level5().Clone<TowerModel>();
			mothership.name=Name+" 6";
			mothership.tier=6;
			mothership.tiers=new(new[]{6,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 7",Name+" 7")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 6");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.range+=8;
			Il2CppReferenceArray<Model>mothershipBehav=mothership.behaviors;
            mothershipBehav.GetModel<AttackModel>().range=mothership.range;
            mothershipBehav.GetModel<AbilityModel>().behaviors.GetModel<ActivateAttackModel>().attacks[0].range=mothership.range;
			return mothership;
		}
		public TowerModel Level7(){
			TowerModel mothership=Level6().Clone<TowerModel>();
			mothership.name=Name+" 7";
			mothership.tier=7;
			mothership.tiers=new(new[]{7,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 8",Name+" 8")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 7");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.behaviors.GetModel<AttackModel>().weapons[0].projectile.behaviors.GetModel<DamageModel>().damage+=2;
			return mothership;
		}
		public TowerModel Level8(){
			TowerModel mothership=Level7().Clone<TowerModel>();
			mothership.name=Name+" 8";
			mothership.tier=8;
			mothership.tiers=new(new[]{8,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 9",Name+" 9")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 8");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			AbilityModel blink=BlankAbilityModel.Clone<AbilityModel>();
			List<Model>blinkBehav=blink.behaviors.ToList();
            blinkBehav.Add(gameModel.GetTowerFromId("SuperMonkey-003").behaviors.GetModel<AbilityModel>().behaviors.GetModel<DarkshiftModel>().Clone<DarkshiftModel>());
			blink.behaviors=blinkBehav.ToArray();
            DarkshiftModel teleModel=blink.behaviors.GetModel<DarkshiftModel>();
            blink.name="Blink";
            blink.displayName="Blink";
            blink.description=LocManager.GetText("Mothership Level 8 Description").Split(':')[1].Remove(0,1);;
            blink.cooldown=50;
            blink.icon=new(){guidRef="Ui[Mothership-BlinkIcon]"};
            blink.addedViaUpgrade="Mothership Level 8";
            teleModel.disappearEffectModel.assetId=new(){guidRef=""};
            teleModel.reappearEffectModel.assetId=new(){guidRef=""};
			List<Model>mothershipBehav=mothership.behaviors.ToList();
            mothershipBehav.Add(blink);
			mothership.behaviors=mothershipBehav.ToArray();
			return mothership;
		}
		public TowerModel Level9(){
			TowerModel mothership=Level8().Clone<TowerModel>();
			mothership.name=Name+" 9";
			mothership.tier=9;
			mothership.tiers=new(new[]{9,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 10",Name+" 10")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 9");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			ProjectileFilterModel filter=mothership.behaviors.GetModel<AbilityModel>("Time Warp").behaviors.GetModel<ActivateAttackModel>().attacks[0].weapons[0].projectile
                .behaviors.GetModel<ProjectileFilterModel>();
			List<FilterModel>filterList=filter.filters.ToList();
			filterList.Add(filter.filters[1].Clone<FilterModel>());
            filter.filters=filterList.ToArray();
            filter.filters[1].Cast<FilterOutTagModel>().tag="ZOMG";
            filter.filters[2].Cast<FilterOutTagModel>().tag="BAD";
			return mothership;
		}
		public TowerModel Level10(){
			TowerModel mothership=Level9().Clone<TowerModel>();
			mothership.name=Name+" 10";
			mothership.tier=10;
			mothership.tiers=new(new[]{10,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 11",Name+" 11")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 10");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.range+=50;
			Il2CppReferenceArray<Model>mothershipBehav=mothership.behaviors;
            mothershipBehav.GetModel<AbilityModel>("Time Warp").behaviors.GetModel<ActivateAttackModel>().attacks[0].range=mothership.range;
            mothershipBehav.GetModel<DisplayModel>().display=new(){guidRef=mothership.display.guidRef};
			AttackModel terminator=mothershipBehav.GetModel<AttackModel>();
            terminator.weapons[0]=gameModel.GetTowerFromId("BallOfLightTower").behaviors.GetModel<AttackModel>().weapons[0].Clone<WeaponModel>();
            WeaponModel terminatorWeapon=terminator.weapons[0];
            terminator.range=mothership.range;
            terminator.addsToSharedGrid=false;
            terminatorWeapon.ejectX=10;
            terminatorWeapon.ejectY=10;
            terminatorWeapon.rate=1;
            terminatorWeapon.projectile.behaviors.GetModel<DamageModel>().damage=20;
			List<WeaponModel>terminatorWeaponList=terminator.weapons.ToList();
            for(int i=0;i<5;i++){
                terminatorWeaponList.Add(terminatorWeapon.Clone<WeaponModel>());
            }
			terminator.weapons=terminatorWeaponList.ToArray();
            terminator.weapons[1].ejectX=0;
            terminator.weapons[2].ejectX=-10;
            terminator.weapons[5].ejectX=10;
            terminator.weapons[3].ejectY=0;
            terminator.weapons[4].ejectY=-10;
            terminator.weapons[5].ejectY=10;
			return mothership;
		}
		public TowerModel Level11(){
			TowerModel mothership=Level10().Clone<TowerModel>();
			mothership.name=Name+" 11";
			mothership.tier=11;
			mothership.tiers=new(new[]{11,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 12",Name+" 12")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 11");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			AbilityModel blink=mothership.behaviors.GetModel<AbilityModel>("Blink");
            blink.behaviors.GetModel<DarkshiftModel>().restrictToTowerRadius=false;
            blink.cooldown-=15;
			return mothership;
		}
		public TowerModel Level12(){
			TowerModel mothership=Level11().Clone<TowerModel>();
			mothership.name=Name+" 12";
			mothership.tier=12;
			mothership.tiers=new(new[]{12,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 13",Name+" 13")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 12");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			ProjectileModel timeWarpProj=mothership.behaviors.GetModel<AbilityModel>("Time Warp").behaviors.GetModel<ActivateAttackModel>().attacks[0].weapons[0].projectile;
            timeWarpProj.radius+=10;
			Il2CppReferenceArray<Model>timeWarpProjBehav=timeWarpProj.behaviors;
            timeWarpProjBehav.GetModel<AgeModel>().lifespan+=10;
            timeWarpProjBehav.GetModel<DisplayModel>().scale=1.5f;
			return mothership;
		}
		public TowerModel Level13(){
			TowerModel mothership=Level12().Clone<TowerModel>();
			mothership.name=Name+" 13";
			mothership.tier=13;
			mothership.tiers=new(new[]{13,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 14",Name+" 14")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 13");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			foreach(WeaponModel weaponModel in mothership.behaviors.GetModel<AttackModel>().weapons){
                weaponModel.rate-=0.2f;
            }
			return mothership;
		}
		public TowerModel Level14(){
			TowerModel mothership=Level13().Clone<TowerModel>();
			mothership.name=Name+" 14";
			mothership.tier=14;
			mothership.tiers=new(new[]{14,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 15",Name+" 15")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 14");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.range+=15;
			Il2CppReferenceArray<Model>mothershipBehav=mothership.behaviors;
            mothershipBehav.GetModel<AbilityModel>("Time Warp").behaviors.GetModel<ActivateAttackModel>().attacks[0].range=mothership.range;
            mothershipBehav.GetModel<AttackModel>().range=mothership.range;
			return mothership;
		}
		public TowerModel Level15(){
			TowerModel mothership=Level14().Clone<TowerModel>();
			mothership.name=Name+" 15";
			mothership.tier=15;
			mothership.tiers=new(new[]{15,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 16",Name+" 16")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 15");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			AttackModel terminator=mothership.behaviors.GetModel<AttackModel>();
            terminator.behaviors.GetModel<AttackFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive=false;
            foreach(WeaponModel weapon in terminator.weapons){
				ProjectileModel proj=weapon.projectile;
                proj.filters[0].Cast<FilterInvisibleModel>().isActive=false;
            	proj.behaviors.GetModel<ProjectileFilterModel>().filters[0].Cast<FilterInvisibleModel>().isActive=false;
            }
			return mothership;
		}
		public TowerModel Level16(){
			TowerModel mothership=Level15().Clone<TowerModel>();
			mothership.name=Name+" 16";
			mothership.tier=16;
			mothership.tiers=new(new[]{16,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 17",Name+" 17")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 16");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			AttackModel destroyerWarp=CreateTowerAttackModel.Clone<AttackModel>();
            destroyerWarp.name="DestroyerWarp";
            destroyerWarp.range=mothership.range;
			WeaponModel destroyerWarpWeapon=destroyerWarp.weapons[0];
            destroyerWarpWeapon.rate=0.025f;
			ProjectileModel destroyerWarpProj=destroyerWarpWeapon.projectile;
            destroyerWarpProj.display=new(){guidRef="Destroyer-WarpPrefab"};
			Il2CppReferenceArray<Model>destroyerWarpProjBehav=destroyerWarpProj.behaviors;
            destroyerWarpProjBehav.GetModel<DisplayModel>().display=new(){guidRef="Destroyer-WarpPrefab"};
            destroyerWarpProjBehav.GetModel<CreateTowerModel>().tower=gameModel.GetTowerFromId("Destroyer");
			AbilityModel deathFleet=BlankAbilityModel.Clone<AbilityModel>();
			deathFleet.name="Death Fleet";
            deathFleet.icon=new(){guidRef="Ui[Mothership-DeathFleetIcon]"};
            deathFleet.displayName="Summon Death Fleet";
            deathFleet.cooldown=80;
            deathFleet.addedViaUpgrade="Mothership Level 16";
			List<Model>deathFleetBehav=deathFleet.behaviors.ToList();
            deathFleetBehav.Add(gameModel.GetTowerFromId("SuperMonkey-040").behaviors.GetModel<AbilityModel>().behaviors.GetModel<ActivateAttackModel>().Clone<ActivateAttackModel>());
			ActivateAttackModel deathFleetAttack=deathFleetBehav.GetModel<ActivateAttackModel>();
            deathFleetAttack.turnOffExisting=false;
            deathFleetAttack.attacks[0]=destroyerWarp;
            deathFleetAttack.lifespan=0.1f;
            deathFleetAttack.isOneShot=false;
			deathFleet.behaviors=deathFleetBehav.ToArray();
			List<Model>mothershipBehav=mothership.behaviors.ToList();
            mothershipBehav.Add(deathFleet);
			mothership.behaviors=mothershipBehav.ToArray();
			return mothership;
		}
		public TowerModel Level17(){
			TowerModel mothership=Level16().Clone<TowerModel>();
			mothership.name=Name+" 17";
			mothership.tier=17;
			mothership.tiers=new(new[]{17,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 18",Name+" 18")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 17");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.behaviors.GetModel<AbilityModel>("Time Warp").cooldown-=20;
			return mothership;
		}
		public TowerModel Level18(){
			TowerModel mothership=Level17().Clone<TowerModel>();
			mothership.name=Name+" 18";
			mothership.tier=18;
			mothership.tiers=new(new[]{18,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 19",Name+" 19")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 18");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			Il2CppReferenceArray<Model>mothershipBehav=mothership.behaviors;
			foreach(var weapon in mothershipBehav.GetModel<AttackModel>().weapons){
                weapon.projectile.behaviors.GetModel<DamageModel>().immuneBloonProperties=0;
            }
			ProjectileModel timeWarpProj=mothershipBehav.GetModel<AbilityModel>("Time Warp").behaviors.GetModel<ActivateAttackModel>().attacks[0].weapons[0].projectile;
            List<Model>timeWarpProjBehav=timeWarpProj.behaviors.ToList();
			timeWarpProjBehav.RemoveModel<ProjectileFilterModel>();
			timeWarpProj.behaviors=timeWarpProjBehav.ToArray();
			return mothership;
		}
		public TowerModel Level19(){
			TowerModel mothership=Level18().Clone<TowerModel>();
			mothership.name=Name+" 19";
			mothership.tier=19;
			mothership.tiers=new(new[]{19,0,0});
			mothership.upgrades=new UpgradePathModel[]{new("Mothership Level 20",Name+" 20")};
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 19");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			mothership.behaviors.GetModel<AbilityModel>("Death Fleet").behaviors.GetModel<ActivateAttackModel>().attacks[0].weapons[0].rate=0.0125f;
			return mothership;
		}
		public TowerModel Level20(){
			TowerModel mothership=Level19().Clone<TowerModel>();
			mothership.name=Name+" 20";
			mothership.tier=20;
			mothership.tiers=new(new[]{20,0,0});
			mothership.upgrades=new(0);
			List<string>appliedUpgrades=mothership.appliedUpgrades.ToList();
			appliedUpgrades.Add("Mothership Level 20");
			mothership.appliedUpgrades=appliedUpgrades.ToArray();
			AttackModel carrierWarp=CreateTowerAttackModel.Clone<AttackModel>();
            carrierWarp.name="CarrierWarp";
            carrierWarp.range=mothership.range;
			WeaponModel carrierWarpWeapon=carrierWarp.weapons[0];
            carrierWarpWeapon.rate=0.06f;
			ProjectileModel carrierWarpProj=carrierWarpWeapon.projectile;
            carrierWarpProj.display=new(){guidRef="Carrier-WarpPrefab"};
			Il2CppReferenceArray<Model>carrierWarpProjBehav=carrierWarpProj.behaviors;
            carrierWarpProjBehav.GetModel<DisplayModel>().display=new(){guidRef="Carrier-WarpPrefab"};
			TowerModel carrierTower=gameModel.GetTowerFromId("Carrier");
            carrierWarpProjBehav.GetModel<CreateTowerModel>().tower=carrierTower;
			AttackModel phoenixWarp=CreateTowerAttackModel.Clone<AttackModel>();
            phoenixWarp.name="PhoenixWarp";
            phoenixWarp.range=mothership.range;
			WeaponModel phoenixWarpWeapon=phoenixWarp.weapons[0];
            phoenixWarpWeapon.rate=0.025f;
			ProjectileModel phoenixWarpProj=phoenixWarpWeapon.projectile;
            phoenixWarpProj.display=new(){guidRef="Phoenix-WarpPrefab"};
			Il2CppReferenceArray<Model>phoenixWarpProjBehav=phoenixWarpProj.behaviors;
            phoenixWarpProjBehav.GetModel<DisplayModel>().display=new(){guidRef="Phoenix-WarpPrefab"};
            phoenixWarpProjBehav.GetModel<CreateTowerModel>().tower=gameModel.GetTowerFromId("Phoenix");
			List<Model>mothershipBehav=mothership.behaviors.ToList();
			AbilityModel deathFleet=mothershipBehav.GetModel<AbilityModel>("Death Fleet");
            deathFleet.cooldown-=30;
			ActivateAttackModel deathFleetAttack=deathFleet.behaviors.GetModel<ActivateAttackModel>();
			List<AttackModel>deathFleetAttackList=deathFleetAttack.attacks.ToList();
            deathFleetAttackList.Add(carrierWarp);
            deathFleetAttackList.Add(phoenixWarp);
			deathFleetAttack.attacks=deathFleetAttackList.ToArray();
			AttackModel timeWarp=mothershipBehav.GetModel<AbilityModel>("Time Warp").behaviors.GetModel<ActivateAttackModel>().attacks[0];
            timeWarp.weapons[0].rate=14;
            timeWarp.weapons[0].projectile.radius*=2;
            timeWarp.weapons[0].projectile.behaviors.GetModel<DisplayModel>().scale*=2;
            foreach(var weapon in mothershipBehav.GetModel<AttackModel>().weapons){
                weapon.rate-=0.2f;
                weapon.projectile.behaviors.GetModel<DamageModel>().damage*=1.5f;
            }
			mothershipBehav.RemoveModel("Time Warp");
            mothershipBehav.Add(timeWarp);
            mothershipBehav.Add(carrierTower.behaviors.GetModel<AttackModel>().Clone<AttackModel>());
            mothershipBehav.GetModel<AbilityModel>("Blink").cooldown-=10;
			mothership.behaviors=mothershipBehav.ToArray();
			return mothership;
		}
		public override void Create(Tower tower){
            PlaySound("Mothership-Core"+new System.Random().Next(1,9));
        }
        public override void Upgrade(int tier,Tower tower){
            if(tier>=10){
				MothershipBehaviour behaviour=tower.Node.graphic.GetComponent<MothershipBehaviour>();
				if(tier==20){
					ActiveObj="Fathership";
					behaviour.Active.SetActive(false);
					behaviour.Active=behaviour.Fathership;
					behaviour.Active.SetActive(true);
				}
                if(behaviour.Upgraded==false){
					ActiveObj="Mothership";
                    PlaySound("Mothership-Birth");
                    behaviour.Upgraded=true;
					behaviour.Active.SetActive(false);
					behaviour.Active=behaviour.Mothership;
					behaviour.Active.SetActive(true);
                    return;
                }
                behaviour.PlayUpgradeSound();
                return;
            }
            PlaySound("Mothership-Core"+new System.Random().Next(1,9));
        }
        public override void Select(Tower tower){
			MothershipBehaviour behaviour=tower.Node.graphic.GetComponent<MothershipBehaviour>();
            if(behaviour.Upgraded){
                behaviour.PlaySelectSound();
                return;
            }
            PlaySound("Mothership-Core"+new System.Random().Next(1,9));
        }
        public override bool Ability(string abilityName,Tower tower){
			MothershipBehaviour behaviour=tower.Node.graphic.GetComponent<MothershipBehaviour>();
			PlayAnimation(behaviour.Active.GetComponent<Animator>(),"Mothership-Ability",0);
            switch(abilityName){
                case"AbilityModel_Time Warp":
                    if(behaviour.Upgraded){
                        PlaySound("Mothership-TimeWarp");
                    }else{
                        PlaySound("Mothership-Core"+new System.Random().Next(1,9));
						PlayAnimation(behaviour.Active.GetComponent<Animator>(),"Mothership-Ability",0);
                    }
                    break;
                case"AbilityModel_Blink":
                    if(behaviour.Upgraded){
                        PlaySound("Mothership-Blink");
                    }else{
                        PlaySound("Mothership-Core"+new System.Random().Next(1,9));
                    }
					PlayAnimation(behaviour.Active.GetComponent<Animator>(),"Mothership-Ability",0);
                    break;
                case"AbilityModel_Death Fleet":
                    PlaySound("Mothership-DeathFleet");
					PlayAnimation(behaviour.Active.GetComponent<Animator>(),"Mothership-Ability");
                    break;
            }
			return true;
        }
	}
}