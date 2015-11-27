using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegexBuilder.Infrastructure.Models
{
    public class RegularExpression : BindableBase
    {
        string id;
        string title;
        string expression;
        string description;
        string matches;
        string nonMatches;
        string camelCaseTitle;
        string titleWithoutSpecialChar;

        public string Id
        {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        public string Title
        {
            get { return title; }
            set 
            { 
                SetProperty(ref title, value);
                camelCaseTitle = null;
                titleWithoutSpecialChar = null;
            }
        }

        public string Expression
        {
            get { return expression; }
            set { SetProperty(ref expression, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public string Matches
        {
            get { return matches; }
            set { SetProperty(ref matches, value); }
        }

        public string NonMatches
        {
            get { return nonMatches; }
            set { SetProperty(ref nonMatches, value); }
        }

        public string CamelCaseTitle
        {
            get
            {
                if (camelCaseTitle == null)
                {
                    var res = Title.Where(ch => Char.IsUpper(ch));
                    camelCaseTitle = new string(res.ToArray());
                }
                return camelCaseTitle;
            }
        }

        public string TitleWithoutSpecialChar
        {
            get
            {
                if (titleWithoutSpecialChar == null)
                {
                    titleWithoutSpecialChar = Regex.Replace(Title, @"[^0-9a-zA-Z]+", "");
                }
                return titleWithoutSpecialChar;
            }
        }
    }
}
