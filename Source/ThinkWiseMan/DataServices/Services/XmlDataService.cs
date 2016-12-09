using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataServices.Models;
using Windows.Storage;
using Windows.Foundation;
using System.IO;

namespace DataServices
{
    public sealed class XmlDataService : IXmlDataService
    {
        static string xmlFileName = "thoughts.xml";
        static string xmlRelativePath = Path.Combine("Data", xmlFileName);
        public static async Task<IEnumerable<WiseIdea>> GetAllThoughts()
        {

            var content = await GetContent();
            XElement root = XElement.Parse(content);

            IEnumerable<WiseIdea> thoughts =
                from thought in root.Elements()
                select new WiseIdea
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                };
            return thoughts;


        }

        public async Task<IEnumerable<WiseIdeaPresentModel>> GetThoughtsByAuthorNameAsync(string author)
        {

            var content = await GetFileLocalContent();
            XElement root = XElement.Parse(content);
            IEnumerable<WiseIdeaPresentModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Author").Value == author
                select new WiseIdeaPresentModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                    Id = int.Parse(thought.Element("ID").Value)
                };

            return thoughts;




        }
        public async Task<IEnumerable<WiseIdeaPresentModel>> GetThoughtsByDayAsync(int day, int month)
        {

            var content = await GetFileLocalContent();
            XElement root = XElement.Parse(content);

            IEnumerable<WiseIdeaPresentModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Day").Value == day.ToString() && thought.Element("Month").Value == month.ToString()
                select new WiseIdeaPresentModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                    Id = int.Parse(thought.Element("ID").Value)
                };
            return thoughts;


        }
        public async Task<IEnumerable<WiseIdeaPresentModel>> GetFavouritesThoughts()
        {

            var content = await GetFileLocalContent();
            XElement root = XElement.Parse(content);

            IEnumerable<WiseIdeaPresentModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Favorite").Value == bool.TrueString
                select new WiseIdeaPresentModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                    Id = int.Parse(thought.Element("ID").Value)
                };
            return thoughts;


        }
        public async Task AddDeleteFavorite(int id, bool isFavorite)
        {
            var content = await GetFileLocalContent();
            XElement root = XElement.Parse(content);

            var wiseElement = root.Elements().SingleOrDefault(e => e.Element("ID").Value == id.ToString());
            if (wiseElement != null)
            {
                wiseElement.Element("Favorite").Value = isFavorite.ToString();
                await SaveFileContent(root.ToString());
            }

        }

        public async static Task<string> GetContent()
        {
            return await Task.Run(() =>
             {
                 var path = Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, xmlRelativePath);
                 return File.ReadAllText(path);
             });

        }

        async Task EnsureXmlFileExist()
        {
            await Task.Run(async () =>
            {
                var xmlPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, xmlFileName);
                if (!File.Exists(xmlPath))
                {
                    string content = await GetContent();
                    var localFolder = ApplicationData.Current.LocalFolder;
                    File.WriteAllText(localFolder.Path, xmlFileName);
                }
            }
             );
        }

        async Task<string> GetFileLocalContent()
        {
            await EnsureXmlFileExist();
            var content = await Task.Run(() =>
            {
                var localFolder = ApplicationData.Current.LocalFolder;
                var path = Path.Combine(localFolder.Path, xmlFileName);
                return File.ReadAllText(path);
            });
            return content;

            //if (await localFolder.TryGetItemAsync("thoughts.xml") == null)
            //{
            //    var currentFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //    var dataFolder = await currentFolder.GetFolderAsync("Data");
            //    var file = await dataFolder.GetFileAsync("thoughts.xml");
            //    var content = await FileIO.ReadTextAsync(file);
            //    var localFile = await localFolder.CreateFileAsync("thoughts.xml");
            //    await FileIO.WriteTextAsync(localFile, content);
            //    return content;
            //}
            //else
            //{
            //    var file = await localFolder.GetFileAsync("thoughts.xml");
            //    var content = await FileIO.ReadTextAsync(file);
            //    return content;
            //}

        }
        async Task SaveFileContent(string content)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync("thoughts.xml");
            await FileIO.WriteTextAsync(file, content);

        }
    }

}


