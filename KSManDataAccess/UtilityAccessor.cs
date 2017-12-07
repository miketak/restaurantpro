using DataObjects.ProgramDataObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UtilityAccessor
    {
        // Country Utility Methods
        /// <summary>
        /// Data Access Method to Retrieve Countries from Database
        /// </summary>
        /// <returns>List of Countries</returns>
        public static List<Country> RetrieveCountries()
        {
            var countriesInDB = new List<Country>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_country_by_country_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CountryID", SqlDbType.Int);
            cmd.Parameters["@CountryID"].Value = -1;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();


                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        var country = new Country()
                        {
                            CountryID = reader.GetInt32(0),
                            ISO = reader.IsDBNull(1) ? null : reader.GetString(1),
                            Name = reader.IsDBNull(2) ? null : reader.GetString(2),
                            NiceName = reader.IsDBNull(3) ? null : reader.GetString(3),
                            ISO3 = reader.IsDBNull(4) ? null : reader.GetString(4),
                            NumCode = reader.IsDBNull(5) ? -1 : reader.GetInt32(5),
                            PhoneCode = reader.IsDBNull(6) ? -1 : reader.GetInt32(6)
                        };
                        countriesInDB.Add(country);
                    }
                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving countries list data." + ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return countriesInDB;

        }

        /// <summary>
        /// Retrieves Country By ID
        /// </summary>
        /// <param name="countryID"></param>
        /// <returns>Country Object</returns>
        public static Country RetrieveCountryByID(int countryID)
        {
            //Retrieve country by Id
            Country country = null;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_country_by_country_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@CountryID", SqlDbType.Int);
            cmd.Parameters["@CountryID"].Value = countryID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    country = new Country()
                    {
                        CountryID = reader.GetInt32(0),
                        ISO = reader.GetString(1),
                        Name = reader.GetString(2),
                        NiceName = reader.GetString(3),
                        ISO3 = reader.GetString(4),
                        NumCode = reader.GetInt32(5),
                        PhoneCode = reader.GetInt32(6)
                    };
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return country;
        }

        // State Utility Methods
        /// <summary>
        /// Retrieves state by ID
        /// </summary>
        /// <param name="stateID">ID for Query</param>
        /// <param name="retrieveAll">Signal to retrieve all</param>
        /// <returns></returns>
        public static State RetrieveStateByID(int stateID)
        {
            //Retrieve country by Id
            State state = null;

            var conn = DBConnection.GetConnection();
            var cmdText = @"sp_retrieve_state_by_state_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@StateID", SqlDbType.Int);
            cmd.Parameters["@StateID"].Value = stateID;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    state = new State()
                    {
                        StateID = reader.GetInt32(0),
                        StateCode = reader.GetString(1),
                        StateName = reader.GetString(2)
                    };
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return state;
        }


        /// <summary>
        /// Retrieves States Data into List
        /// </summary>
        /// <returns>States List</returns>
        public static List<State> RetrieveStates()
        {
            var statesList = new List<State>();

            //Getting connection
            var conn = DBConnection.GetConnection();

            //Using stored procedure
            var cmdText = @"sp_retrieve_state_by_state_id";
            var cmd = new SqlCommand(cmdText, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@StateID", SqlDbType.Int);
            cmd.Parameters["@StateID"].Value = -1;

            try
            {
                // you have to open a connection before using it
                conn.Open();

                // create a data reader with our command
                var reader = cmd.ExecuteReader();
                State state;

                // check to make sure the reader has data
                if (reader.HasRows)
                {
                    // process the data reader
                    while (reader.Read())
                    {
                        state = new State()
                        {
                            StateID = reader.GetInt32(0),
                            StateCode = reader.GetString(1),
                            StateName = reader.GetString(2)
                        };
                        statesList.Add(state);
                    }

                    // close the reader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException("There was a problem retrieving states data.", ex);
            }
            finally
            {
                // final housekeeping
                conn.Close();
            }

            return statesList;

        }


    }
}
