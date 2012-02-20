using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TBTracker.Models
{
    public class Seeder : DropCreateDatabaseIfModelChanges<TrackerEntities>
    {
        protected override void Seed(TrackerEntities context)
        {
            context.SaveChanges();
        }
    }
}