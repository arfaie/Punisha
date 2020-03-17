using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Models
{
    public class FieldTypeModel
    {
        public List<Types> Types { get; set; }
        public string SelectedType { get; set; }
    }
    public class Types
    {
        public short Id { get; set; }
        public string Title { get; set; }
    }
}