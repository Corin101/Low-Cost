using System;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;

namespace low_cost
{
    /// <summary>
    /// Origin and Destination data
    /// </summary>
    class Airport
    {
        protected string origin;
        protected string destination;
        protected string departureDate;
        protected string arrivalDate;
        protected string iataOrigin;
        protected string iataArrival;
        protected int passengers;
        protected string currency;

        public string Origin
        {
            get { return origin; }
            set { origin = value; iataOrigin = SetIata(value); }
        }
        public string Destination
        {
            get { return destination; }
            set { destination = value; iataArrival = SetIata(value); }
        }
        public string IataOrigin
        {
            get { return iataOrigin; }
        }
        public string IataArrival
        {
            get { return iataArrival; }
        }
        public int Passengers
        {
            get { return passengers; }
            set { passengers = value; }
        }
        public string DepartureDate
        {
            get { return departureDate; }
            set { departureDate = value; }
        }
        public string ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }
        public string Currency
        {
            get { return currency; }
            set { currency = value; }
        }
        // Convert airfield name into IATA code
        public string SetIata(string airportName)
        {
            string connectionString = low_cost.Properties.Settings.Default.iataConnectionString;
            string upit = "SELECT code FROM iata_airport_codes WHERE airport = " + "'" + airportName + "'";
            SqlConnection sqlConnection1 = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = upit;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            reader = cmd.ExecuteReader();
            reader.Read();
            string iata = reader.GetString(0);
            sqlConnection1.Close();
            return iata;
            }
    }
    /// <summary>
    /// Creating of an URL string for Amadeus
    /// </summary>
    class CreateUrl
        {
            private string url = "http://api.sandbox.amadeus.com/v1.2/flights/low-fare-search?";

            protected void setString(Airport data)
            {
                string apikey = "apikey="+ GetKey() + "&";
                string ori = "origin=" + data.IataOrigin + "&";
                string des = "destination=" + data.IataArrival + "&";
                string tim = "departure_date=" + data.DepartureDate + "&";
                string pas = "adults=" + data.Passengers + "&";
                url = url + apikey + ori + des + tim + pas;
            if (data.Currency != null)
            {
                string sur = "¤cy=" + data.Currency + " HTTP / 1.1";
                url += sur;
            }
            else
            {
                url += " HTTP / 1.1";
            }
                
            }
            public string Url 
            {
                get {return url;}
            }
        protected string GetKey()
        {
            BinaryReader reader;
            try
            {
                reader = new BinaryReader(new FileStream("key.bin", FileMode.Open));
            }
            catch (IOException excep)
            {
                Console.WriteLine(excep.Message);
                Console.WriteLine("Error openinig file key.bin");
                return null ;
            }
            string key = reader.ReadString();
            reader.Close();
            return key;
        }
         }
    /// <summary>
    /// Compare current query with local database querries
    /// </summary>
    class CompareQuery : CreateUrl
            {
        protected string connectionString = low_cost.Properties.Settings.Default.iataConnectionString;
        public bool CompareData(Airport data)
        {    
            SqlConnection connect = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("SELECT Origin, Destination, DateOfTravel, Passengers, Currency FROM QuerryTable", connect);
            SqlDataReader reader = query.ExecuteReader();
            if (reader.HasRows)
            {
                if (reader.GetString(reader.GetOrdinal("Origin")) != data.IataOrigin)
                    return false;
                if (reader.GetString(reader.GetOrdinal("Destination")) != data.IataArrival)
                    return false;
                if (string.Compare(data.DepartureDate, reader.GetString(reader.GetOrdinal("DateOfTravel"))) != 0)
                    return false;
                if (reader.GetInt32(reader.GetOrdinal("Passengers")) != data.Passengers)
                    return false;
                if (reader.GetString(reader.GetOrdinal("Currency")) != data.Currency)
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
        public void SaveQuerylData(Airport data)
        {
            SqlConnection connect = new SqlConnection(connectionString);
            SqlCommand query = new SqlCommand("INSERT INTO QueryTable (Origin, Destination, DateOfTravel, Passengers, Currency, ID) VALUES (@Origin, @Destination, @DateOfTravel, @Passengers, @Currency, @ID)");
            query.CommandType = CommandType.Text;
            query.Connection = connect;
            query.Parameters.AddWithValue("@Origin", data.IataOrigin);
            query.Parameters.AddWithValue("@Destination", data.IataArrival);
            query.Parameters.AddWithValue("@DateOfTravel", data.DepartureDate);
            query.Parameters.AddWithValue("@Passengers", data.Passengers);
            query.Parameters.AddWithValue("@Currency", data.Currency);
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
        public void send(Airport data)
        {
            setString(data);

            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
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
    static class LoadComboBox
    {
        public static void Set(ref System.Windows.Forms.ComboBox comboBox)
        {
            string connetionString = null;
            SqlConnection connection;
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataSet ds = new DataSet();
            string sql = null;
            connetionString = low_cost.Properties.Settings.Default.iataConnectionString;
            sql = "select airport AS name from iata_airport_codes";
            connection = new SqlConnection(connetionString);
            connection.Open();
            command = new SqlCommand(sql, connection);
            adapter.SelectCommand = command;
            adapter.Fill(ds);
            adapter.Dispose();
            command.Dispose();
            connection.Close();
            comboBox.DataSource = ds.Tables[0];
            comboBox.DisplayMember = "name";
        }
    }

}
