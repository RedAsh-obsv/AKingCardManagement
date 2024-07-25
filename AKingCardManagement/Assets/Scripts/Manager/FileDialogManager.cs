using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using NativeFileBrowser;

namespace AKingCard
{
    public class FileDialogManager: MonoBehaviour
    {
        public static FileDialogManager instance { private set; get; }
        FileDialogManager()
        {
            instance = this;
        }

        public string OpenFile()
        {
            var title = "Open Image File";
            var extensions = new[]
            {
                new ExtensionFilter("Image Files", "png", "jpg", "jpeg"),
                new ExtensionFilter("JPG ", "jpg", "jpeg"),
                new ExtensionFilter("PNG ", "png"),
            };

            var path = StandaloneFileBrowser.OpenFilePanel(title, extensions, false);
            if (path != null && path.Length > 0)
                return path[0];
            else
                return string.Empty;
        }
    }
}
