using DRG.Debug;

namespace DRG.Network
{
    public static class NetworkUtils
    {

        /// <summary>
        /// Content-Type based on the file path.
        /// </summary>
        /// <param name="filepath">file path</param>
        public static string MapContentTypeFromPath(string filepath)
        {
            string extension = System.IO.Path.GetExtension(filepath);

            switch (extension)
            {
                case ".amr":
                    return "audio/amr";
                case ".wav":
                    return "audio/wav";
                case ".spx":
                    return "audio/x-speex";
            }

            Log.Error("Unsupported extension \"" + extension + "\"");
            return "";
        }
    }
}
