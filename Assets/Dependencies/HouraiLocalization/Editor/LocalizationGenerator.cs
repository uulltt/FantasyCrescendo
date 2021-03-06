using System.Collections.Generic;
using Google.GData.Spreadsheets;
using HouraiTeahouse.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace HouraiTeahouse.Localization.Editor {

    /// <summary> A Editor-Only ScriptableObject for pulling localization data from Google Spreadsheets and creating the
    /// approriate Langauge assets. </summary>
    public class LocalizationGenerator : ScriptableObject {

        const string DefaultStoragePath = "Assets/Resources/Lang";

        [SerializeField]
        [Tooltip("The public Google Spreadsheets link to pull data from")]
        string GoogleLink;

        [SerializeField]
        [Tooltip("Columns in the spreadsheet to ignore")]
        string[] _ignoreColumns;

        [SerializeField]
        [Tooltip("The folder to save all of the generated assets into.")]
        Object _saveFolder;

        [MenuItem("Hourai/Localization/Generate")]
        static void Create() {
            var generator = Assets.LoadOrCreate<LocalizationGenerator>();
            Assert.IsNotNull(generator);
            if (generator)
                generator.Generate();
        }

        /// <summary> Reads the Google Spreadsheet and generates/updates the StringSet asset files </summary>
        public void Generate() {
            ListFeed test = GDocService.GetSpreadsheet(GoogleLink);
            var languageMap = new Dictionary<string, StringSet>();
            var ignore = new HashSet<string>(_ignoreColumns);
            foreach (ListEntry row in test.Entries) {
                foreach (ListEntry.Custom element in row.Elements) {
                    string lang = element.LocalName;
                    if (ignore.Contains(lang))
                        continue;
                    if (!languageMap.ContainsKey(lang))
                        languageMap[lang] = CreateInstance<StringSet>();
                    languageMap[lang].Add(element.Value);
                }
            }
            string folderPath = _saveFolder ? AssetDatabase.GetAssetPath(_saveFolder) : DefaultStoragePath;
            foreach (KeyValuePair<string, StringSet> lang in languageMap) {
                var method = "Generating";
                string path = string.Format("{0}/{1}.asset", folderPath, lang.Key);
                var language = AssetDatabase.LoadAssetAtPath<StringSet>(path);
                if (language) {
                    method = "Updating";
                    language.Copy(lang.Value);
                    EditorUtility.SetDirty(language);
                }
                else
                    AssetDatabase.CreateAsset(lang.Value, path);
                Log.Info("{0} language files for: {1}", method, lang.Key);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.SaveAssets();
        }

    }

}
