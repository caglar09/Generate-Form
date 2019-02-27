using System.Collections.Generic;

namespace AutoGenerateForm.ViewModel
{
    public class FormModel
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<Fileds> fields { get; set; }
    }

    public class Fileds
    {
        public bool required { get; set; }
        public string name { get; set; }
        public string dataType { get; set; }
    }
}