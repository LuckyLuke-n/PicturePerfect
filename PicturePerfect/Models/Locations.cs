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
            /// Get the location id.
            /// </summary>
            public int Id { get; private set; }
            /// <summary>
            /// Get or set the location name.
            /// </summary>
            public string Name { get; set; } = string.Empty;
            /// <summary>
            /// Get or set the geo tag for this location. E.g. coordinates.
            /// </summary>
            public string GeoTag { get; set; } = string.Empty;
            /// <summary>
            /// Get or set the notex for this location.
            /// </summary>
            public string Notes { get; set; } = string.Empty;

            /// <summary>
            /// Creates a new instance of the location class.
            /// </summary>
            public Location()
            {

            }

            /// <summary>
            /// Method to create an entry for this instance in the sqlite table.
            /// </summary>
            public void Create()
            {
                Database.AddLocation(this);
            }

            /// <summary>
            /// Method to set the private property Id.
            /// </summary>
            /// <param name="id"></param>
            public void SetId(int id)
            {
                Id = id;
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
            List.Add(new Location() { Name = "None" });
            locations.ForEach(location => List.Add(location));
        }
    }
}
