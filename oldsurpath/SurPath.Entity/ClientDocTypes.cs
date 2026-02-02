using System;

namespace SurPath.Entity
{
    /// <summary>
    /// Cleint Address entity
    /// </summary>
    public class ClientDocTypes
    {
        #region Private Variables

        private int _clientDocTypeId;
        private int _clientDepartmentId;
        private string _description;
        private string _instructions;
        private DateTime _duedate;
        private string _semester;
        private bool _isnotifyStudent;
        private int _notifyDays1;
        private int _notifyDays2;
        private int _notifyDays3;
        private bool _doesExpire;
        private bool _isrequired;
        private bool _isArchived;
        private DateTime _createdOn;

        #endregion Private Variables

        #region Public Properties

        public int ClientDoctypeId
        {
            get
            {
                return this._clientDocTypeId;
            }
            set
            {
                this._clientDocTypeId = value;
            }
        }

        public int ClientDepartmentId
        {
            get
            {
                return this._clientDepartmentId;
            }
            set
            {
                this._clientDepartmentId = value;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public string Instructions
        {
            get
            {
                return this._instructions;
            }
            set
            {
                this._instructions = value;
            }
        }

        public DateTime DueDate
        {
            get
            {
                return this._duedate;
            }
            set
            {
                this._duedate = value;
            }
        }

        public string Semester
        {
            get
            {
                return this._semester;
            }
            set
            {
                this._semester = value;
            }
        }

        public bool IsNotifyStudent
        {
            get
            {
                return this._isnotifyStudent;
            }
            set
            {
                this._isnotifyStudent = value;
            }
        }

        public int NotifyDays1
        {
            get
            {
                return this._notifyDays1;
            }
            set
            {
                this._notifyDays1 = value;
            }
        }

        public int NotifyDays2
        {
            get
            {
                return this._notifyDays2;
            }
            set
            {
                this._notifyDays2 = value;
            }
        }

        public int NotifyDays3
        {
            get
            {
                return this._notifyDays3;
            }
            set
            {
                this._notifyDays3 = value;
            }
        }

        public bool IsDoesExpire
        {
            get
            {
                return this._doesExpire;
            }
            set
            {
                this._doesExpire = value;
            }
        }

        public bool IsRequired
        {
            get
            {
                return this._isrequired;
            }
            set
            {
                this._isrequired = value;
            }
        }

        public bool IsArchived
        {
            get
            {
                return this._isArchived;
            }
            set
            {
                this._isArchived = value;
            }
        }

        public DateTime CreatedOn
        {
            get
            {
                return this._createdOn;
            }
            set
            {
                this._createdOn = value;
            }
        }

        #endregion Public Properties
    }
}