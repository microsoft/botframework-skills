﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace CalendarSkill.Models.ActionInfos
{
    public class SummaryResult : ActionResult
    {
        [JsonProperty("eventList")]
        public List<EventInfo> EventList { get; set; }
    }
}
