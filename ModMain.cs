[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(Mothership.ModMain),Mothership.ModHelperData.Name,Mothership.ModHelperData.Version,"Silentstorm")]
[assembly:MelonOptionalDependencies("SC2ExpansionLoader")]
namespace Mothership{
    public class ModMain:MelonMod{
        public static string LoaderPath=MelonEnvironment.ModsDirectory+"/SC2ExpansionLoader.dll";
        public override void OnEarlyInitializeMelon(){
            if(!File.Exists(LoaderPath)){
                var httpClient=new HttpClient();
                var stream=httpClient.GetStreamAsync("https://github.com/Onixiya/SC2Expansion/releases/latest/download/SC2ExpansionLoader.dll");
                var fileStream=new FileStream(LoaderPath,FileMode.CreateNew);
                stream.Result.CopyToAsync(fileStream);
                Log("Restarting game so MelonLoader correctly loads all mods");
                Application.Quit();
            }
        }
    }
}