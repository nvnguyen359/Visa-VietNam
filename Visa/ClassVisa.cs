using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Visa
{
    public static class ClassVisa
    {
        public static int MoneyVisa(string visit, string typeOfvisa, int numberpax)
        {
            int money = 0;
            using (VisaDataContext mydata= new VisaDataContext())
            {
                var kt =
                    mydata.ViSaPart1s.FirstOrDefault(j => j.visit.Contains(visit) && j.typeOfVisa.Contains(typeOfvisa));
                var heading = mydata.HEADINGs.FirstOrDefault(j => j.min <= numberpax && numberpax <= j.max.Value);
                if (kt != null)
                {
                    switch (numberpax)
                    {
                        case 1:
                            {
                                money = kt.pax1.Value;
                            }
                            break;
                        case 10:
                            {
                                money = kt.pax4.Value;
                            }
                            break;
                        default:
                            {
                                if (heading != null)
                                {
                                    switch (heading.idpax)
                                    {
                                        case 2:
                                            {
                                                money = kt.pax2.Value;
                                            }
                                            break;
                                        case 3:
                                            {
                                                money = kt.pax3.Value;
                                            }
                                            break;
                                    }
                                }
                               
                            }
                            break;
                    }
                }
              
            }
            return money;
        }

        //public static int FeeMoneyVisa(string processingtime ,string visit,int numberPax)
        //{
        //    var kt = new FEEVISA();
        //    using (VisaDataContext myData= new VisaDataContext())
        //    {
        //        try
        //        {
        //            kt = myData.FEEVISAs.FirstOrDefault(j => j.minPax >= numberPax
        //            && j.vist.Contains(purposeofvisit)
        //            && j.typeofVisa.Contains(typeofVisa)
        //            && j.serviceFee.Contains(processingtime));
        //        }
        //        catch (Exception)
        //        {

        //            kt = myData.FEEVISAs.FirstOrDefault(j => j.minPax <= numberPax && numberPax <= j.maxPax.Value
        //              && j.vist.Contains(purposeofvisit)
        //            && j.typeofVisa.Contains(typeofVisa)
        //            && j.serviceFee.Contains(processingtime));
        //        }
        //    }
        //}

    }
}