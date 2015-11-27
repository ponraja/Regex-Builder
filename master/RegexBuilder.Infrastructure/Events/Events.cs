using Microsoft.Practices.Prism.Events;
using RegexBuilder.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegexBuilder.Infrastructure.Events
{
    public class UpdateRegularExpressionCollectionEvent : CompositePresentationEvent<NotifyCollectionChangedAction>
    {

    }

    public class SelectedRegularExpressionEvent : CompositePresentationEvent<RegularExpression>
    {

    }
}
