using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicturePerfect.Models
{
    public class Locations
    {
        /// <summary>
        /// Get the list of currently available locations.
        /// </summary>
        public ObservableCollection<Location> List { get; private set; } = new();

        /// <summary>
        /// Class for locations associated with image files.
        /// </summary>
        public class Location
        {
            /// <summary>
            /// Get or set the location id.
            /// </summary>
            public int Id { get; private set; }
            /// <summary>
            /// Get or set the location name.
            /// </summary>
            public string Name { get; private set; } = string.Empty;
            /// <summary>
            /// Get or set the geo tag for this location. E.g. coordinates.
            /// </summary>
            public string GeoTag { get; private set; } = string.Empty;
            /// <summary>
            /// Get or set the notex for this location.
            /// </summary>
            public string Notes { get; private set; } = string.Empty;

            /// <summary>
            /// Creates a new instance of the location class.
            /// </summary>
            public Location()
            {

            }

            /// <summary>
            /// Method to create a new location object by inserting data from the sqlite database.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="name"></param>
            /// <param name="geoTag"></param>
            /// <param name="notes"></param>
            /// <returns>Returns the location object.</returns>
            public static Location NewFromDatabase(int id, string name, string geoTag, string notes)
            {
                Location location = new()
                {
                    Id = id,
                    Name = name,
                    GeoTag = geoTag,
                    Notes = notes
                };

                return location;
            }

            /// <summary>
            /// Method to create an entry for this instance in the sqlite table.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="geoTag"></param>
            /// <param name="notes"></param>
            /// <returns>Returns the location object.</returns>
            public static Location Create(string name, string geoTag, string notes)
            {
                Location location = new()
                {
                    Name = name,
                    GeoTag = geoTag,
                    Notes = notes
                };
                Database.AddLocation(location);

                return location;
            }
        }

        /// <summary>
        /// Method to load the location list from the database.
        /// </summary>
        public void LoadList()
        {
            List.Clear();
            List<Location> locations = Database.LoadAllLocations();

            // repopulate list with location objects
            locations.ForEach(location => List.Add(location));
        }
    }
}
