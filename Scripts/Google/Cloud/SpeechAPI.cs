namespace DRG.Google.Cloud
{
    using System.Collections.Generic;
    using System.IO;
    using DRG.Debug;
    using DRG.Network;

    public class SpeechAPI
    {
        private const string URL_V2 = "https://www.google.com/speech-api/v2/recognize?output=json&lang={0}&key={1}";

        private ConfigGoogleCloud config;

        public IRequest Recognize(string audioFilePath, string lang, int sampleRate = 0)
        {
            string url = string.Format(URL_V2, lang, config.key);
            string audioFileName = Path.GetFileName(audioFilePath);
            string audioContentType = NetworkUtils.MapContentTypeFromPath(audioFileName) + ((sampleRate != 0) ? ("; rate=" + sampleRate) : (""));

            FileStream audioFileStream = null;
            audioFileStream = new FileStream(audioFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(audioFileStream);
            byte[] binaryData = reader.ReadBytes((int)audioFileStream.Length);

            reader.Close();
            audioFileStream.Close();

            if (null != binaryData)
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", audioContentType);

                IRequest request = new RequestWWW(url, binaryData, headers);

                return request;
            }
            else
            {
                Log.Error("binaryData is null");
                return null;
            }
        }
    }
}