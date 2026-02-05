using System.Collections.Generic;

namespace SurPath.Entity.Master
{
    public class NearestZipCode
    {
        #region Private Variables

        private string _donorZip;
        private string _nearestVendorZip;
        private List<int> _nearestZip = new List<int>();

        #endregion Private Variables

        #region Public Properties

        public string DonorZip
        {
            get
            {
                return this._donorZip;
            }
            set
            {
                this._donorZip = value;
            }
        }

        public string NearestVendorZip
        {
            get
            {
                return this._nearestVendorZip;
            }
            set
            {
                this._nearestVendorZip = value;
            }
        }

        public List<int> NearestZip
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