namespace SurPath.Entity
{
    public class DonorAccounting
    {
        #region Private Variables

        private double _uaRevenue = 0;
        private double _hairRevenue = 0;
        private double _dnaRevenue = 0;
        private double _laboratoryCost = 0;
        private double _mroCost = 0;
        private double _cupCost = 0;
        private double _shippingCost = 0;
        private double _vendorCost = 0;
        //private double _totalRevenue = 0;
        //private double _totalCost = 0;
        //private double _grossProfit = 0;

        private double _cumulativeUARevenue = 0;
        private double _cumulativeHairRevenue = 0;
        private double _cumulativeDNARevenue = 0;
        private double _cumulativeLaboratoryCost = 0;
        private double _cumulativeMROCost = 0;
        private double _cumulativeCupCost = 0;
        private double _cumulativeShippingCost = 0;
        private double _cumulativeVendorCost = 0;
        //private double _cumulativeTotalRevenue = 0;
        //private double _cumulativeTotalCost = 0;
        //private double _cumulativegrossProfit = 0;

        #endregion Private Variables

        #region Public Properties

        public double UARevenue
        {
            get
            {
                return this._uaRevenue;
            }
            set
            {
                this._uaRevenue = value;
            }
        }

        public double HairRevenue
        {
            get
            {
                return this._hairRevenue;
            }
            set
            {
                this._hairRevenue = value;
            }
        }

        public double DNARevenue
        {
            get
            {
                return this._dnaRevenue;
            }
            set
            {
                this._dnaRevenue = value;
            }
        }

        public double LaboratoryCost
        {
            get
            {
                return this._laboratoryCost;
            }
            set
            {
                this._laboratoryCost = value;
            }
        }

        public double MROCost
        {
            get
            {
                return this._mroCost;
            }
            set
            {
                this._mroCost = value;
            }
        }

        public double CupCost
        {
            get
            {
                return this._cupCost;
            }
            set
            {
                this._cupCost = value;
            }
        }

        public double ShippingCost
        {
            get
            {
                return this._shippingCost;
            }
            set
            {
                this._shippingCost = value;
            }
        }

        public double VendorCost
        {
            get
            {
                return this._vendorCost;
            }
            set
            {
                this._vendorCost = value;
            }
        }

        public double TotalRevenue
        {
            get
            {
                return this._uaRevenue + this._hairRevenue + this._dnaRevenue;
            }
        }

        public double TotalCost
        {
            get
            {
                return this._laboratoryCost + this._mroCost + this._cupCost + this._shippingCost + this._vendorCost;
            }
        }

        public double GrossProfit
        {
            get
            {
                return this.TotalRevenue - this.TotalCost;
            }
        }

        public double CumulativeUARevenue
        {
            get
            {
                return this._cumulativeUARevenue;
            }
            set
            {
                this._cumulativeUARevenue = value;
            }
        }

        public double CumulativeHairRevenue
        {
            get
            {
                return this._cumulativeHairRevenue;
            }
            set
            {
                this._cumulativeHairRevenue = value;
            }
        }

        public double CumulativeDNARevenue
        {
            get
            {
                return this._cumulativeDNARevenue;
            }
            set
            {
                this._cumulativeDNARevenue = value;
            }
        }

        public double CumulativeLaboratoryCost
        {
            get
            {
                return this._cumulativeLaboratoryCost;
            }
            set
            {
                this._cumulativeLaboratoryCost = value;
            }
        }

        public double CumulativeMROCost
        {
            get
            {
                return this._cumulativeMROCost;
            }
            set
            {
                this._cumulativeMROCost = value;
            }
        }

        public double CumulativeCupCost
        {
            get
            {
                return this._cumulativeCupCost;
            }
            set
            {
                this._cumulativeCupCost = value;
            }
        }

        public double CumulativeShippingCost
        {
            get
            {
                return this._cumulativeShippingCost;
            }
            set
            {
                this._cumulativeShippingCost = value;
            }
        }

        public double CumulativeVendorCost
        {
            get
            {
                return this._cumulativeVendorCost;
            }
            set
            {
                this._cumulativeVendorCost = value;
            }
        }

        public double CumulativeTotalRevenue
        {
            get
            {
                return this._cumulativeUARevenue + this._cumulativeHairRevenue + this._cumulativeDNARevenue;
            }
        }

        public double CumulativeTotalCost
        {
            get
            {
                return this._cumulativeLaboratoryCost + this._cumulativeMROCost + this._cumulativeCupCost + this._cumulativeShippingCost + this._cumulativeVendorCost;
            }
        }

        public double CumulativeGrossProfit
        {
            get
            {
                return this.CumulativeTotalRevenue - this.CumulativeTotalCost;
            }
        }

        #endregion Public Properties
    }
}