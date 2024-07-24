using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AKingCard
{
    public class SaveJsonManager: MonoBehaviour
    {
        public static SaveJsonManager instance { private set; get; }
        SaveJsonManager()
        {
            instance = this;
        }
        string savePath = Application.streamingAssetsPath;
        string savePathTemplate = Application.streamingAssetsPath + "/Template";
        string savePathCard = Application.streamingAssetsPath + "/Card";

        Dictionary<string, DataTemplate> templatePathPairs = new Dictionary<string, DataTemplate>();
        public List<DataTemplate> ReadTemplates()
        {
            List<DataTemplate> templateList = new List<DataTemplate>();
            DirectoryInfo saveFolderInfo = new DirectoryInfo(savePathTemplate);
            if (!saveFolderInfo.Exists)
            {
                saveFolderInfo.Create();    //不存在目录就新建一个
                return templateList;
            }
            FileInfo[] fileInfos = saveFolderInfo.GetFiles();
            if (fileInfos == null || fileInfos.Length <= 1)
                return templateList;
            templatePathPairs.Clear();
            for (int i = 0; i < fileInfos.Length; i++)
            {
                if (fileInfos[i].Extension == ".json")
                {
                    string jsonText = File.ReadAllText(fileInfos[i].FullName);
                    DataTemplate thisTemplate = JsonUtility.FromJson<DataTemplate>(jsonText);
                    templateList.Add(thisTemplate);
                    templatePathPairs.Add(fileInfos[i].FullName, thisTemplate);
                }
            }

            return templateList;
        }
        public void DeleteTemplate(DataTemplate data)
        {
            if (templatePathPairs.Count > 0)
            {
                foreach (var pair in templatePathPairs)
                {
                    if(pair.Value.index == data.index)
                    {
                        new FileInfo(pair.Key).Delete();
                        templatePathPairs.Remove(pair.Key);
                        return;
                    }
                }
            }
        }
        public DataTemplate ReadTemplate(string index)
        {
            List<DataTemplate> templateList = ReadTemplates();
            
            for (int i = 0; i < templateList.Count; i++)
            {
                if (templateList[i].index == index)
                {
                    return templateList[i];
                }
            }
            return null;
        }
        public void SaveTemplate(DataTemplate data)
        {
            DirectoryInfo saveFolderInfo = new DirectoryInfo(savePathTemplate);
            if (!saveFolderInfo.Exists)
                saveFolderInfo.Create();    //不存在目录就新建一个

            string thisTemplatePath = savePathTemplate + $"/{data.index}-{data.name}.json";
            //新文件全地址
            FileStream mFileStream = new FileStream(thisTemplatePath, FileMode.Create, FileAccess.Write);
            StreamWriter mStreamWriter = new StreamWriter(mFileStream);

            string templateJsonString = JsonUtility.ToJson(data);
            mStreamWriter.Write(templateJsonString);    //写入Json数据
            mStreamWriter.Close();

            return;
        }

    }
}

