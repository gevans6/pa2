using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace PA2WebApp
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        private static Trie myTrie = new Trie();
        private static List<string> wikiList = new List<string>();
        private static string finalPath;

        int counter = 0;
        [WebMethod]
        public string HelloWorld()
        {
            counter++;
            return "Hello World" + counter;
        }


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<string> searchTrie(string input)
        {
            input = input.ToLower();
            List<string> answers = myTrie.SearchAll(input);
            return answers;
        }

        [WebMethod]
        public string DownloadFile()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.AppSettings["StorageConnectionString"]);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("pa2");

            if (container.Exists())
            {
                foreach (IListBlobItem item in container.ListBlobs(null, false))
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;


                        using (var fileStream = System.IO.File.OpenWrite(Path.GetTempFileName() + "\\wikipedia.txt"))
                        {
                            blob.DownloadToStream(fileStream);
                        }

                        finalPath = System.IO.Path.GetTempFileName() + "\\wikipedia.txt";

                        return (System.IO.Path.GetTempFileName() + "\\wikipedia.txt");
                    }
                }
            }

            return null;
        }

        [WebMethod]
        public string BuildTrie()
        {
            var stream = finalPath;
            {
                using (StreamReader reader = new StreamReader(stream))
                {

                    while (!reader.EndOfStream)
                    {
                        string title = reader.ReadLine();
                        myTrie.InsertWord(title);
                    }
                }
            }
            return null;
        }
    }
}
