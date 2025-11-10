using HMSDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMSBusinessLayer
{
    public class Country
    {
        public int ID { set; get; }
        public string CountryName { set; get; }

        public Country()

        {
            this.ID = -1;
            this.CountryName = "";

        }

        private Country(int ID, string CountryName)

        {
            this.ID = ID;
            this.CountryName = CountryName;
        }

        public static Country Find(int ID)
        {
            string CountryName = "";

            if (CountryDataAccess.GetCountryInfoByID(ID, ref CountryName))

                return new Country(ID, CountryName);
            else
                return null;

        }

        public static Country Find(string CountryName)
        {

            int ID = -1;

            if (CountryDataAccess.GetCountryInfoByName(CountryName, ref ID))

                return new Country(ID, CountryName);
            else
                return null;

        }

        public static DataTable GetAllCountries()
        {
            return CountryDataAccess.GetAllCountries();

        }
    }
}
