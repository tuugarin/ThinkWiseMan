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
        public IAsyncOperation<IEnumerable<WiseIdeaModel>> GetThoughtsByAuthorNameAsync(string author)
        {
            return Task.Run<IEnumerable<WiseIdeaModel>>(async () =>
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
                        Author = thought.Element("Author").Value
                    };

                return thoughts;
            }).AsAsyncOperation();




        }

        public IAsyncOperation<IEnumerable<WiseIdeaModel>> GetThoughtsByDayAsync(int day, int month)
        {
            return Task.Run<IEnumerable<WiseIdeaModel>>(async () =>
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
                        Author = thought.Element("Author").Value
                    };
                return thoughts;
            }).AsAsyncOperation();

        }
        IAsyncOperation<string> GetFileContent()
        {
            return Task.Run<string>(async () =>
            {
                var currentFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                var dataFolder = await currentFolder.GetFolderAsync("Data");
                var file = await dataFolder.GetFileAsync("thoughts.xml");
                var content = await FileIO.ReadTextAsync(file);
                return content;
            }).AsAsyncOperation<string>();

        }

    }

}


