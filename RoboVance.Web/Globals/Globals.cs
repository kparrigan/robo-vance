using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RoboVance.Web
{
    public static class Globals
    {
        // TODO something better than this
        public static ConcurrentDictionary<Guid, String> AgentConnectionIds = new ConcurrentDictionary<Guid, String>();
        public static ConcurrentDictionary<Guid, DateTime> AgentTimestamps = new ConcurrentDictionary<Guid, DateTime>();
        public static ConcurrentDictionary<Guid, IList<String>> AgentDevices = new ConcurrentDictionary<Guid, IList<String>>();
    }
}