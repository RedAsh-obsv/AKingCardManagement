using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Excel;
using System.IO;
using System.Text;

public class ReadTable : MonoBehaviour
{
    public string tableInStreamingAssets;
    private string outputInStreamingAssets;
    void Start()
    {
        //TableToList();
        tableInStreamingAssets = Application.streamingAssetsPath + "\\" + tableInStreamingAssets;
        outputInStreamingAssets = Application.streamingAssetsPath + "\\";
        ListToTxt(TableToList());
    }

    public List<List<string>> TableToList()
    {
        FileStream fileStream = File.Open(tableInStreamingAssets, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelDataReader = ExcelReaderFactory.CreateBinaryReader(fileStream);
        DataSet result = excelDataReader.AsDataSet();

        // 获取表格有多少列 
        int columns = result.Tables[0].Columns.Count;
        // 获取表格有多少行 
        int rows = result.Tables[0].Rows.Count;

        Debug.Log($"{columns} x { rows}");
        List<List<string>> tableData = new List<List<string>>();

        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < rows; i++)
        {
            List<string> rowData = new List<string>();
            for (int j = 1; j < columns; j++)
            {
                var cell = result.Tables[0].Rows[i][j].ToString();
                rowData.Add(cell);
                stringBuilder.Append(cell + " ");
            }
            stringBuilder.Append("\n");
            tableData.Add(rowData);
        }
        fileStream.Close();
        //Debug.Log(stringBuilder.ToString());
        return tableData;
    }

    public void ListToTxt(List<List<string>> tableData)
    {
        for (int i = 0; i < tableData[0].Count; i++)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("namespace MoonXR.I18N.Impl\n" +
                                "{\n" +
                                $"    public partial class {tableData[0][i]}Language : ILanguage\n" +
                                "    {\n");
            for (int j = 1; j < tableData.Count; j++)
            {
                if (tableData[j][0] != "")
                {
                    string content = "";
                    if (tableData[j][i] != "")
                        content = tableData[j][i];
                    else
                        content = tableData[j][1];
                    content = content.Replace("\"", "\\\"");
                    content = content.Replace("\\\\\"", "\\\"");
                    content = content.Replace("\n", "\\n");
                    stringBuilder.Append($"        public string {tableData[j][0]} => \"{content }\";\n");
                }
            }
            stringBuilder.Append("    }\n" +
                                "}" );
            Debug.Log(stringBuilder.ToString());

            string path = outputInStreamingAssets + $"\\{tableData[0][i]}Language.cs";
            // 文件流创建一个文本文件
            FileStream file = new FileStream(path, FileMode.Create);
            //得到字符串的UTF8 数据流
            byte[] bts = System.Text.Encoding.UTF8.GetBytes(stringBuilder.ToString());
            // 文件写入数据流
            file.Write(bts, 0, bts.Length);
            if (file != null)
            {
                file.Flush();
                file.Close();
                file.Dispose();
            }
        }
    }
}
