using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace low_cost
{
    public class RootObject
    {
        public string Currency { get; set; }
        public List<Result> Results { get; set; }
    }
    public class Result
    {
        public List<Itinerary> Itineraries { get; set; }
        public Fare Fare { get; set; }
    }
    public class Itinerary
    {
        public Outbound Outbound { get; set; }
        public Inbound Inbound { get; set; }
    }
    public class Outbound
    {
        public List<Flight> Flights { get; set; }
    }
    public class Flight
    {
        public string DepartsAt { get; set; }
        public string ArrivesAt { get; set; }
        public Origin Origin { get; set; }
        public Destination Destination { get; set; }
        public string MarketingAirline { get; set; }     
        public string OperatingAirline { get; set; }  
        public string FlightNumber { get; set; } 
        public string Aircraft { get; set; }
        public Booking_info BookingInfo { get; set; }
    }
    public class Origin
    {
        public string Airport { get; set; }
        public string Terminal { get; set; }
    }
    public class Destination
    {
        public string Airport { get; set; }
        public string Terminal { get; set; }
    }
    public class Booking_info
    {
        public string TravelClass { get; set; }
        public string BookingCode { get; set; }
        public int SeatsRemaining { get; set; }
    }
    public class Inbound
    {
        public List<Flight> Flights { get; set; }
    }
    public class Fare
    {
        public string TotalPrice { get; set; }
        public Price_per_adult PricePerAdult { get; set; }
        public Restrictions Restrictions { get; set; }
    }
    public class Price_per_adult
    {
        public string TotalFare { get; set; }
        public string Tax { get; set; }
    }
    public class Restrictions
    {
        public bool Refundable { get; set; }
        public bool ChangePenalties { get; set; }
    }

}
