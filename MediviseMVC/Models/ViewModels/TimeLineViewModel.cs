using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MediviseMVC.Models.ViewModels
{
    public class TimeLineViewModel
    {
        public TimeLineViewModel(ICollection<Drug> drugs, ICollection<Test> tests, ICollection<Message> msgs)
        {
            this.Drugs = drugs;
            this.Tests = tests;
            this.Messages = msgs;
        }
        public ICollection<Drug> Drugs { get; private set; }
        public ICollection<Test> Tests { get; private set; }
        public ICollection<Message> Messages { get; private set; }
    }
}
