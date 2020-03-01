﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace RestaurantBookingSkill.Models.Action
{
    public class BookingInfo
    {
        [JsonProperty("foodType")]
        public string FoodType { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("attendeeCount")]
        public int AttendeeCount { get; set; }

        [JsonProperty("bookingPlace")]
        public string BookingPlace { get; set; }
    }
}
