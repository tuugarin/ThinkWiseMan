using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using ThinkWiseMan.Models;
using Windows.Storage;

namespace ThinkWiseMan.Services
{
    public class XmlDataService : IXmlDataService
    {

        public async Task<IEnumerable<WiseIdeaModel>> GetThoughtsByAuthorName(string author)
        {
                string content = await GetFileContent();
                XElement root = XElement.Parse(content);
                IEnumerable<WiseIdeaModel> thoughts =
                    from thought in root.Elements()
                    where thought.Element("Author").Value == author
                    select new WiseIdeaModel
                    {
                        Content = thought.Element("Content").Value,
                        Day = int.Parse(thought.Element("Day").Value),
                        Month = int.Parse(thought.Element("Month").Value),
                        Author = thought.Element("Author").Value
                    };

                return thoughts;
        
          

        }

        public async Task<IEnumerable<WiseIdeaModel>> GetThoughtsByDay(int day, int month)
        {
            string content = await GetFileContent();
            XElement root = XElement.Parse(content);

            IEnumerable<WiseIdeaModel> thoughts =
                from thought in root.Elements()
                where thought.Element("Day").Value == day.ToString() && thought.Element("Month").Value == month.ToString()
                select new WiseIdeaModel
                {
                    Content = thought.Element("Content").Value,
                    Day = int.Parse(thought.Element("Day").Value),
                    Month = int.Parse(thought.Element("Month").Value),
                    Author = thought.Element("Author").Value
                };
            return thoughts;
        }
        async Task<string> GetFileContent()
        {

            var currentFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            var dataFolder = await currentFolder.GetFolderAsync("Data");
            var file = await dataFolder.GetFileAsync("thoughts.xml");
            return await FileIO.ReadTextAsync(file);
        }
    }
}
