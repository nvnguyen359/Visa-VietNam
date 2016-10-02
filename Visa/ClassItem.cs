using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Visa
{
    public class ClassItem
    {
    }

    public class DropDown
    {
        public int Value { get; set; }
        public string Text { get; set; }

        public List<DropDown> GetDropDown(List<ViSaPart1> l)
        {
            List<DropDown> k= new List<DropDown>();
            using (VisaDataContext mydata= new VisaDataContext())
            {
                foreach (var w in l)
                {
                    var kt = mydata.TTypeofvisas.FirstOrDefault(j => j.name.Contains(w.typeOfVisa));
                    if (kt != null)
                    {
                        k.Add(new DropDown() {Text = kt.name, Value = (int)kt.value.Value});
                    }
                }
            }
            return k;
        }
    }
}