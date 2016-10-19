using System;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Data;

namespace low_cost
{
    /// <summary>
    /// Origin and Destination data
    /// </summary>
    class Airport
    {
        protected string airportName;
        protected string iata;
        protected DateTime time;
        protected uint passengers;
        protected string currency;
        protected uint id; 

        public void SetData(string name, DateTime t, uint pass, string curr,uint ident)
        {
            airportName = name;
            time = t;
            passengers = pass;
            currency = curr;
            iata = SetIata(airportName);
            id = ident;
        }
        public string Name
        {
            get { return airportName; }
            set { airportName = value; }
        }
        public string AirportIata
        {
            get { return iata; }
            set { iata = SetIata(value); }
        }
        public DateTime Date
        {
            get { return time; }
            set { time = value; }
        }
        public uint PassengerCount
        {
            get { return passengers; }
            set { passengers = value; }
        }
        public string CurrencyUsed
        {
            get { return currency; }
            set { currency = value; }
        }
        public uint QueryId
        {
            get { return id; }
            set { id = value; }
        }
        // Convert airfield name into IATA code
        protected string SetIata(string airportName)
        {
            string connectionString = low_cost.Properties.Settings.Default.iataConnectionString;
            SqlConnection connect = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("SELECT code FROM iata_airport_codes WHERE airport = " + airportName, connect);
            SqlDataReader reader = query.ExecuteReader();
            string iata = reader.GetString(reader.GetOrdinal("code"));
            return iata;
        }
    }
    /// <summary>
    /// Creating of an URL string for Amadeus
    /// </summary>
    class CreateUrl
        {
            protected string url = "http://api.sandbox.amadeus.com/v1.2/flights/low-fare-search?";

            protected void setString(Airport origin, Airport destination)
            {
                string apikey = "apikey=YQGzMuqt1UQXZBrmRAZO250tS1j1sFBQ" + "&";
                string ori = "origin=" + origin.AirportIata + "&";
                string des = "destination=" + destination.AirportIata + "&";
                string tim = "departure_date=" + origin.Date + "&";
                string pas = "adults=" + origin.PassengerCount + "&";
                string sur = "¤cy=" + origin.CurrencyUsed + " HTTP / 1.1";
                url = url + apikey + ori + des + tim + pas + sur;
            }
            public string GetData()
            {
                return url;
            }
        }
    /// <summary>
    /// Compare current query with local database querries
    /// </summary>
    class CompareQuery : CreateUrl
            {
        protected string connectionString = low_cost.Properties.Settings.Default.iataConnectionString;
        public bool CompareData(Airport origin, Airport destination)
        {    
            SqlConnection connect = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("SELECT Origin, Destination, DateOfTravel, Passengers, Currency FROM QuerryTable", connect);
            SqlDataReader reader = query.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.GetString(reader.GetOrdinal("Origin")) != origin.AirportIata)
                    return false;
                if (reader.GetString(reader.GetOrdinal("Destination")) != destination.AirportIata)
                    return false;
                if (DateTime.Compare(origin.Date, reader.GetDateTime(reader.GetOrdinal("DateOfTravel"))) != 0)
                    return false;
                if (reader.GetInt32(reader.GetOrdinal("Passengers")) != origin.PassengerCount)
                    return false;
                if (reader.GetString(reader.GetOrdinal("Currency")) != origin.CurrencyUsed)
                    return false;
            }
            else
                return false;
            return true;
        }
            }
    /// <summary>
    /// Save query to database. 
    /// Save results from Amadeus to database
    /// </summary>
    class SaveData : CompareQuery
    {
        public void SaveQuerylData(Airport origin, Airport destination)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("INSERT INTO QueryTable (Origin, Destination, DateOfTravel, Passengers, Currency, ID) VALUES (@Origin, @Destination, @DateOfTravel, @Passengers, @Currency, @ID)");
            query.CommandType = CommandType.Text;
            query.Connection = connect;
            query.Parameters.AddWithValue("@Origin", origin.AirportIata);
            query.Parameters.AddWithValue("@Destination", destination.AirportIata);
            query.Parameters.AddWithValue("@DateOfTravel", origin.Date);
            query.Parameters.AddWithValue("@Passengers", origin.PassengerCount);
            query.Parameters.AddWithValue("@Currency", origin.CurrencyUsed);
            query.Parameters.AddWithValue("@ID", origin.QueryId);
            connect.Open();
            query.ExecuteNonQuery();
        }
        public void SaveResultData()
        {
            // TODO
            // data collected from Amadeus saved to a local db
            // data saved with the same id as query
        }
    }
    /// <summary>
    /// Send query to Amadeus, receive query result from Amadeus
    /// </summary>
    class SendHttpRequest : CreateUrl
    {
        protected string html = string.Empty;
        public void send(Airport origin, Airport destination)
        {
            setString(origin, destination);

            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
        }
    }
    /// <summary>
    /// Parsing of collected result into relevat data
    /// </summary>
    class ParseData : SendHttpRequest
    {
        //parse relevant data
        // transform json data into c# class data
    }
    /// <summary>
    /// Displays the data
    /// </summary>
    class DisplayData
    {
        //  TODO
        //     Display data from local db
        //     concept: get id from query, get saved data by id, display it
    }
}
