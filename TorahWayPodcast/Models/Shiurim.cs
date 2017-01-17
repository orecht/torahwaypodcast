using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TorahWayPodcast.Models
{
    /// <summary>
    /// List of shiurim. This is the container for the entire data model. 
    /// HashSet because RSS spec says there can;t be doublons in an RSS feed. Equality criteria for 2 items is URL as per RSS spec
    /// </summary>
    [Serializable]
    public class Shiurim : HashSet<Shiur>
    { }
}