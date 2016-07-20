using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using DataServices.Models;
using Windows.Storage;
using Windows.Foundation;

namespace DataServices
{
    public sealed class XmlDataService : IXmlDataService
    {
        public async Task<IEnumerable<WiseIdeaModel>> GetThoughtsByAuthorNameAsync(string author)
        {

            var content = await GetFileContent();
            XElement root = XElement.Parse(content);
            IEnumerable<WiseIdeaModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Author").Value == author
                select new WiseIdeaModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                    Id = thought.Attribute("ID").Value
                };

            return thoughts;




        }
        public async Task<IEnumerable<WiseIdeaModel>> GetThoughtsByDayAsync(int day, int month)
        {

            var content = await GetFileContent();
            XElement root = XElement.Parse(content);

            IEnumerable<WiseIdeaModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Day").Value == day.ToString() && thought.Element("Month").Value == month.ToString()
                select new WiseIdeaModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                    Id = thought.Attribute("ID").Value
                };
            return thoughts;


        }
        public async Task<IEnumerable<WiseIdeaModel>> GetFavouritesThoughts()
        {

            var content = await GetFileContent();
            XElement root = XElement.Parse(content);

            IEnumerable<WiseIdeaModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Favorite").Value == bool.TrueString
                select new WiseIdeaModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value,
                    IsFavorite = bool.Parse(thought.Element("Favorite").Value),
                    Id = thought.Attribute("ID").Value
                };
            return thoughts;


        }
        public async Task AddDeleteFavorite(string id, bool isFavorite)
        {
            var content = await GetFileContent();
            XElement root = XElement.Parse(content);

            var wiseElement = root.Elements().SingleOrDefault(e => e.Attribute("ID").Value == id);
            if (wiseElement != null)
            {
                wiseElement.Element("Favorite").Value = isFavorite.ToString();
                await SaveFileContent(root.ToString());
            }

        }

        async Task<string> GetFileContent()
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            if (await localFolder.TryGetItemAsync("thoughts.xml") == null)
            {
                var currentFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var dataFolder = await currentFolder.GetFolderAsync("Data");
                var file = await dataFolder.GetFileAsync("thoughts.xml");
                var content = await FileIO.ReadTextAsync(file);
                var localFile = await localFolder.CreateFileAsync("thoughts.xml");
                await FileIO.WriteTextAsync(localFile, content);
                return content;
            }
            else
            {
                var file = await localFolder.GetFileAsync("thoughts.xml");
                var content = await FileIO.ReadTextAsync(file);
                return content;
            }

        }
        async Task SaveFileContent(string content)
        {
            var localFolder = ApplicationData.Current.LocalFolder;
            var file = await localFolder.GetFileAsync("thoughts.xml");
            await FileIO.WriteTextAsync(file, content);

        }
    }

}


