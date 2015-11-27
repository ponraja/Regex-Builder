using Microsoft.Practices.Prism.PubSubEvents;
using RegexBuilder.Infrastructure.Events;
using RegexBuilder.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RegexBuilder.Infrastructure.Utility
{
    public class LibraryRepository
    {
        private string DataFilePath;
        bool isInitialized;
        XDocument librarydoc;
        ObservableCollection<RegularExpression> _regularExpression;
        IEventAggregator eventAggregator;

        public ObservableCollection<RegularExpression> RegularExpressions
        {
            get 
            {
                if (_regularExpression == null && !isInitialized)
                {
                    InitializeRepository();
                    isInitialized = true;
                }
                return _regularExpression;
            }
            set { _regularExpression = value; }
        }

        public LibraryRepository(IEventAggregator eventAggregator)
        {
            this.eventAggregator = eventAggregator;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                DataFilePath = ad.DataDirectory;
            }
            else
            {
                DataFilePath = @"..\..\Data\RegexLibrary.xml";
            }
            librarydoc = XDocument.Load(DataFilePath);
        }

        public async Task InitializeRepository()
        {
            //_regularExpression = await GetDataAsync();
            _regularExpression = GetData();
            isInitialized = true;
        }

        public bool AddRegularExpression(RegularExpression exp)
        {
            XElement element = new XElement("RegularExpression");
            var id = GetLastId();
            id++;
            element.Add(new XElement("Id", id));
            element.Add(new XElement("Title", exp.Title));
            element.Add(new XElement("Expression", exp.Expression));
            element.Add(new XElement("Description", exp.Description));
            element.Add(new XElement("Matches", exp.Matches));
            element.Add(new XElement("NonMatches", exp.NonMatches));

            librarydoc.Element("RegularExpressions").Add(element);
            SetLastId(id);
            SaveFile(NotifyCollectionChangedAction.Add);
            return true;
        }

        private int GetLastId()
        {
            var id = librarydoc.Element("RegularExpressions").Attribute("LastId").Value;
            return Convert.ToInt32(id);
        }

        private void SetLastId(int id)
        {
            librarydoc.Element("RegularExpressions").Attribute("LastId").SetValue(id);
        }

        private void SaveFile(NotifyCollectionChangedAction action)
        {
            librarydoc.Save(DataFilePath);
            InitializeRepository();
            eventAggregator.GetEvent<UpdateRegularExpressionCollectionEvent>().Publish(action);
        }

        public RegularExpression GetRegularExpressionById(string id)
        {
            var exp = this.RegularExpressions.Where(e => e.Id.Equals(id));
            if(exp.Any())
                return exp.FirstOrDefault();
            return null;
        }

        public bool UpdateRegularExpression(RegularExpression exp)
        {
            var target = librarydoc.Element("RegularExpressions").Elements("RegularExpression").Where(item => item.Element("Id").Value.Equals(exp.Id)).FirstOrDefault();
            if(target != null)
            {
                target.Element("Title").Value = exp.Title;
                target.Element("Expression").Value = exp.Expression;
                target.Element("Description").Value = exp.Description;
                target.Element("Matches").Value = exp.Matches;
                target.Element("NonMatches").Value = exp.NonMatches;
                SaveFile(NotifyCollectionChangedAction.Replace);
            }
            return true;
        }

        public bool DeleteRegularExpression(RegularExpression exp)
        {
            var target = librarydoc.Element("RegularExpressions").Elements("RegularExpression").Where(item => item.Element("Id").Value.Equals(exp.Id)).FirstOrDefault();
            if (target != null)
            {
                target.Remove();
                SaveFile(NotifyCollectionChangedAction.Remove);
            }
            return true;
        }

        private Task<ObservableCollection<RegularExpression>> GetDataAsync()
        {
            return Task.Run(() => GetData());
        }

        private ObservableCollection<RegularExpression> GetData()
        {
            var data = from item in librarydoc.Descendants("RegularExpression")
                       select new RegularExpression
                       {
                           Id = item.Element("Id").Value,
                           Title = item.Element("Title").Value,
                           Expression = item.Element("Expression").Value,
                           Description = item.Element("Description").Value,
                           Matches = item.Element("Matches").Value,
                           NonMatches = item.Element("NonMatches").Value
                       };
            return data.ToObservableCollection();
        }
    }
}
