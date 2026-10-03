using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ForceURPAssetsUpgrade : IPreprocessBuildWithReport
{
  public int callbackOrder => -100; // Se ejecuta ANTES de que valide URP

  public void OnPreprocessBuild(BuildReport report)
  {
    Debug.Log("Forzando reimportación y guardado de URP Assets...");
    string[] assetPaths = new string[]
    {
      "Assets/Settings/Mobile_RPAsset.asset",
      "Assets/Settings/PC_RPAsset.asset",
      "Assets/Settings/UniversalRenderPipelineGlobalSettings.asset"
    };

    foreach (string path in assetPaths)
      AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
    AssetDatabase.SaveAssets();
  }
}
