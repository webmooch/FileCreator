using System;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;

namespace FileCreator.Models
{
    [DataContract]
    internal class VersionData
    {
        [DataMember(Name = "v")]
        public string latestVersion { get; private set; }

        [IgnoreDataMember]
        public Version LatestVersion
        {
            get
            {
                Version version;
                return Version.TryParse(latestVersion, out version) ? version : null;
            }
        }

        [DataMember(Name = "d")]
        public string DownloadUri { get; private set; }

        [IgnoreDataMember]
        public bool IsValid
        {
            get
            {
                Version test;
                return Uri.IsWellFormedUriString(DownloadUri, UriKind.Absolute) && Version.TryParse(latestVersion, out test);
            }
        }

        [IgnoreDataMember]
        public Version AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version;
            }
        }

        [IgnoreDataMember]
        public bool NewVersionAvailable
        {
            get
            {
                return (AssemblyVersion.CompareTo(LatestVersion) < 0);
            }
        }

        public static VersionData GetVersionData(string staticVersionUri)
        {
            using (var webClient = new WebClient())
            {
                var dynamicVersionLink = webClient.DownloadString(staticVersionUri).Trim();

                if (!Uri.IsWellFormedUriString(dynamicVersionLink, UriKind.Absolute))
                    throw new ArgumentException("Improperly formatted dynamic version link.");

                var json = webClient.DownloadString(dynamicVersionLink);

                return json.DeserializeAsJsonObject<VersionData>();
            }
        }
    }
}
