using DataAccessLayer;
using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /// <summary>
    /// Manages program wide most used utilities
    /// </summary>
    public class UtilityManager
    {

        /// <summary>
        /// Retrieves all countriesInDB from database
        /// </summary>
        /// <returns>Returns a list of Country Objects</returns>
        public List<Country> RetrieveCountries()
        {

            List<Country> countries = UtilityAccessor.RetrieveCountries();

            return countries;

        }

        /// <summary>
        /// Retrieves States
        /// </summary>
        /// <returns>Returns states</returns>
        public List<State> RetrieveStates()
        {
            List<State> stateList = new List<State>();

            stateList = UtilityAccessor.RetrieveStates();

            return stateList;
        }


    }
}
