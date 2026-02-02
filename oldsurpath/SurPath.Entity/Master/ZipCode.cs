using System.Collections.Generic;

namespace SurPath.Entity.Master
{
    public class ZipCode
    {
        #region Private Variables

        private int _zipId;
        private string _zip;
        private string _city;
        private string _state;
        private List<string> _nearestZip = new List<string>();

        #endregion Private Variables

        #region Public Properties

        public int ZipId
        {
            get
            {
                return this._zipId;
            }
            set
            {
                this._zipId = value;
            }
        }

        public string Zip
        {
            get
            {
                return this._zip;
            }
            set
            {
                this._zip = value;
            }
        }

        public string City
        {
            get
            {
                return this._city;
            }
            set
            {
                this._city = value;
            }
        }

        public string State
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
            }
        }

        public List<string> NearestZip
        {
            get
            {
                return this._nearestZip;
            }
            set
            {
                this._nearestZip = value;
            }
        }

        #endregion Public Properties
    }
}